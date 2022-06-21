using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test ProcesTail.
    /// </summary>
    public class ProcessTailTest : ProcessTest
    {
        uint mTailSamples;
        uint mInTail;

        float[] dataPtrFloat;
        double[] dataPtrDouble;
        bool mInSilenceInput;
        bool mDontTest;

        public override string Name => "Check Tail processing";

        public ProcessTailTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            return true;
        }
    }
}
