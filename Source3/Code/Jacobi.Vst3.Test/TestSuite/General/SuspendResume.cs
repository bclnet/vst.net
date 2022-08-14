using Jacobi.Vst3.Core;
using Jacobi.Vst3.Hosting;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Suspend/Resume.
    /// </summary>
    public class SuspendResumeTest : TestEnh
    {
        public override string Name => "Suspend/Resume";

        public SuspendResumeTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            for (var i = 0; i < 3; ++i)
            {
                if (audioEffect != null)
                {
                    if (audioEffect.CanProcessSampleSize(SymbolicSampleSizes.Sample32) == TResult.S_OK) processSetup.SymbolicSampleSize = SymbolicSampleSizes.Sample32;
                    else if (audioEffect.CanProcessSampleSize(SymbolicSampleSizes.Sample64) == TResult.S_OK) processSetup.SymbolicSampleSize = SymbolicSampleSizes.Sample64;
                    else
                    {
                        testResult.AddErrorMessage("No appropriate symbolic sample size supported!");
                        return false;
                    }

                    if (audioEffect.SetupProcessing(processSetup) != TResult.S_OK)
                    {
                        testResult.AddErrorMessage("Process setup failed!");
                        return false;
                    }
                }
                var result = vstPlug.SetActive(true);
                if (result != TResult.S_OK) return false;

                result = vstPlug.SetActive(false);
                if (result != TResult.S_OK) return false;
            }
            return true;
        }
    }
}
