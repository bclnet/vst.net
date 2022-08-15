using Jacobi.Vst3.Core;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Process Format.
    /// </summary>
    public class ProcessFormatTest : ProcessTest
    {
        public override string Name => "Process Format";

        public ProcessFormatTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (vstPlug == null || testResult == null || audioEffect == null) return false;

            if (!CanProcessSampleSize(testResult)) return true;

            PrintTestHeader(testResult);

            var numFails = 0;
            var numRates = 12;
            var sampleRateFormats = new double[]{22050.0,    32000.0,    44100.0,    48000.0,
                                              88200.0,    96000.0,    192000.0,   384000.0,
                                              1234.5678, 12345.678, 123456.78, 1234567.8};

            var result = vstPlug.SetActive(false);
            if (result != kResultOk) { testResult.AddErrorMessage("IComponent::setActive (false) failed."); return false; }

            testResult.AddMessage("***Tested Sample Rates***");

            for (var i = 0; i < numRates; ++i)
            {
                processSetup.SampleRate = sampleRateFormats[i];
                result = audioEffect.SetupProcessing(processSetup);
                if (result == kResultOk)
                {
                    result = vstPlug.SetActive(true);
                    if (result != kResultOk) { testResult.AddErrorMessage("IComponent::setActive (true) failed."); return false; }

                    audioEffect.SetProcessing(true);
                    result = audioEffect.Process(ref processData._);
                    audioEffect.SetProcessing(false);

                    if (result == kResultOk) testResult.AddMessage($" {sampleRateFormats[i],10}10G Hz - processed successfully!");
                    else { numFails++; testResult.AddErrorMessage($" {sampleRateFormats[i]}10G Hz - failed to process!"); }

                    result = vstPlug.SetActive(false);
                    if (result != kResultOk) { testResult.AddErrorMessage($"IComponent::setActive (false) failed."); return false; }
                }
                else if (sampleRateFormats[i] > 0.0) testResult.AddErrorMessage($"IAudioProcessor::setupProcessing (..) failed for samplerate {sampleRateFormats[i],3} Hz! ");
            }

            result = vstPlug.SetActive(true);
            if (result != kResultOk) return false;

            return true;
        }
    }
}
