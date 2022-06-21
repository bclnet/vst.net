using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Terminate/Initialize.
    /// </summary>
    public class TerminateInitializeTest : TestBase
    {
        public override string Name => "Terminate/Initialize";

        public TerminateInitializeTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var result = true;
            if (vstPlug.Terminate() != TResult.S_True)
            {
                testResult.AddErrorMessage("IPluginBase::terminate () failed.");
                result = false;
            }
            if (vstPlug.Initialize(TestingPluginContext.Get()) != TResult.S_True)
            {
                testResult.AddErrorMessage("IPluginBase::initialize (..) failed.");
                result = false;
            }
            return result;
        }
    }
}
