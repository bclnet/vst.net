using static Jacobi.Vst3.Utility.Testing;

namespace Jacobi.Vst3.Hosting.Test
{
    public static class PlugInterfacesSupportTest
    {
        public static void Touch() { var _ = InitTests; }

        static ModuleInitializer InitTests = new(() =>
        {
            const string TestSuiteName = "EventList";
            RegisterTest(TestSuiteName, "", (ITestResult testResult) =>
            {
                return true;
            });
            RegisterTest(TestSuiteName, "", (ITestResult testResult) =>
            {
                return true;
            });
            RegisterTest(TestSuiteName, "", (ITestResult testResult) =>
            {
                return true;
            });
            RegisterTest(TestSuiteName, "", (ITestResult testResult) =>
            {
                return true;
            });
            RegisterTest(TestSuiteName, "", (ITestResult testResult) =>
            {
                return true;
            });
        });
    }
}
