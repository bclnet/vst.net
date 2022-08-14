using Jacobi.Vst3.Core;
using System.Threading;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// ProcessTest
    /// </summary>
    public class ProcessThreadTest : ProcessTest
    {
        public override string Name => "Process function running in another thread";

        public ProcessThreadTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            const int NUM_ITERATIONS = 9999;

            if (vstPlug == null || testResult == null || audioEffect == null) return false;
            if (!CanProcessSampleSize(testResult)) return true;

            PrintTestHeader(testResult);

            var result = false;

            var processThread = new Thread(() =>
            {
                result = true;
                audioEffect.SetProcessing(true);
                for (var i = 0; i < NUM_ITERATIONS; i++)
                {
                    var tr = audioEffect.Process(processData._);
                    if (tr != TResult.S_True) { result = false; break; }
                }
                audioEffect.SetProcessing(false);
            });
            processThread.Start();
            processThread.Join();

            if (!result) testResult.AddErrorMessage("Processing failed.");
            return result;
        }
    }
}
