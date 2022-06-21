using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

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
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            return true;
        }
    }
}
