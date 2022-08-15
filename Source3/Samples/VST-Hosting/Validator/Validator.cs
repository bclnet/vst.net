using Jacobi.Vst3.Core;
using Jacobi.Vst3.Hosting;
using Jacobi.Vst3.TestSuite;
using Jacobi.Vst3.Utility;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Jacobi.Vst3.Core.SpeakerArrangement;
using static Jacobi.Vst3.Core.TResult;

namespace Steinberg.Vst
{
    internal class Validator : ITestResult, IHostApplication
    {
        struct ModuleTestConfig
        {
            public ModuleTestConfig(bool useGlobalInstance, bool useExtensiveTests, string customTestComponentPath, string testSuiteName, Guid? testProcessor)
            {
                this.useGlobalInstance = useGlobalInstance;
                this.useExtensiveTests = useExtensiveTests;
                this.customTestComponentPath = customTestComponentPath;
                this.testSuiteName = testSuiteName;
                this.testProcessor = testProcessor;
            }

            public bool useGlobalInstance;
            public bool useExtensiveTests;
            public string customTestComponentPath;
            public string testSuiteName;
            public Guid? testProcessor;
        }

        const string SEPARATOR = "-------------------------------------------------------------\n";
        public static readonly string VALIDATOR_INFO = $"{Constants.Vst3SdkVersion} Plug-in Validator\nProgram by Steinberg (Built on {DateTime.Today})\n";

        static bool FilterClassCategory(string category, string classCategory)
            => category == classCategory;

        static void PrintAllInstalledPlugins(TextWriter os)
        {
            if (os == null) return;

            os.Write("Searching installed Plug-ins...\n");
            os.Flush();

            var paths = Module.GetModulePaths();
            if (!paths.Any()) { os.WriteLine("No Plug-ins found.\n"); return; }
            foreach (var path in paths) os.Write($"{path}\n");
        }

        static void PrintAllSnapshots(TextWriter os)
        {
            if (os == null) return;
            os.Write("Searching installed Plug-ins...\n");
            var paths = Module.GetModulePaths();
            if (!paths.Any()) { os.Write("No Plug-ins found.\n"); return; }
            foreach (var path in paths)
            {
                var snapshots = Module.GetSnapshots(path);
                if (!snapshots.Any()) { os.Write($"No snapshots in {path}\n"); continue; }
                foreach (var snapshot in snapshots)
                    foreach (var desc in snapshot.Images)
                        os.Write($"Snapshot : {desc.Path}[{desc.ScaleFactor}x]\n");
            }
        }

        static void PrintFactoryInfo(PluginFactory factory, TextWriter os)
        {
            if (os != null)
            {
                os.Write("* Scanning classes...\n\n");

                var factoryInfo = factory.Info;

                os.Write($"  Factory Info:\n\tvendor = {factoryInfo.Vendor}"
                    + $"\n\turl = {factoryInfo.Url}\n\temail = {factoryInfo.Email}"
                    + $"\n\n");

                //---print all included plug-ins---------------
                var i = 0U;
                foreach (var classInfo in factory.ClassInfos)
                {
                    os.Write($"  Class Info {i}:\n\tname = {classInfo.Name}"
                        + $"\n\tcategory = {classInfo.Category}"
                        + $"\n\tsubCategories = {classInfo.SubCategoriesString()}"
                        + $"\n\tversion = {classInfo.Version}"
                        + $"\n\tsdkVersion = {classInfo.SdkVersion}"
                        + $"\n\tcid = {classInfo.ID.ToString().Replace("-", "")}\n\n");
                    ++i;
                }
            }
        }

        static void CheckModuleSnapshots(Module module, TextWriter infoStream)
        {
            infoStream?.Write("* Checking snapshots...\n\n");

            var snapshots = Module.GetSnapshots(module.Path);
            if (!snapshots.Any()) infoStream?.Write("Info: No snapshots in Bundle.\n\n");
            else
                foreach (var classInfo in module.Factory.ClassInfos)
                {
                    if (!FilterClassCategory(Constants.kVstAudioEffectClass, classInfo.Category)) continue;
                    var found = false;
                    foreach (var snapshot in snapshots)
                        if (snapshot.Uid == classInfo.ID)
                        {
                            found = true;
                            if (infoStream != null)
                            {
                                infoStream.Write($"Found snapshots for '{classInfo.Name}'\n");
                                foreach (var image in snapshot.Images)
                                    infoStream.Write($" - {image.Path} [{image.ScaleFactor}x]\n");
                                infoStream.Write("\n");
                            }
                            break;
                        }
                    if (!found) infoStream?.Write($"Info: No snapshot for '{classInfo.Name}' in Bundle.\n\n");
                }
        }

        string[] args;
        PlugInterfaceSupport _plugInterfaceSupport = new();
        int numTestsFailed;
        int numTestsPassed;
        bool addErrorWarningTextToOutput = true;

        TextWriter infoStream = Console.Out;
        TextWriter errorStream = Console.Out;

        public Validator(string[] args)
        {
            this.args = args;
            PluginContextFactory.Instance.SetPluginContext(this);
            TestingPluginContext.Set(this);
        }

        public void AddErrorMessage(string msg)
        {
            if (errorStream != null)
            {
                var str = msg;
                if (addErrorWarningTextToOutput) errorStream.Write($"ERROR: {str}\n");
                else errorStream.Write($"{str}\n");
            }
        }

        public void AddMessage(string msg)
        {
            if (infoStream != null)
            {
                var str = msg;
                if (addErrorWarningTextToOutput) infoStream.Write($"Info:  {str}\n");
                else infoStream.Write($"{str}\n");
            }
        }

        public TResult GetName(StringBuilder name)
        {
            name.Append("vstvalidator");
            return kResultTrue;
        }

        HostMessage _hostMessage;
        HostAttributeList _hostAttributeList;
        public TResult CreateInstance(ref Guid cid, ref Guid iid, out IntPtr obj)
        {
            Type imessage = typeof(IMessage), iattributeList = typeof(IAttributeList);

            var classID = cid;
            var interfaceID = iid;
            obj = default;
            if (classID == imessage.GUID && interfaceID == imessage.GUID)
            {
                var objx = _hostMessage = new HostMessage();
                obj = Marshal.GetComInterfaceForObject(objx, imessage);
                return kResultTrue;
            }
            else if (classID == iattributeList.GUID && interfaceID == iattributeList.GUID)
            {
                var al = _hostAttributeList = new HostAttributeList();
                if (al != null)
                {
                    obj = Marshal.GetComInterfaceForObject(al, iattributeList);
                    return kResultTrue;
                }
                return kOutOfMemory;
            }
            obj = default;
            return kResultFalse;
        }

        //-- Options
        public static readonly Command optVersion = new("version", "Print version");
        public static readonly Option<bool> optLocalInstance = new("l", "Use local instance per test");
        public static readonly Option<string> optSuiteName = new("suite", "[name] Only run a special test suite");
        public static readonly Option<bool> optExtensiveTests = new("e", "Run extensive tests [may take a long time]");
        public static readonly Option<bool> optQuiet = new("q", "Only print errors");
        public static readonly Option<string> optTestComponentPath = new("test-component", "[path] Path to an additional component which includes custom tests");
        public static readonly Command optListInstalledPlugIns = new("list", "Show all installed Plug-Ins");
        public static readonly Command optListPlugInSnapshots = new("snapshots", "List snapshots from all installed Plug-Ins");
        public static readonly Option<string> optCID = new("cid", "Only test processor with specified class ID");
        public static readonly Command optSelftest = new("selftest", "Run a selftest");
        public static readonly Option<string[]> optFiles = new("f", "Files") { IsRequired = true, AllowMultipleArgumentsPerToken = true };

        public async Task<int> Run()
        {
            var returnCode = 0;
            var rootCommand = new RootCommand($"{Constants.Vst3SdkVersion} Plug-in Validator")
            {
                optVersion,
                optLocalInstance,
                optSuiteName,
                optExtensiveTests,
                optQuiet,
                optCID,
                optTestComponentPath,
                optListInstalledPlugIns,
                optSelftest,
                optListPlugInSnapshots,
                optFiles
            };
            optVersion.SetHandler(() =>
            {
                Console.Write(VALIDATOR_INFO);
                returnCode = 0;
            });
            optListInstalledPlugIns.SetHandler(() =>
            {
                PrintAllInstalledPlugins(infoStream);
                returnCode = 0;
            });
            optListPlugInSnapshots.SetHandler(() =>
            {
                PrintAllSnapshots(infoStream);
                returnCode = 0;
            });
            optSelftest.SetHandler(() =>
            {
                addErrorWarningTextToOutput = false;
                var testFactoryInstance = (ITestFactory)Testing.CreateTestFactoryInstance(null);
                var testFactory = testFactoryInstance;
                if (testFactory != null)
                {
                    Console.Write("Running validator selftest:\n\n");
                    var testSuite = new TestSuite(string.Empty);
                    if (testFactory.CreateTests(null, testSuite) == kResultTrue) RunTestSuite(testSuite, null);
                    Console.Write($"Executed {numTestsFailed + numTestsPassed} Tests.\n");
                    Console.Write($"{numTestsFailed} failed test(s).\n");
                    returnCode = numTestsFailed == 0 ? 0 : 1;
                    return;
                }
                returnCode = 1;
            });
            rootCommand.SetHandler((localInstance, suiteName, extensiveTests, quiet, testComponentPath, cid, files) =>
            {
                var useGlobalInstance = !localInstance;
                var useExtensiveTests = extensiveTests;
                if (quiet) infoStream = null;
                var testSuiteName = suiteName;
                var testProcessor = cid != null ? (Guid?)Guid.Parse(cid) : null;
                var customTestComponentPath = testComponentPath;

                var globalFailure = false;
                foreach (var p in files)
                {
                    var path = p[0] != '/' ? Path.GetFullPath(p) : p; // if path is not absolute, create one
                    if (customTestComponentPath?.Any() == true && customTestComponentPath[0] != '/') customTestComponentPath = Path.GetFullPath(customTestComponentPath);

                    //---load VST module-----------------
                    infoStream?.Write($"* Loading module...\n\n\t{path}\n\n");

                    var module = Module.Create(path, out var error);
                    if (module == null)
                    {
                        errorStream.Write("Invalid Module!\n");
                        if (!string.IsNullOrEmpty(error)) errorStream.Write($"{error}\n");
                        returnCode = -1;
                        return;
                    }

                    TestModule(module, new ModuleTestConfig(useGlobalInstance, useExtensiveTests, customTestComponentPath, testSuiteName, testProcessor));

                    if (numTestsFailed > 0) globalFailure = true;
                }
                returnCode = globalFailure ? -1 : 0;
            }, optLocalInstance, optSuiteName, optExtensiveTests, optQuiet, optTestComponentPath, optCID, optFiles);
            await rootCommand.InvokeAsync(args);
            return returnCode;
        }

        void TestModule(Module module, ModuleTestConfig config)
        {
            numTestsFailed = numTestsPassed = 0;

            var factory = module.Factory;
            PrintFactoryInfo(module.Factory, infoStream);

            //---check for snapshots-----------------
            CheckModuleSnapshots(module, infoStream);

            Module testModule;
            List<PlugProvider> plugProviders = new();
            Dictionary<string, ITestFactory> testFactories = new();
            IPluginCompatibility plugCompatibility = null;
            var testSuite = new TestSuite("Tests");

            //---create tests---------------
            infoStream?.Write("* Creating tests...\n\n");
            foreach (var classInfo in factory.ClassInfos)
                if (FilterClassCategory(Constants.kVstAudioEffectClass, classInfo.Category))
                {
                    if (config.testProcessor == null || config.testProcessor == classInfo.ID)
                    {
                        var plugProvider = new PlugProvider(factory, classInfo, config.useGlobalInstance);
                        if (plugProvider != null)
                        {
                            var tests = CreateTests(plugProvider, classInfo.Name, config.useExtensiveTests);
                            testSuite.AddTestSuite(classInfo.Name, tests);
                            plugProviders.Add(plugProvider);
                        }
                    }
                }
                else if (FilterClassCategory(Constants.kTestClass, classInfo.Category))
                {   // gather test factories supplied by the plug-in
                    var testFactory = factory.CreateInstance<ITestFactory>(classInfo.ID);
                    if (testFactory != null) testFactories.Add(classInfo.Name, testFactory);
                }
                else if (FilterClassCategory(Constants.kPluginCompatibilityClass, classInfo.Category))
                {
                    if (plugCompatibility != null) { errorStream?.Write("Error: Factory contains multiple Plugin Compatibility classes.\n"); ++numTestsFailed; }
                    plugCompatibility = factory.CreateInstance<IPluginCompatibility>(classInfo.ID);
                    if (plugCompatibility == null) errorStream?.Write("Error: Failed creating IPluginCompatibility instance.\n");
                }

            // now check testModule if supplied
            if (!string.IsNullOrEmpty(config.customTestComponentPath))
            {
                testModule = Module.Create(config.customTestComponentPath, out var error);
                if (testModule != null)
                {
                    var _factory = testModule.Factory;
                    foreach (var classInfo in _factory.ClassInfos)
                        if (FilterClassCategory(Constants.kTestClass, classInfo.Category))
                        { // gather test factories supplied by the plug-in
                            var testFactory = _factory.CreateInstance<ITestFactory>(classInfo.ID);
                            if (testFactory != null) testFactories.Add(classInfo.Name, testFactory);
                        }
                }
                else errorStream?.Write($"Could not create custom test component [{config.customTestComponentPath}]\n");
            }
            if (testFactories.Count != 0) infoStream?.Write("* Creating Plug-in supplied tests...\n\n");
            // create plug-in supplied tests
            foreach (var item in testFactories)
                foreach (var plugProvider in plugProviders)
                {
                    var plugTestSuite = new TestSuite(item.Key);
                    if (item.Value.CreateTests(plugProvider, plugTestSuite) == kResultTrue)
                        testSuite.AddTestSuite(plugTestSuite.Name, plugTestSuite);
                }
            testFactories.Clear();

            //---run tests---------------------------
            infoStream?.Write("* Running tests...\n\n");

            RunTestSuite(testSuite, string.IsNullOrEmpty(config.testSuiteName) ? null : config.testSuiteName);

            if (plugCompatibility != null)
                if (!PlugCompat.CheckPluginCompatibility(module, plugCompatibility, out var error))
                {
                    errorStream?.WriteLine(error);
                    ++numTestsFailed;
                }

            if (infoStream != null)
            {
                infoStream.Write(SEPARATOR);
                infoStream.Write($"Result: {numTestsPassed} tests passed, {numTestsFailed} tests failed\n");
                infoStream.Write(SEPARATOR);
            }
        }

        static void CreateTest<T>(ITestSuite parent, ITestPlugProvider plugProvider, params object[] arguments) where T : TestBase
        {
            var test = (T)Activator.CreateInstance(typeof(T), (new object[] { plugProvider }).Concat(arguments).ToArray());
            parent.AddTest(test.Name, test);
        }

        static void CreateSpeakerArrangementTest(ITestSuite parent, ITestPlugProvider plugProvider, SymbolicSampleSizes sampleSize, SpeakerArrangement inSpArr, SpeakerArrangement outSpArr)
            => CreateTest<SpeakerArrangementTest>(parent, plugProvider, sampleSize, inSpArr, outSpArr);

        static void CreatePrecisionTests(ITestSuite parent, ITestPlugProvider plugProvider, SymbolicSampleSizes sampleSize, bool extensive)
        {
            CreateTest<ProcessTest>(parent, plugProvider, sampleSize);
            CreateTest<ProcessThreadTest>(parent, plugProvider, sampleSize);
            CreateTest<SilenceFlagsTest>(parent, plugProvider, sampleSize);
            CreateTest<SilenceProcessingTest>(parent, plugProvider, sampleSize);
            CreateTest<FlushParamTest>(parent, plugProvider, sampleSize);
            CreateTest<FlushParamTest2>(parent, plugProvider, sampleSize);
            CreateTest<FlushParamTest3>(parent, plugProvider, sampleSize);
            CreateTest<VariableBlockSizeTest>(parent, plugProvider, sampleSize);
            CreateTest<ProcessFormatTest>(parent, plugProvider, sampleSize);
            CreateTest<BypassPersistenceTest>(parent, plugProvider, sampleSize);

            if (extensive)
            {
                var saArray = new SpeakerArrangement[]
                {
                    kMono, kStereo, kStereoSurround,
                    kStereoCenter, kStereoSide, kStereoCLfe,
                    k30Cine, k30Music, k31Cine, k31Music,
                    k40Cine, k40Music, k41Cine, k41Music,
                    k50
                };
                foreach (var inArr in saArray)
                    foreach (var outArr in saArray)
                        CreateSpeakerArrangementTest(parent, plugProvider, sampleSize, inArr, outArr);

                var autoRates = new int[] { 100, 50, 1 };
                var numParams = new int[] { 1, 2, 10, -1 };
                foreach (var inp in numParams)
                    foreach (var iar in autoRates)
                    {
                        CreateTest<AutomationTest>(parent, plugProvider, sampleSize, iar, inp, false);
                        CreateTest<AutomationTest>(parent, plugProvider, sampleSize, iar, inp, true);
                    }
            }
            else
            {
                CreateSpeakerArrangementTest(parent, plugProvider, sampleSize, kStereo, kStereo);
                CreateSpeakerArrangementTest(parent, plugProvider, sampleSize, kMono, kMono);

                CreateTest<AutomationTest>(parent, plugProvider, sampleSize, 100, 1, false);
                CreateTest<AutomationTest>(parent, plugProvider, sampleSize, 100, 1, true);
            }
        }

        TestSuite CreateTests(ITestPlugProvider plugProvider, string plugName, bool extensive)
        {
            var plugTestSuite = new TestSuite(plugName);
            var generalTests = new TestSuite("General Tests");
            // todo: add tests here!
            CreateTest<EditorClassesTest>(generalTests, plugProvider);
            CreateTest<ScanBussesTest>(generalTests, plugProvider);
            CreateTest<ScanParametersTest>(generalTests, plugProvider);
            CreateTest<MidiMappingTest>(generalTests, plugProvider);
            CreateTest<MidiLearnTest>(generalTests, plugProvider);
            CreateTest<UnitInfoTest>(generalTests, plugProvider);
            CreateTest<ProgramInfoTest>(generalTests, plugProvider);
            CreateTest<TerminateInitializeTest>(generalTests, plugProvider);
            CreateTest<UnitStructureTest>(generalTests, plugProvider);
            CreateTest<ValidStateTransitionTest>(generalTests, plugProvider, SymbolicSampleSizes.Sample32);
            CreateTest<ValidStateTransitionTest>(generalTests, plugProvider, SymbolicSampleSizes.Sample64);
            //CreateTest<InvalidStateTransitionTest>(generalTests, plugProvider);
            //CreateTest<RepeatIdenticalStateTransitionTest>(generalTests, plugProvider);

            CreateTest<BusConsistencyTest>(generalTests, plugProvider);
            //CreateTest<BusInvalidIndexTest> (generalTests, plugProvider);
            CreateTest<BusActivationTest>(generalTests, plugProvider);

            CreateTest<CheckAudioBusArrangementTest>(generalTests, plugProvider);
            CreateTest<SideChainArrangementTest>(generalTests, plugProvider);

            CreateTest<SuspendResumeTest>(generalTests, plugProvider, SymbolicSampleSizes.Sample32);

            CreateTest<NoteExpressionTest>(generalTests, plugProvider);
            CreateTest<KeyswitchTest>(generalTests, plugProvider);
            CreateTest<ProcessContextRequirementsTest>(generalTests, plugProvider);

            plugTestSuite.AddTestSuite(generalTests.Name, generalTests);

            var singlePrecisionTests = new TestSuite("Single Precision (32 bit) Tests");
            CreatePrecisionTests(singlePrecisionTests, plugProvider, SymbolicSampleSizes.Sample32, extensive);
            plugTestSuite.AddTestSuite(singlePrecisionTests.Name, singlePrecisionTests);

            var doublePrecisionTests = new TestSuite("Double Precision (64 bit) Tests");
            CreatePrecisionTests(doublePrecisionTests, plugProvider, SymbolicSampleSizes.Sample64, extensive);
            plugTestSuite.AddTestSuite(doublePrecisionTests.Name, doublePrecisionTests);

            return plugTestSuite;
        }

        void AddTest(ITestSuite testSuite, TestBase testItem)
        {
            testSuite.AddTest(testItem.Name, testItem);
            //Marshal.Release(Marshal.GetIUnknownForObject(testItem));
        }

        void RunTestSuite(TestSuite suite, string nameFilter)
        {
            if (nameFilter == null || suite.Name == nameFilter)
            {
                nameFilter = null; // make sure if suiteName is the namefilter that sub suite will run
                // first run all tests in the suite
                for (var i = 0; i < suite.GetTestCount(); i++)
                    if (suite.GetTest(i, out var testItem, out var name) == kResultTrue)
                    {
                        if (infoStream != null)
                        {
                            infoStream.Write($"[{name}");
                            var desc = testItem.GetDescription();
                            if (!string.IsNullOrEmpty(desc)) infoStream.Write($": {desc}");
                            infoStream.Write("]\n");
                        }

                        if (testItem.Setup())
                        {
                            var success = testItem.Run(this);
                            if (success)
                            {
                                infoStream?.Write("[Succeeded]\n");
                                numTestsPassed++;
                            }
                            else
                            {
                                infoStream.Write("[XXXXXXX Failed]\n");
                                if (errorStream != infoStream) errorStream?.Write($"Test [{name}] Failed\n");
                                numTestsFailed++;
                            }
                            if (!testItem.Teardown())
                            {
                                infoStream?.Write("Failed to teardown test!\n");
                                if (errorStream != infoStream) errorStream?.Write($"[{name}] Failed to teardown test!\n");
                            }
                        }
                        else
                        {
                            testItem.Teardown();
                            infoStream?.Write("Failed to setup test!\n");
                            if (errorStream != infoStream) errorStream?.Write($"[{name}] Failed to setup test!\n");
                        }
                        infoStream?.Write("\n");
                    }
            }
            // next run sub suites
            var subTestSuiteIndex = 0;
            while (suite.GetTestSuite(subTestSuiteIndex++, out var subSuite, out _) == kResultTrue)
            {
                var ts = (TestSuite)subSuite;
                if (ts != null)
                {
                    if (infoStream != null)
                    {
                        infoStream.Write(SEPARATOR);
                        infoStream.Write($"TestSuite : {ts.Name}\n");
                        infoStream.Write($"{SEPARATOR}\n");
                    }
                    RunTestSuite(ts, nameFilter);
                }
            }
        }
    }
}