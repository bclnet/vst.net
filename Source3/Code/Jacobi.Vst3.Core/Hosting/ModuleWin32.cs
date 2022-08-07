﻿using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using _Path = System.IO.Path;

namespace Jacobi.Vst3.Hosting
{
    public class ModuleWin32 : Module
    {
        //static readonly string ArchitectureString = Environment.Is64BitOperatingSystem ? "x86_64-win" : "x86-win";
#if X86
        static readonly string ArchitectureString = "x86-win";
#endif
#if X64
        static readonly string ArchitectureString = "x86_64-win";
#endif

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)] public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)] public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll", SetLastError = true)][return: MarshalAs(UnmanagedType.Bool)] public static extern bool FreeLibrary(IntPtr hModule);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate bool InitModuleFunc();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate bool ExitModuleFunc();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate IPluginFactory GetFactoryProc();

        IntPtr _module;

        T GetFunctionPointer<T>(string name) where T : Delegate
        {
            var intPtr = GetProcAddress(_module, name);
            if (intPtr == IntPtr.Zero) return default;
            return Marshal.GetDelegateForFunctionPointer(intPtr, typeof(T)) as T;
        }

        public override void Dispose()
        {
            if (_module != IntPtr.Zero)
            {
                // ExitDll is optional
                var dllExit = GetFunctionPointer<ExitModuleFunc>("ExitDll");
                dllExit?.Invoke();
                FreeLibrary(_module);
            }
        }

        protected override bool Load(string inPath, out string errorDescription)
        {
            var path = _Path.Combine(inPath, "Contents", ArchitectureString, _Path.GetFileName(inPath));
            _module = LoadLibrary(path);
            if (_module == IntPtr.Zero)
            {
                _module = LoadLibrary(inPath);
                if (_module == IntPtr.Zero)
                {
                    var lastError = Marshal.GetLastWin32Error();
                    var msg = new Win32Exception(lastError).Message;
                    errorDescription = $"LoadLibray failed: {msg}";
                    return false;
                }
            }
            var factoryProc = GetFunctionPointer<GetFactoryProc>("GetPluginFactory");
            if (factoryProc == null) { errorDescription = "The dll does not export the required 'GetPluginFactory' function."; return false; }
            errorDescription = null;
            // InitDll is optional
            var dllEntry = GetFunctionPointer<InitModuleFunc>("InitDll");
            if (dllEntry != null)
            {
                try
                {
                    if (!dllEntry()) { errorDescription = "Calling 'InitDll' failed."; return false; }
                }
                catch (SEHException e) { errorDescription = $"Calling 'InitDll' failed. {e.Message}"; }
            }
            var f = factoryProc();
            if (f == null) { errorDescription = "Calling 'GetPluginFactory' returned null."; return false; }
            _factory = f;
            errorDescription ??= default;
            return true;
        }

        static bool CheckVST3Package(string p, out string result)
        {
            var path = _Path.Combine(p, "Contents", ArchitectureString, _Path.GetFileName(p));
            if (File.Exists(path))
            {
                var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (file != null) { file.Close(); result = path; return true; }
            }
            result = default;
            return false;
        }

        static bool IsFolderSymbolicLink(string p) => false;

        static string GetKnownFolder(Environment.SpecialFolder folder) => Environment.GetFolderPath(folder);

        static string ResolveShellLink(string p) => null;

        static void FindFilesWithExt(string path, string ext, List<string> paths, bool recursive = true)
        {
            foreach (var p in Directory.GetFileSystemEntries(path))
            {
                var cpExt = _Path.GetExtension(p);
                if (cpExt == ext)
                {
                    if (Directory.Exists(p) || IsFolderSymbolicLink(p))
                    {
                        if (CheckVST3Package(p, out var finalPath)) { paths.Add(finalPath); continue; }
                        FindFilesWithExt(p, ext, paths, recursive);
                    }
                    else paths.Add(p);
                }
                else if (recursive)
                {
                    if (Directory.Exists(p)) FindFilesWithExt(p, ext, paths, recursive);
                    else if (cpExt == ".lnk")
                    {
                        var resolvedLink = ResolveShellLink(p);
                        if (resolvedLink == null) continue;
                        else if (_Path.GetExtension(resolvedLink) == ext)
                        {
                            if (Directory.Exists(resolvedLink) || IsFolderSymbolicLink(resolvedLink))
                            {
                                if (CheckVST3Package(p, out var finalPath)) { paths.Add(finalPath); continue; }
                                FindFilesWithExt(resolvedLink, ext, paths, recursive);
                            }
                            else paths.Add(resolvedLink);
                        }
                        else if (Directory.Exists(resolvedLink))
                        {
                            if (p != resolvedLink) FindFilesWithExt(resolvedLink, ext, paths, recursive);
                        }
                    }
                }
            }
        }

        static void FindModules(string path, List<string> pathList)
        {
            if (Directory.Exists(path)) FindFilesWithExt(path, ".vst3", pathList);
        }

        static string GetContentsDirectoryFromModuleExecutablePath(string modulePath)
        {
            var path = _Path.GetDirectoryName(modulePath);
            if (_Path.GetFileName(path) != ArchitectureString) return null;
            path = _Path.GetDirectoryName(path);
            if (_Path.GetFileName(path) != "Contents") return null;
            return path;
        }

        internal new static Module Create(string path, out string errorDescription)
        {
            var module = new ModuleWin32();
            if (module.Load(path, out errorDescription))
            {
                module.Path = path;
                module.Name = _Path.GetFileName(path);
                module.Factory = new PluginFactory(module._factory);
                return module;
            }
            return null;
        }

        internal new static List<string> GetModulePaths()
        {
            string knownFolder;
            // find plug-ins located in common/VST3
            var list = new List<string>();
            if ((knownFolder = GetKnownFolder(Environment.SpecialFolder.CommonProgramFiles)) != null) FindModules(_Path.Combine(knownFolder, "VST3"), list);
            //if ((knownFolder = GetKnownFolder(Environment.SpecialFolder.CommonProgramFiles)) != null) FindModules(_Path.Combine(knownFolder, "VST3"), list);

            // find plug-ins located in VST3 (application folder)
            var path = _Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FindModules(_Path.Combine(path, "VST3"), list);
            return list;
        }

        internal new static string GetModuleInfoPath(string modulePath)
        {
            var path = GetContentsDirectoryFromModuleExecutablePath(modulePath);
            if (path == null)
            {
                if (!CheckVST3Package(modulePath, out var p)) return null;
                path = _Path.GetDirectoryName(_Path.GetDirectoryName(p));
            }
            path = _Path.Combine(path, "moduleinfo.json");
            return File.Exists(path) ? path : null;
        }

        internal new static List<Snapshot> GetSnapshots(string modulePath)
        {
            var result = new List<Snapshot>();
            var path = GetContentsDirectoryFromModuleExecutablePath(modulePath);
            if (path == null)
            {
                if (!CheckVST3Package(modulePath, out var p)) return result;
                path = _Path.GetDirectoryName(_Path.GetDirectoryName(p));
            }
            path = _Path.Combine(path, "Resources", "Snapshots");
            if (!File.Exists(path)) return result;

            var pngList = new List<string>();
            FindFilesWithExt(path, ".png", pngList, false);
            foreach (var png in pngList)
            {
                var filename = _Path.GetFileName(png);
                var uid = Snapshot.DecodeUID(filename);
                if (uid == null) continue;
                var decodedScaleFactor = Snapshot.DecodeScaleFactor(filename);
                var scaleFactor = decodedScaleFactor != null ? decodedScaleFactor.Value : 1D;
                var desc = new Snapshot.ImageDesc { ScaleFactor = scaleFactor, Path = png };
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