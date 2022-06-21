using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Silence Processing.
    /// </summary>
    public class SilenceProcessingTest : ProcessTest
    {
        public override string Name => "Silence Processing";

        public SilenceProcessingTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            return true;
        }
    }
}
