using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Hosting;

namespace Jacobi.Vst3.TestSuite
{
    public class ValidStateTransitionTest : ProcessTest
    {
        protected string name;
        public override string Name => this.name;

        public ValidStateTransitionTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampleSize)
            : base(plugProvider, sampleSize) => name = sampleSize == SymbolicSampleSizes.Sample32
                ? "Valid State Transition 32bits"
                : "Valid State Transition 64bits";

        public override bool Run(ITestResult testResult)
        {
            if (vstPlug == null || testResult == null || audioEffect == null) return false;

            PrintTestHeader(testResult);
            if (!CanProcessSampleSize(testResult)) return true;

            // disable it, it was enabled in setup call
            var result = vstPlug.SetActive(false);
            if (result != TResult.S_True) return false;

            for (var i = 0; i < 3; ++i)
            {
                result = audioEffect.SetupProcessing(processSetup);
                if (result != TResult.S_True) return false;

                result = vstPlug.SetActive(true);
                if (result != TResult.S_True) return false;

                result = vstPlug.SetActive(false);
                if (result != TResult.S_True) return false;

                result = vstPlug.Terminate();
                if (result != TResult.S_True) return false;

                result = vstPlug.Initialize(TestingPluginContext.Get());
                if (result != TResult.S_True) return false;
            }
            return true;
        }
    }
}
