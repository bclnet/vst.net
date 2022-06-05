using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using IOPath = System.IO.Path;

namespace Jacobi.Vst3.Host
{
    public class ModuleWin32 : Module
    {
        static readonly string ArchitectureString = Environment.Is64BitOperatingSystem ? "x86_64-win" : "x86-win";

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)] public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)] public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll", SetLastError = true)][return: MarshalAs(UnmanagedType.Bool)] public static extern bool FreeLibrary(IntPtr hModule);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate bool InitModuleFunc();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate bool ExitModuleFunc();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate IPluginFactory GetFactoryProc();

        IntPtr mModule;

        T GetFunctionPointer<T>(string name) where T : Delegate
        {
            var intPtr = GetProcAddress(mModule, name);
            if (intPtr == IntPtr.Zero) return default;
            return Marshal.GetDelegateForFunctionPointer(intPtr, typeof(T)) as T;
        }

        public override void Dispose()
        {
            if (mModule != IntPtr.Zero)
            {
                // ExitDll is optional
                var dllExit = GetFunctionPointer<ExitModuleFunc>("ExitDll");
                dllExit?.Invoke();
                FreeLibrary(mModule);
            }
        }

        protected override bool Load(string inPath, out string errorDescription)
        {
            var path = $"{inPath}\\Contents\\{ArchitectureString}\\{IOPath.GetFileName(inPath)}";
            mModule = LoadLibrary(path);
            if (mModule == IntPtr.Zero)
            {
                mModule = LoadLibrary(inPath);
                if (mModule == IntPtr.Zero)
                {
                    var lastError = Marshal.GetLastWin32Error();
                    var msg = new Win32Exception(lastError).Message;
                    errorDescription = $"LoadLibray failed: {msg}";
                    return false;
                }
            }
            var factoryProc = GetFunctionPointer<GetFactoryProc>("GetPluginFactory");
            if (factoryProc == null)
            {
                errorDescription = "The dll does not export the required 'GetPluginFactory' function";
                return false;
            }
            // InitDll is optional
            var dllEntry = GetFunctionPointer<InitModuleFunc>("InitDll");
            if (dllEntry != null)
            {
                try
                {
                    if (!dllEntry())
                    {
                        errorDescription = "Calling 'InitDll' failed";
                        return false;
                    }
                }
                catch (SEHException e)
                {
                    errorDescription = "Calling 'InitDll' failed";
                    return false;
                }
            }
            var f = factoryProc() as IPluginFactory;
            if (f == null)
            {
                errorDescription = "Calling 'GetPluginFactory' returned nullptr";
                return false;
            }
            _factory = f;
            errorDescription = default;
            return true;
        }

        static bool CheckVST3Package(string p, out string result)
        {
            var path = $"{p}/Contents/{ArchitectureString}/{IOPath.GetFileName(p)}";
            var hFile = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (hFile != null)
            {
                hFile.Close();
                result = path;
                return true;
            }
            result = default;
            return false;
        }

        static bool IsFolderSymbolicLink(string p) => false;

        static string GetKnownFolder(Environment.SpecialFolder folder) => Environment.GetFolderPath(folder);
        static string ResolveShellLink(string p) => null;

        static void FindFilesWithExt(string path, string ext, List<string> pathList, bool recursive = true)
        {
            if (recursive)
                foreach (var p in Directory.GetDirectories(path)) FindFilesWithExt(p, ext, pathList, recursive);
            foreach (var p in Directory.GetFiles(path, $"*.{ext}")) if (CheckVST3Package(p, out _)) pathList.Add(p);
        }

        static void FindModules(string path, List<string> pathList)
        {
            if (File.Exists(path)) FindFilesWithExt(path, ".vst3", pathList);
        }

        static string GetContentsDirectoryFromModuleExecutablePath(string modulePath)
        {
            var path = IOPath.GetDirectoryName(modulePath);
            if (IOPath.GetFileName(path) != ArchitectureString) return null;
            path = IOPath.GetDirectoryName(path);
            if (IOPath.GetFileName(path) != "Contents") return null;
            return path;
        }

        internal new static Module Create(string path, out string errorDescription)
        {
            var module = new ModuleWin32();
            if (module.Load(path, out errorDescription))
            {
                module.Path = path;
                module.Name = IOPath.GetFileName(path);
                return module;
            }
            return null;
        }

        internal new static List<string> GetModulePaths()
        {
            string knownFolder;
            // find plug-ins located in common/VST3
            var list = new List<string>();
            if ((knownFolder = GetKnownFolder(Environment.SpecialFolder.CommonProgramFiles)) != null) FindModules($"{knownFolder}VST3", list);
            //if ((knownFolder = GetKnownFolder(Environment.SpecialFolder.CommonProgramFiles)) != null) FindModules($"{knownFolder}VST3", list);

            // find plug-ins located in VST3 (application folder)
            var path = IOPath.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FindModules($"{path}VST3", list);
            return list;
        }

        internal new static string GetModuleInfoPath(string modulePath)
        {
            var path = GetContentsDirectoryFromModuleExecutablePath(modulePath);
            if (path == null)
            {
                if (!CheckVST3Package(modulePath, out var p)) return null;
                path = IOPath.GetDirectoryName(IOPath.GetDirectoryName(p));
            }
            path = $"{path}/moduleinfo.json";
            return File.Exists(path) ? path : null;
        }

        internal new static List<Snapshot> GetSnapshots(string modulePath)
        {
            var result = new List<Snapshot>();
            var path = GetContentsDirectoryFromModuleExecutablePath(modulePath);
            if (path == null)
            {
                if (!CheckVST3Package(modulePath, out var p)) return result;
                path = IOPath.GetDirectoryName(IOPath.GetDirectoryName(p));
            }
            path = $"{path}/Resources/Snapshots";
            if (!File.Exists(path)) return result;

            var pngList = new List<string>();
            FindFilesWithExt(path, ".png", pngList, false);
            foreach (var png in pngList)
            {
                var filename = IOPath.GetFileName(png);
                var uid = Snapshot.DecodeUID(filename);
                if (uid == null) continue;
                var decodedScaleFactor = Snapshot.DecodeScaleFactor(filename);
                var scaleFactor = decodedScaleFactor != null ? decodedScaleFactor.Value : 1D;
                var desc = new Snapshot.ImageDesc
                {
                    ScaleFactor = scaleFactor,
                    Path = png,
                };
                var found = false;
                foreach (var entry in result)
                {
                    if (entry.Uid != uid.Value) continue;
                    found = true;
                    entry.Images.Add(desc);
                    break;
                }
                if (found) continue;
                var snapshot = new Snapshot { Uid = uid.Value };
                snapshot.Images.Add(desc);
                result.Add(snapshot);
            }
            return result;
        }
    }
}
