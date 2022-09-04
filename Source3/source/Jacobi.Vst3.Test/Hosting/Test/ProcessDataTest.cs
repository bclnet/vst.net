using static Steinberg.Vst3.Utility.Testing;

namespace Steinberg.Vst3.Hosting.Test
{
    public static class ProcessDataTest
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
