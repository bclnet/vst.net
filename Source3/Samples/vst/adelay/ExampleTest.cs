using static Jacobi.Vst3.Utility.Testing;

namespace Jacobi.Vst3
{
    public static class ExampleTest
    {
        public static void Touch() { var _ = InitTests; }

        static ModuleInitializer InitTests = new(() =>
        {
            RegisterTest("ExampleTest", null, (object context, ITestResult testResult) =>
            {
                var plugProvider = context as ITestPlugProvider;
                if (plugProvider != null)
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
