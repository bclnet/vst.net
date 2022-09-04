using Steinberg.Vst3;
using System;
using System.IO;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
{
    public class BypassPersistenceTest : AutomationTest
    {
        public override string Name => "Parameter Bypass persistence";

        public BypassPersistenceTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl)
            : base(plugProvider, sampl, 100, 1, false) { }

        public override bool Run(ITestResult testResult)
        {
            if (vstPlug == null || testResult == null || audioEffect == null) return false;
            if (!CanProcessSampleSize(testResult)) return true;

            PrintTestHeader(testResult);

            if (bypassId == ParameterInfo.NoParamId)
            {
                testResult.AddMessage("This plugin does not have a bypass parameter!!!");
                return true;
            }
            UnprepareProcessing();

            processData._.NumSamples = 0;
            processData._.NumInputs = 0;
            processData._.NumOutputs = 0;
            processData._.Inputs = IntPtr.Zero;
            processData._.Outputs = IntPtr.Zero;

            audioEffect.SetProcessing(true);

            PreProcess(testResult);

            // set bypass on
            //if (paramChanges[0].GetParameterId() == bypassId)
            {
                paramChanges[0].Init(bypassId, 1);
                paramChanges[0].SetPoint(0, 0, 1);

                controller.SetParamNormalized(bypassId, 1);
                if (controller.GetParamNormalized(bypassId) < 1) testResult.AddErrorMessage("The bypass parameter was not correctly set!");
            }

            // flush
            var result = audioEffect.Process(ref processData._);
            if (result != kResultOk)
            {
                testResult.AddErrorMessage("The component failed to process without audio buffers!");
                audioEffect.SetProcessing(false);
                return false;
            }

            PostProcess(testResult);

            audioEffect.SetProcessing(false);

            // save State
            plugProvider.GetComponentUID(out var uid);

            MemoryBStream stream = new();
            PresetFile.SavePreset(stream, uid, vstPlug, controller, null, 0);

            audioEffect.SetProcessing(true);

            PreProcess(testResult);

            // set bypass off
            if (paramChanges[0].GetParameterId() == bypassId)
            {
                paramChanges[0].Init(bypassId, 1);
                paramChanges[0].SetPoint(0, 0, 0);

                controller.SetParamNormalized(bypassId, 0);
                if (controller.GetParamNormalized(bypassId) > 0) testResult.AddErrorMessage("The bypass parameter was not correctly set in the controller!");
            }

            // flush
            result = audioEffect.Process(ref processData._);
            if (result != kResultOk)
            {
                testResult.AddErrorMessage("The component failed to process without audio buffers!");
                audioEffect.SetProcessing(false);
                return false;
            }

            PostProcess(testResult);

            audioEffect.SetProcessing(false);

            // load previous preset
            stream.Seek(0, SeekOrigin.Begin, out var z);
            PresetFile.LoadPreset(stream, uid, vstPlug, controller);

            if (controller.GetParamNormalized(bypassId) < 1)
            {
                testResult.AddErrorMessage("The bypass parameter is not in sync in the controller!");
                return false;
            }

            return true;
        }
    }
}
