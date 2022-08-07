using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Hosting;
using Jacobi.Vst3.Utility;
using System;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Silence Flags.
    /// </summary>
    public class ProcessContextRequirementsTest : TestEnh
    {
        public override string Name => "ProcessContext Requirements";

        public ProcessContextRequirementsTest(ITestPlugProvider plugProvider) : base(plugProvider, SymbolicSampleSizes.Sample32) { }

        public override bool Setup() => base.Setup();

        Version GetPluginSDKVersion(ITestPlugProvider plugProvider, ITestResult testResult)
        {
            var pp2 = plugProvider as ITestPlugProvider2;
            if (pp2 == null)
            {
                testResult.AddErrorMessage("Internal test Error. Expected Interface not there!");
                return null;
            }
            var pluginFactory = new PluginFactory(pp2.GetPluginFactory());
            if (pluginFactory.Get() == null)
            {
                testResult.AddErrorMessage("Internal test Error. Expected PluginFactory not there!");
                return null;
            }
            if (pp2.GetComponentUID(out var fuid) != TResult.S_True)
            {
                testResult.AddErrorMessage("Internal test Error. Could not query the UID of the plug-in!");
                return null;
            }
            var plugClassID = fuid;
            var classInfos = pluginFactory.ClassInfos;
            var it = classInfos.Find(element => element.ID == plugClassID);
            if (it == null)
            {
                testResult.AddErrorMessage("Internal test Error. Could not find the class info of the plug-in!");
                return null;
            }
            return VersionX.Parse(it.SdkVersion);
        }

        public override bool Run(ITestResult testResult)
        {
            if (vstPlug == null || testResult == null || audioEffect == null) return false;

            PrintTestHeader(testResult);

            // check if plug-in is build with any earlier VST SDK which does not support this interface
            var sdkVersion = GetPluginSDKVersion(plugProvider, testResult);
            if (sdkVersion == null) return false;
            if (sdkVersion.Major < 3 || (sdkVersion.Major == 3 && sdkVersion.Minor < 7))
            {
                testResult.AddMessage("No ProcessContextRequirements required. Plug-In built with older SDK.");
                return true;
            }

            var contextRequirements = audioEffect as IProcessContextRequirements;
            if (contextRequirements != null)
            {
                var req = new ProcessContextRequirements(contextRequirements.GetProcessContextRequirements());
                testResult.AddMessage("ProcessContextRequirements:");
                if (req.WantsNone) testResult.AddMessage(" - None");
                else
                {
                    if (req.WantsSystemTime) testResult.AddMessage(" - SystemTime");
                    if (req.WantsContinousTimeSamples) testResult.AddMessage(" - ContinousTimeSamples");
                    if (req.WantsProjectTimeMusic) testResult.AddMessage(" - ProjectTimeMusic");
                    if (req.WantsBarPositionMusic) testResult.AddMessage(" - BarPosititionMusic");
                    if (req.WantsCycleMusic) testResult.AddMessage(" - CycleMusic");
                    if (req.WantsSamplesToNextClock) testResult.AddMessage(" - SamplesToNextClock");
                    if (req.WantsTempo) testResult.AddMessage(" - Tempo");
                    if (req.WantsTimeSignature) testResult.AddMessage(" - TimeSignature");
                    if (req.WantsChord) testResult.AddMessage(" - Chord");
                    if (req.WantsFrameRate) testResult.AddMessage(" - FrameRate");
                    if (req.WantsTransportState) testResult.AddMessage(" - TransportState");
                }
                return true;
            }

            testResult.AddMessage("Since VST SDK 3.7 you need to implement IProcessContextRequirements!");
            testResult.AddErrorMessage("Missing mandatory IProcessContextRequirements extension!");
            return false;
        }
    }
}
