using static Steinberg.Vst3.TResult;
using static Steinberg.Vst3.Utility.Testing;

namespace Steinberg.Vst3.Hosting.Test
{
    public static class EventListTest
    {
        public static void Touch() { var _ = EventListTests; }

        static ModuleInitializer EventListTests = new(() =>
        {
            const string TestSuiteName = "EventList";
            RegisterTest(TestSuiteName, "Set and get single event", (ITestResult testResult) =>
            {
                EventList eventList = new();
                Event event1 = default;
                event1.Type = Event.EventTypes.NoteOnEvent;
                event1.NoteOn.NoteId = 10;
                testResult.ExpectEQ(eventList.AddEvent(ref event1), kResultTrue);
                testResult.ExpectEQ(eventList.GetEvent(0, out var event2), kResultTrue);
                testResult.ExpectTrue(event1 == event2);
                return true;
            });
            RegisterTest(TestSuiteName, "Count events", (ITestResult testResult) =>
            {
                EventList eventList = new();
                Event eventx = default;
                for (var i = 0; i < 20; ++i)
                    testResult.ExpectEQ(eventList.AddEvent(ref eventx), kResultTrue);
                testResult.ExpectEQ(eventList.GetEventCount(), 20);
                return true;
            });
            RegisterTest(TestSuiteName, "Overflow", (ITestResult testResult) =>
            {
                EventList eventList = new(20);
                Event eventx = default;
                for (var i = 0; i < 20; ++i)
                    testResult.ExpectEQ(eventList.AddEvent(ref eventx), kResultTrue);
                testResult.ExpectEQ(eventList.GetEventCount(), 20);
                testResult.ExpectNE(eventList.AddEvent(ref eventx), kResultTrue);
                testResult.ExpectEQ(eventList.GetEventCount(), 20);
                return true;
            });
            RegisterTest(TestSuiteName, "Get unknown event", (ITestResult testResult) =>
            {
                EventList eventList = new();
                testResult.ExpectNE(eventList.GetEvent(0, out var eventx), kResultTrue);
                return true;
            });
            RegisterTest(TestSuiteName, "Resize", (ITestResult testResult) =>
            {
                EventList eventList = new(1);
                testResult.ExpectNE(eventList.GetEvent(0, out var eventx), kResultTrue);

                eventx = default;
                testResult.ExpectEQ(eventList.AddEvent(ref eventx), kResultTrue);
                testResult.ExpectEQ(eventList.GetEventCount(), 1);
                testResult.ExpectNE(eventList.AddEvent(ref eventx), kResultTrue);
                testResult.ExpectEQ(eventList.GetEventCount(), 1);
                eventList.SetMaxSize(2);
                testResult.ExpectEQ(eventList.GetEventCount(), 0);
                testResult.ExpectEQ(eventList.AddEvent(ref eventx), kResultTrue);
                testResult.ExpectEQ(eventList.AddEvent(ref eventx), kResultTrue);
                testResult.ExpectEQ(eventList.GetEventCount(), 2);
                testResult.ExpectNE(eventList.AddEvent(ref eventx), kResultTrue);
                testResult.ExpectEQ(eventList.GetEventCount(), 2);
                return true;
            });
        });
    }
}
