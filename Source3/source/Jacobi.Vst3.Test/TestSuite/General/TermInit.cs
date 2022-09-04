using Steinberg.Vst3;
using Steinberg.Vst3.Hosting;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
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
            if (vstPlug.Terminate() != kResultTrue)
            {
                testResult.AddErrorMessage("IPluginBase::terminate () failed.");
                result = false;
            }
            if (vstPlug.Initialize(TestingPluginContext.Get()) != kResultTrue)
            {
                testResult.AddErrorMessage("IPluginBase::initialize (..) failed.");
                result = false;
            }
            return result;
        }
    }
}
