using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.Test.State
{
    public class RepeatIdenticalStateTransitionTest : TestEnh
    {
        public override string Name => "Repeat Identical State Transition";

        public RepeatIdenticalStateTransitionTest(ITestPlugProvider plugProvider) : base(plugProvider, SymbolicSampleSizes.Sample32) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null || audioEffect == null) return false;

            PrintTestHeader(testResult);

            var result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result != TResult.S_False) return false;

            result = audioEffect.SetupProcessing(processSetup);
            if (result != TResult.S_True) return false;

            result = vstPlug.SetActive(true);
            if (result != TResult.S_OK) return false;

            result = vstPlug.SetActive(true);
            if (result != TResult.S_False) return false;

            result = vstPlug.SetActive(false);
            if (result != TResult.S_OK) return false;

            result = vstPlug.SetActive(false);
            if (result == TResult.S_OK) return false;

            result = vstPlug.Terminate();
            if (result != TResult.S_OK) return false;

            result = vstPlug.Terminate();
            if (result == TResult.S_OK) return false;

            result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result != TResult.S_OK) return false;

            return true;
        }
    }
}
