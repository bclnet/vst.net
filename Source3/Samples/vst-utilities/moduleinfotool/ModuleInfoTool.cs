using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.ModuleInfo;
using Jacobi.Vst3.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Steinberg.Vst
{
    internal class ModuleInfoTool
    {
        string[] args;

        public ModuleInfoTool(string[] args)
        {
            this.args = args;
        }

        //-- Options
        public static readonly Command optCreate = new("create", "Create moduleinfo"); // -create -version VERSION -path MODULE_PATH [-compat PATH -output PATH]
        public static readonly Command optValidate = new("validate", "Validate moduleinfo"); // -validate -path MODULE_PATH [-infopath PATH]
        public static readonly Option<string> optModuleVersion = new("version", "Module version");
        public static readonly Option<string> optModulePath = new("path", "Path to module");
        public static readonly Option<string> optInfoPath = new("infopath", "Path to moduleinfo.json");
        public static readonly Option<string> optModuleCompatPath = new("compat", "Path to compatibility.json");
        public static readonly Option<string> optOutputPath = new("output", "Write json to file instead of stdout");

        static void PrintUsage()
        {
            Console.Write("Usage:\n");
            Console.Write("  moduleinfotool -create -version VERSION -path MODULE_PATH [-compat PATH -output PATH]\n");
            Console.Write("  moduleinfotool -validate -path MODULE_PATH [-infopath PATH]\n");
        }

        static string LoadFile(string path)
        {
            return File.ReadAllText(path);
        }

        static List<ModuleInfo.Compatibility_> OpenAndParseCompatJSON(string path)
        {
            var data = LoadFile(path);
            if (string.IsNullOrEmpty(data))
            {
                Console.Write($"Can not read '{path}'\n");
                PrintUsage();
                return null;
            }
            var result = ModuleInfoLib.ParseCompatibilityJson(data, out var error);
            if (result == null)
            {
                Console.Write($"Can not parse '{path}'\n");
                Console.Write(error);
                PrintUsage();
                return null;
            }
            return result;
        }

        static int CreateJSON(List<ModuleInfo.Compatibility_> compat, string modulePath, string moduleVersion, TextWriter outStream)
        {
            var module = Module.Create(modulePath, out var errorStr);
            if (module == null)
            {
                Console.Write(errorStr);
                return 1;
            }
            var moduleInfo = ModuleInfoLib.CreateModuleInfo(module, false);
            if (compat != null) moduleInfo.Compatibility = compat;
            moduleInfo.Version = moduleVersion;

            var output = new MemoryStream();
            ModuleInfoLib.OutputJson(moduleInfo, output);
            var str = output.ToString();
            outStream.Write(str);
            return 0;
        }

        class ValidateError : Exception
        {
            public ValidateError(string str) : base(str) { }
        }

        static void Validate(ModuleInfo moduleInfo, Module module)
        {
            var factory = module.Factory;
            var factoryInfo = factory.Info;
            var classInfoList = factory.ClassInfos;
            var snapshotList = Module.GetSnapshots(module.Path);

            if (factoryInfo.Vendor != moduleInfo.FactoryInfo.Vendor) throw new ValidateError($"factoryInfo.vendor different: {moduleInfo.FactoryInfo.Vendor}");
            if (factoryInfo.Url != moduleInfo.FactoryInfo.Url) throw new ValidateError($"factoryInfo.url different: {moduleInfo.FactoryInfo.Url}");
            if (factoryInfo.Email != moduleInfo.FactoryInfo.Email) throw new ValidateError($"factoryInfo.email different: {moduleInfo.FactoryInfo.Email}");
            if (factoryInfo.Flags != moduleInfo.FactoryInfo.Flags) throw new ValidateError($"factoryInfo.flags different: {moduleInfo.FactoryInfo.Flags}");

            foreach (var ci in moduleInfo.Classes)
            {
                var cid = Guid.Parse(ci.Cid);
                if (cid == Guid.Empty) throw new ValidateError($"could not parse class UID: {ci.Cid}");
                var it = classInfoList.FirstOrDefault(el => el.ID == cid);
                if (it == null) throw new ValidateError($"cannot find CID in class list: {ci.Cid}");
                if (it.Name != ci.Name) throw new ValidateError($"class name different:  {ci.Name}");
                if (it.Category != ci.Category) throw new ValidateError($"class category different:  {ci.Category}");
                if (it.Vendor != ci.Vendor) throw new ValidateError($"class vendor different:  {ci.Vendor}");
                if (it.Version != ci.Version) throw new ValidateError($"class version different:  {ci.Version}");
                if (it.SdkVersion != ci.SdkVersion) throw new ValidateError($"class sdkVersion different: {ci.SdkVersion}");
                if (it.SubCategories != ci.SubCategories) throw new ValidateError($"class subCategories different: " /* + ci.SubCategories*/);
                if (it.Cardinality != ci.Cardinality) throw new ValidateError($"class cardinality different: {ci.Cardinality}");
                if (it.ClassFlags != (ComponentFlags)ci.Flags) throw new ValidateError($"class flags different: {ci.Flags}");
                classInfoList.Remove(it);

                var snapshotListIt = snapshotList.FirstOrDefault(el => el.Uid == cid);
                if (snapshotListIt == null && ci.Snapshots.Any()) throw new ValidateError($"cannot find snapshots for: {ci.Cid}");
                foreach (var snapshot in ci.Snapshots)
                {
                    var snapshotIt = snapshotListIt.Images.FirstOrDefault(el => el.ScaleFactor == snapshot.ScaleFactor);
                    if (snapshotIt.Path == null) throw new ValidateError($"cannot find snapshots for scale factor: {snapshot.ScaleFactor}");
                    var path = snapshotIt.Path;
                    if (path.Contains(module.Path)) path = path.Remove(module.Path.Length + 1);
                    if (path != snapshot.Path) throw new ValidateError($"cannot find snapshots with path: {snapshot.Path}");
                    snapshotListIt.Images.Remove(snapshotIt);
                }
                if (snapshotListIt != null && snapshotListIt.Images.Any())
                {
                    var errorStr = "Missing Snapshots in moduleinfo:\n";
                    foreach (var s in snapshotListIt.Images) errorStr += s.Path + '\n';
                    throw new ValidateError(errorStr);
                }
                if (snapshotListIt != null) snapshotList.Remove(snapshotListIt);
            }
            if (classInfoList.Any()) throw new ValidateError("Missing classes in moduleinfo");
            if (snapshotList.Any()) throw new ValidateError("Missing snapshots in moduleinfo");
        }

        static int Validate(string modulePath, string infoJsonPath)
        {
            if (string.IsNullOrEmpty(infoJsonPath))
            {
                var path = Module.GetModuleInfoPath(modulePath);
                if (path == null)
                {
                    Console.Error.Write($"Module does not contain a moduleinfo.json: '{modulePath}'\n");
                    return 1;
                }
                infoJsonPath = path;
            }

            var data = LoadFile(infoJsonPath);
            if (string.IsNullOrEmpty(data))
            {
                Console.Error.Write($"Empty or non existing file: '{infoJsonPath}'\n");
                PrintUsage();
                return 1;
            }
            var moduleInfo = ModuleInfoLib.ParseJson(data, out var error); if (error != null) Console.Error.Write(error);
            if (moduleInfo != null)
            {
                var module = Module.Create(modulePath, out var errorStr);
                if (module == null)
                {
                    Console.Error.Write(errorStr);
                    PrintUsage();
                    return 1;
                }
                try
                {
                    Validate(moduleInfo, module);
                }
                catch (Exception exc)
                {
                    Console.Error.Write($"Error:\n{exc.Message}\n");
                    PrintUsage();
                    return 1;
                }
                return 0;
            }
            PrintUsage();
            return 1;
        }

        public async Task<int> Run()
        {
            var returnCode = 0;
            var rootCommand = new RootCommand($"")
            {
                optCreate,
                optValidate,
                optModuleVersion,
                optModulePath,
                optInfoPath,
                optModuleCompatPath,
                optOutputPath
            };
            optCreate.SetHandler((moduleVersion, modulePath, moduleCompatPath, outputPath) =>
            {
                TextWriter infoStream = Console.Out;
                TextWriter errorStream = Console.Out;

                var outputStream = Console.Out;
                List<ModuleInfo.Compatibility_> compat = null;
                if (moduleCompatPath != null)
                {
                    compat = OpenAndParseCompatJSON(moduleCompatPath);
                    if (compat == null) { returnCode = 1; return; }
                }
                var writeToFile = false;
                if (outputPath != null)
                {
                    writeToFile = true;
                    var ostream = new StreamWriter(File.Open(outputPath, FileMode.CreateNew));
                    if (ostream != null) outputStream = ostream;
                    else
                    {
                        Console.Write($"Cannot create output file: {outputPath}\n");
                        returnCode = 1;
                        return;
                    }
                }
                returnCode = CreateJSON(compat, modulePath, moduleVersion, outputStream);
                if (writeToFile) outputStream.Dispose();
            }, optModuleVersion, optModulePath, optModuleCompatPath, optOutputPath);
            optValidate.SetHandler((modulePath, infoPath) =>
            {
                returnCode = Validate(modulePath, infoPath);
            }, optModulePath, optInfoPath);
            await rootCommand.InvokeAsync(args);
            return returnCode;
        }
    }
}