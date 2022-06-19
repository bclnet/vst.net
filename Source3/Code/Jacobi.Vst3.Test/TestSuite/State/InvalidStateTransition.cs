using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.Test.State
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
            if (result == TResult.S_False) return false;

            // setupProcessing is missing !
            //result = audioEffect.SetupProcessing(processSetup);
            //if (result != TResult.S_True) return false;

            // initialized
            result = vstPlug.SetActive(false);
            if (result == TResult.S_OK) return false;

            result = vstPlug.SetActive(true);
            if (result == TResult.S_False) return false;

            // allocated
            result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result == TResult.S_OK) return false;

            result = vstPlug.SetActive(false);
            if (result == TResult.S_False) return false;

            // deallocated (initialized)
            result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result == TResult.S_OK) return false;

            result = vstPlug.Terminate();
            if (result == TResult.S_False) return false;

            // terminated (created)
            result = vstPlug.SetActive(false);
            if (result == TResult.S_OK) return false;

            result = vstPlug.Terminate();
            if (result == TResult.S_OK) return false;

            return true;
        }
    }
}
