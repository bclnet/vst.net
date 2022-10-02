using Steinberg.Vst3;
using static Steinberg.Vst3.Utility.Testing;

namespace Steinberg
{
    public static class ExampleTest
    {
        public static void Touch() { var _ = InitTests; }

        static ModuleInitializer InitTests = new(() =>
        {
            RegisterTest("ExampleTest", null, (object context, ITestResult testResult) =>
            {
                if (context is ITestPlugProvider plugProvider)
                {
                    var controller = plugProvider.GetController();
                    var testController = controller as IDelayTestController;
                    if (controller == null)
                    {
                        testResult.AddErrorMessage("Unknown IEditController");
                        return false;
                    }
                    var result = testController.DoTest();
                    plugProvider.ReleasePlugIn(null, controller);

                    return result;
                }
                return false;
            });
        });
    }
}
