using Steinberg.Vst3;
using Steinberg.Vst3.Hosting;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
{
    public class InvalidStateTransitionTest : TestEnh
    {
        public override string Name => "Invalid State Transition";

        public InvalidStateTransitionTest(ITestPlugProvider plugProvider) : base(plugProvider, SymbolicSampleSizes.Sample32) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            // created
            var result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result == kResultFalse) return false;

            // setupProcessing is missing !
            //result = audioEffect.SetupProcessing(processSetup);
            //if (result != S_True) return false;

            // initialized
            result = vstPlug.SetActive(false);
            if (result == kResultOk) return false;

            result = vstPlug.SetActive(true);
            if (result == kResultFalse) return false;

            // allocated
            result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result == kResultOk) return false;

            result = vstPlug.SetActive(false);
            if (result == kResultFalse) return false;

            // deallocated (initialized)
            result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result == kResultOk) return false;

            result = vstPlug.Terminate();
            if (result == kResultFalse) return false;

            // terminated (created)
            result = vstPlug.SetActive(false);
            if (result == kResultOk) return false;

            result = vstPlug.Terminate();
            if (result == kResultOk) return false;

            return true;
        }
    }
}
