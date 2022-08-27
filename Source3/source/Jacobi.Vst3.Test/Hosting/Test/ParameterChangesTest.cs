using Jacobi.Vst3;
using static Jacobi.Vst3.Utility.Testing;
using ParamID = System.UInt32;
using ParamValue = System.Double;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.Hosting.Test
{
    public static class ParameterChangesTest
    {
        public static void Touch() { var _ = ParameterValueQueueTests; }

        struct ValuePoint
        {
            public int SampleOffset;
            public ParamValue Value;
            public ValuePoint(int sampleOffset, ParamValue value) { SampleOffset = sampleOffset; Value = value; }
        }

        static ModuleInitializer ParameterValueQueueTests = new(() =>
        {
            const string TestSuiteName = "ParameterValueQueue";
            RegisterTest(TestSuiteName, "Set paramID", (ITestResult testResult) =>
            {
                ParameterValueQueue queue = new(10);
                testResult.ExpectEQ(queue.GetParameterId(), (ParamID)10);
                queue.SetParamID(5);
                testResult.ExpectEQ(queue.GetParameterId(), (ParamID)5);
                return true;
            });
            RegisterTest(TestSuiteName, "Set/get point", (ITestResult testResult) =>
            {
                ParameterValueQueue queue = new(0);
                ValuePoint vp = new(100, 0.5);
                var index = 0;
                testResult.ExpectEQ(queue.AddPoint(vp.SampleOffset, vp.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.GetPointCount(), 1);
                testResult.ExpectEQ(index, 0);
                ValuePoint test = default;
                testResult.ExpectEQ(queue.GetPoint(index, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp.Value, test.Value);
                return true;
            });
            RegisterTest(TestSuiteName, "Set/get multiple points", (ITestResult testResult) =>
            {
                ParameterValueQueue queue = new(0);
                ValuePoint vp1 = new(10, 0.1);
                ValuePoint vp2 = new(30, 0.3);
                ValuePoint vp3 = new(50, 0.6);
                ValuePoint vp4 = new(70, 0.8);
                var index = 0;
                testResult.ExpectEQ(queue.AddPoint(vp1.SampleOffset, vp1.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.AddPoint(vp2.SampleOffset, vp2.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.AddPoint(vp3.SampleOffset, vp3.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.AddPoint(vp4.SampleOffset, vp4.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.GetPointCount(), 4);
                testResult.ExpectEQ(index, 3);
                ValuePoint test = new();
                testResult.ExpectEQ(queue.GetPoint(0, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp1.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp1.Value, test.Value);
                testResult.ExpectEQ(queue.GetPoint(1, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp2.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp2.Value, test.Value);
                testResult.ExpectEQ(queue.GetPoint(2, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp3.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp3.Value, test.Value);
                testResult.ExpectEQ(queue.GetPoint(3, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp4.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp4.Value, test.Value);
                return true;
            });
            RegisterTest(TestSuiteName, "Ordered points", (ITestResult testResult) =>
            {
                ParameterValueQueue queue = new(0);
                ValuePoint vp1 = new(70, 0.1);
                ValuePoint vp2 = new(50, 0.3);
                ValuePoint vp3 = new(30, 0.6);
                ValuePoint vp4 = new(10, 0.8);
                var index = 0;
                testResult.ExpectEQ(queue.AddPoint(vp1.SampleOffset, vp1.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.AddPoint(vp2.SampleOffset, vp2.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.AddPoint(vp3.SampleOffset, vp3.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.AddPoint(vp4.SampleOffset, vp4.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.GetPointCount(), 4);
                ValuePoint test = new();
                testResult.ExpectEQ(queue.GetPoint(0, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp4.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp4.Value, test.Value);
                testResult.ExpectEQ(queue.GetPoint(1, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp3.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp3.Value, test.Value);
                testResult.ExpectEQ(queue.GetPoint(2, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp2.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp2.Value, test.Value);
                testResult.ExpectEQ(queue.GetPoint(3, out test.SampleOffset, out test.Value), kResultTrue);
                testResult.ExpectEQ(vp1.SampleOffset, test.SampleOffset);
                testResult.ExpectEQ(vp1.Value, test.Value);
                return true;
            });
            RegisterTest(TestSuiteName, "Clear", (ITestResult testResult) =>
            {
                ParameterValueQueue queue = new(0);
                ValuePoint vp = new(100, 0.5);
                var index = 0;
                testResult.ExpectEQ(queue.AddPoint(vp.SampleOffset, vp.Value, out index), kResultTrue);
                testResult.ExpectEQ(queue.GetPointCount(), 1);
                testResult.ExpectEQ(index, 0);
                queue.Clear();
                testResult.ExpectEQ(queue.GetPointCount(), 0);
                ValuePoint test = new();
                testResult.ExpectNE(queue.GetPoint(index, out test.SampleOffset, out test.Value), kResultTrue);
                return true;
            });
        });

        static ModuleInitializer ParameterChangesTests = new(() =>
        {
            const string TestSuiteName = "ParameterChanges";
            RegisterTest(TestSuiteName, "Parameter count", (ITestResult testResult) =>
            {
                ParameterChanges changes = new(1);
                testResult.ExpectEQ(changes.GetParameterCount(), 0);
                var index = 0;
                var queue = changes.AddParameterData(0, out index);
                testResult.ExpectNE(queue, null);
                testResult.ExpectEQ(index, 0);
                testResult.ExpectEQ(changes.GetParameterCount(), 1);
                return true;
            });
            RegisterTest(TestSuiteName, "Clear queue", (ITestResult testResult) =>
            {
                ParameterChanges changes = new(1);
                var index = 0;
                testResult.ExpectEQ(changes.GetParameterCount(), 0);
                changes.AddParameterData(0, out index);
                testResult.ExpectEQ(changes.GetParameterCount(), 1);
                changes.ClearQueue();
                testResult.ExpectEQ(changes.GetParameterCount(), 0);
                return true;
            });
            RegisterTest(TestSuiteName, "Increase max parameters", (ITestResult testResult) =>
            {
                ParameterChanges changes = new(1);
                var index = 0;
                testResult.ExpectEQ(changes.GetParameterCount(), 0);
                changes.AddParameterData(0, out index);
                testResult.ExpectEQ(changes.GetParameterCount(), 1);
                testResult.ExpectNE(changes.AddParameterData(1, out index), null);
                testResult.ExpectEQ(changes.GetParameterCount(), 2);
                changes.SetMaxParameters(4);
                testResult.ExpectEQ(changes.GetParameterCount(), 2);
                return true;
            });
            RegisterTest(TestSuiteName, "Get parameter data", (ITestResult testResult) =>
            {
                ParameterChanges changes = new(1);
                var index = 0;
                var queue1 = changes.AddParameterData(0, out index);
                var queue2 = changes.GetParameterData(index);
                testResult.ExpectEQ(queue1, queue2);
                return true;
            });
        });

        struct ParamChange
        {
            public ParamID Id;
            public ParamValue Value;
            public int SampleOffset;
            public ParamChange(ParamID id, ParamValue value, int sampleOffset) { Id = id; Value = value; SampleOffset = sampleOffset; }

            public static bool operator ==(ParamChange _, ParamChange o)
                => _.Id == o.Id && _.Value == o.Value && _.SampleOffset == o.SampleOffset;
            public static bool operator !=(ParamChange _, ParamChange o)
                => _.Id != o.Id || _.Value != o.Value || _.SampleOffset != o.SampleOffset;
        }

        static ModuleInitializer ParameterChangeTransferTests = new(() =>
        {
            const string TestSuiteName = "ParameterChangeTransfer";
            RegisterTest(TestSuiteName, "Add/get change", (ITestResult testResult) =>
            {
                ParameterChangeTransfer transfer = new(1);
                ParamChange change = new(1, 0.8, 2);
                transfer.AddChange(change.Id, change.Value, change.SampleOffset);
                ParamChange test = default;
                testResult.ExpectNE(change, test);
                testResult.ExpectTrue(transfer.GetNextChange(out test.Id, out test.Value, out test.SampleOffset));
                testResult.ExpectEQ(change, test);
                return true;
            });
            RegisterTest(TestSuiteName, "Remove changes", (ITestResult testResult) =>
            {
                ParameterChangeTransfer transfer = new(1);
                ParamChange change = new(1, 0.8, 2);
                transfer.AddChange(change.Id, change.Value, change.SampleOffset);
                transfer.RemoveChanges();
                ParamChange test = default;
                testResult.ExpectFalse(transfer.GetNextChange(out test.Id, out test.Value, out test.SampleOffset));
                return true;
            });
            RegisterTest(TestSuiteName, "Transfer changes to", (ITestResult testResult) =>
            {
                ParameterChangeTransfer transfer = new(10);
                ParamChange ch1 = new(1, 0.8, 2);
                ParamChange ch2 = new(2, 0.4, 8);
                transfer.AddChange(ch1.Id, ch1.Value, ch1.SampleOffset);
                transfer.AddChange(ch2.Id, ch2.Value, ch2.SampleOffset);
                ParameterChanges changes = new(2);
                transfer.TransferChangesTo(changes);
                testResult.ExpectEQ(changes.GetParameterCount(), 2);
                var valueQueue1 = changes.GetParameterData(0);
                testResult.ExpectNE(valueQueue1, null);
                var valueQueue2 = changes.GetParameterData(1);
                testResult.ExpectNE(valueQueue2, null);
                var pid1 = valueQueue1.GetParameterId();
                var pid2 = valueQueue2.GetParameterId();
                testResult.Expect(() => pid1 == ch1.Id || pid1 == ch2.Id);
                testResult.Expect(() => pid2 == ch1.Id || pid2 == ch2.Id);
                testResult.ExpectNE(pid1, pid2);
                ValuePoint vp1 = new();
                ValuePoint vp2 = new();
                if (pid1 == ch1.Id)
                {
                    testResult.ExpectEQ(valueQueue1.GetPoint(0, out vp1.SampleOffset, out vp1.Value), kResultTrue);
                    testResult.ExpectEQ(valueQueue2.GetPoint(0, out vp2.SampleOffset, out vp2.Value), kResultTrue);
                }
                else
                {
                    testResult.ExpectEQ(valueQueue2.GetPoint(0, out vp1.SampleOffset, out vp1.Value), kResultTrue);
                    testResult.ExpectEQ(valueQueue1.GetPoint(0, out vp2.SampleOffset, out vp2.Value), kResultTrue);
                }
                return true;
            });
            RegisterTest(TestSuiteName, "Transfer changes from", (ITestResult testResult) =>
            {
                ParamChange ch1 = new(1, 0.8, 2);
                ParamChange ch2 = new(2, 0.4, 8);
                ParameterChangeTransfer transfer = new(2);
                ParameterChanges changes = new();
                var index = 0;
                var valueQueue = changes.AddParameterData(ch1.Id, out index);
                testResult.ExpectNE(valueQueue, null);
                testResult.ExpectEQ(valueQueue.AddPoint(ch1.SampleOffset, ch1.Value, out index), kResultTrue);
                valueQueue = changes.AddParameterData(ch2.Id, out index);
                testResult.ExpectNE(valueQueue, null);
                testResult.ExpectEQ(valueQueue.AddPoint(ch2.SampleOffset, ch2.Value, out index), kResultTrue);
                transfer.TransferChangesFrom(changes);
                ParamChange test1 = default;
                ParamChange test2 = default;
                ParamChange test3 = default;
                testResult.ExpectTrue(transfer.GetNextChange(out test1.Id, out test1.Value, out test1.SampleOffset));
                testResult.ExpectTrue(transfer.GetNextChange(out test2.Id, out test2.Value, out test2.SampleOffset));
                testResult.ExpectFalse(transfer.GetNextChange(out test3.Id, out test3.Value, out test3.SampleOffset));
                testResult.Expect(() => test1 == ch1 || test1 == ch2);
                testResult.Expect(() => test2 == ch1 || test2 == ch2);
                testResult.ExpectNE(test1, test2);
                return true;
            });
        });
    }
}
