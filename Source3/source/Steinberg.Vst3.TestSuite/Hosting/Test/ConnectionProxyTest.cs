using static Steinberg.Vst3.Utility.Testing;
using static Steinberg.Vst3.TResult;
using System.Threading;

namespace Steinberg.Vst3.Hosting.Test
{
    public static class ConnectionProxyTest
    {
        class ConnectionPoint : IConnectionPoint
        {
            public IConnectionPoint Other;
            public bool MessageReceived;

            public TResult Connect(IConnectionPoint inOther)
            {
                Other = inOther;
                return kResultTrue;
            }

            public TResult Disconnect(IConnectionPoint inOther)
            {
                return inOther != Other
                    ? kResultFalse
                    : kResultTrue;
            }

            public TResult Notify(IMessage message)
            {
                MessageReceived = true;
                return kResultTrue;
            }
        }

        public static void Touch() { var _ = InitTests; }

        static ModuleInitializer InitTests = new(() =>
        {
            const string TestSuiteName = "ConnectionProxy";
            RegisterTest(TestSuiteName, "Connect and disconnect", (ITestResult testResult) =>
            {
                ConnectionPoint cp1 = new();
                ConnectionPoint cp2 = new();
                ConnectionProxy proxy = new(cp1);
                testResult.ExpectEQ(proxy.Connect(cp2), kResultTrue);
                testResult.ExpectEQ(proxy.Disconnect(cp2), kResultTrue);
                return true;
            });
            RegisterTest(TestSuiteName, "Disconnect wrong object", (ITestResult testResult) =>
            {
                ConnectionPoint cp1 = new();
                ConnectionPoint cp2 = new();
                ConnectionPoint cp3 = new();
                ConnectionProxy proxy = new(cp1);
                testResult.ExpectEQ(proxy.Connect(cp2), kResultTrue);
                testResult.ExpectNE(proxy.Disconnect(cp3), kResultTrue);
                return true;
            });
            RegisterTest(TestSuiteName, "Send message on UI thread", (ITestResult testResult) =>
            {
                ConnectionPoint cp1 = new();
                ConnectionPoint cp2 = new();
                ConnectionProxy proxy = new(cp1);
                testResult.ExpectEQ(proxy.Connect(cp2), kResultTrue);
                testResult.ExpectFalse(cp2.MessageReceived);
                HostMessage msg = new();
                testResult.ExpectEQ(proxy.Notify(msg), kResultTrue);
                testResult.ExpectTrue(cp2.MessageReceived);
                return true;
            });
            RegisterTest(TestSuiteName, "Send message on 2nd thread", (ITestResult testResult) =>
            {
                ConnectionPoint cp1 = new();
                ConnectionPoint cp2 = new();
                ConnectionProxy proxy = new(cp1);
                testResult.ExpectEQ(proxy.Connect(cp2), kResultTrue);
                testResult.ExpectFalse(cp2.MessageReceived);

                ManualResetEvent c1 = new(false);
                Mutex m1 = new();
                Mutex m2 = new();
                var automicNotifyResult = 0;
                var thread = new Thread(new ThreadStart(() =>
                {
                    m2.WaitOne();
                    HostMessage msg = new();
                    Interlocked.Exchange(ref automicNotifyResult, (int)proxy.Notify(msg));
                    c1.Set();
                    m2.ReleaseMutex();
                }))
                { Name = "2nd thread" };

                m2.WaitOne();
                thread.Start();
                m2.ReleaseMutex();
                c1.WaitOne();
                var notifyResult = (TResult)Interlocked.CompareExchange(ref automicNotifyResult, 0, 0);
                testResult.ExpectNE(notifyResult, kResultTrue);
                testResult.ExpectFalse(cp2.MessageReceived);
                thread.Join();
                return true;
            });
        });
    }
}
