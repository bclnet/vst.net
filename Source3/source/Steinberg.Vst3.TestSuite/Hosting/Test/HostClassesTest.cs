using System;
using static Steinberg.Vst3.Utility.Testing;
using static Steinberg.Vst3.TResult;
using System.Text;

namespace Steinberg.Vst3.Hosting.Test
{
    public static class HostClassesTest
    {
        public static void Touch() { var _ = HostApplicationTests; }

        static ModuleInitializer HostApplicationTests = new(() =>
        {
            const string TestSuiteName = "HostApplication";
            RegisterTest(TestSuiteName, "Create instance of IAttributeList", (ITestResult testResult) =>
            {
                HostApplication hostApp = new();
                IntPtr instance;
                Guid iid = typeof(IAttributeList).GUID;
                testResult.ExpectEQ(hostApp.CreateInstance(ref iid, ref iid, out instance), kResultTrue);
                testResult.ExpectNE(instance, IntPtr.Zero);
                //instance.Release();
                return true;
            });
            RegisterTest(TestSuiteName, "Create instance of IMessage", (ITestResult testResult) =>
            {
                HostApplication hostApp = new();
                IntPtr instance;
                Guid iid = typeof(IMessage).GUID;
                testResult.ExpectEQ(hostApp.CreateInstance(ref iid, ref iid, out instance), kResultTrue);
                testResult.ExpectNE(instance, IntPtr.Zero);
                //instance.Release();
                return true;
            });
        });

        static ModuleInitializer HostAttributeListTests = new(() =>
        {
            const string TestSuiteName = "HostAttributeList";
            RegisterTest(TestSuiteName, "Int", (ITestResult testResult) =>
            {
                var attrList = new HostAttributeList();
                const long testValue = 5L;
                testResult.ExpectEQ(attrList.SetInt("Int", testValue), kResultTrue);
                var value = 0L;
                testResult.ExpectEQ(attrList.GetInt("Int", out value), kResultTrue);
                testResult.ExpectEQ(value, testValue);
                return true;
            });
            RegisterTest(TestSuiteName, "Float", (ITestResult testResult) =>
            {
                var attrList = new HostAttributeList();
                const double testValue = 2.636D;
                testResult.ExpectEQ(attrList.SetFloat("Float", testValue), kResultTrue);
                var value = 0D;
                testResult.ExpectEQ(attrList.GetFloat("Float", out value), kResultTrue);
                testResult.ExpectEQ(value, testValue);
                return true;
            });
            RegisterTest(TestSuiteName, "String", (ITestResult testResult) =>
            {
                var attrList = new HostAttributeList();
                const string testValue = "TestValue";
                testResult.ExpectEQ(attrList.SetString("Str", testValue), kResultTrue);
                StringBuilder value = new();
                var valueSize = 10U * sizeof(char);
                testResult.ExpectEQ(attrList.GetString("Str", value, ref valueSize), kResultTrue);
                testResult.ExpectTrue(testValue == value.ToString());
                return true;
            });
            //TODO
            //RegisterTest(TestSuiteName, "Binary", (ITestResult testResult) =>
            //{
            //    var attrList = new HostAttributeList();
            //    var testData = new int[20];
            //    var testDataSize = (uint)(testData.Length * sizeof(int));
            //    testResult.ExpectEQ(attrList.SetBinary("Binary", testData.data(), testDataSize), kResultTrue);
            //    byte[] data;
            //    var dataSize = 0U;
            //    testResult.ExpectEQ(attrList.GetBinary("Binary", data, ref dataSize), kResultTrue);
            //    testResult.ExpectEQ(dataSize, testDataSize);
            //    var s = (int)data;
            //    foreach (var i in testData)
            //    {
            //        testResult.ExpectEQ(i, s);
            //        s++;
            //    }
            //    return true;
            //});
            RegisterTest(TestSuiteName, "Multiple Set", (ITestResult testResult) =>
            {
                var attrList = new HostAttributeList();
                const long testValue1 = 5L;
                const long testValue2 = 6L;
                const long testValue3 = 7L;
                testResult.ExpectEQ(attrList.SetInt("Int", testValue1), kResultTrue);
                testResult.ExpectEQ(attrList.SetInt("Int", testValue2), kResultTrue);
                testResult.ExpectEQ(attrList.SetInt("Int", testValue3), kResultTrue);
                var value = 0L;
                testResult.ExpectEQ(attrList.GetInt("Int", out value), kResultTrue);
                testResult.ExpectEQ(value, testValue3);
                return true;
            });
        });
    }
}
