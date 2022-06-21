using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Input Overwriting
    /// </summary>
    public class ProcessInputOverwritingTest : ProcessTest
    {
        bool noNeedtoProcess;

        public override string Name => "Process Input Overwriting";

        public ProcessInputOverwritingTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            return true;
        }
    }
}
