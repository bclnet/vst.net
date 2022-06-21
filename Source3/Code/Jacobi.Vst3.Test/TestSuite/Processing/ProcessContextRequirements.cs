using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Silence Flags.
    /// </summary>
    public class ProcessContextRequirementsTest : TestEnh
    {
        public override string Name => "ProcessContext Requirements";

        public ProcessContextRequirementsTest(ITestPlugProvider plugProvider) : base(plugProvider, SymbolicSampleSizes.Sample32) { }

        public override bool Setup() => base.Setup();

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            return true;
        }
    }
}
