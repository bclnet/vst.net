using Jacobi.Vst3.Core;
using Jacobi.Vst3.Hosting;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Note Expression.
    /// </summary>
    public unsafe class NoteExpressionTest : TestBase
    {
        public override string Name => "Note Expression";

        public NoteExpressionTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller == null)
            {
                testResult.AddMessage("No Edit Controller supplied!");
                return true;
            }

            if (controller is not INoteExpressionController noteExpression)
            {
                testResult.AddMessage("No Note Expression interface supplied!");
                return true;
            }

            if (controller is not INoteExpressionPhysicalUIMapping noteExpressionPUIMapping)
            {
                testResult.AddMessage("No Note Expression PhysicalUIMapping interface supplied!");
                return true;
            }

            var eventBusCount = vstPlug.GetBusCount(MediaTypes.Event, BusDirections.Input);
            var maxPUI = (uint)PhysicalUITypeIDs.PUITypeCount;
            var puiArray = stackalloc PhysicalUIMap[(int)maxPUI];
            var puiMap = new PhysicalUIMapList
            {
                Count = maxPUI,
                Map = puiArray
            };
            for (var i = 0U; i < maxPUI; i++) puiMap.Map[i].PhysicalUITypeID = i;

            for (var bus = 0; bus < eventBusCount; bus++)
            {
                vstPlug.GetBusInfo(MediaTypes.Event, BusDirections.Input, bus, out var busInfo);

                for (short channel = 0; channel < busInfo.ChannelCount; channel++)
                {
                    var count = noteExpression.GetNoteExpressionCount(bus, channel);
                    if (count > 0) testResult.AddMessage($"Note Expression count bus[{bus}], channel[{channel}]: {count}");

                    for (var i = 0; i < count; ++i)
                    {
                        NoteExpressionTypeInfo info = new();
                        if (noteExpression.GetNoteExpressionInfo(bus, channel, i, ref info) == kResultTrue)
                        {
                            testResult.AddMessage($"Note Expression TypeID: {info.TypeId} [{info.Title}]");
                            var id = info.TypeId;
                            var valueNormalized = info.ValueDesc.DefaultValue;
                            var str = string.Empty;
                            if (noteExpression.GetNoteExpressionStringByValue(bus, channel, id, valueNormalized, ref str) != kResultTrue)
                                testResult.AddMessage($"Note Expression getNoteExpressionStringByValue ({bus}, {channel}, {id}) return kResultFalse!");
                            if (noteExpression.GetNoteExpressionValueByString(bus, channel, id, str, valueNormalized) != kResultTrue)
                                testResult.AddMessage($"Note Expression getNoteExpressionValueByString ({bus}, {channel}, {id}) return kResultFalse!");
                        }
                        else
                        {
                            testResult.AddErrorMessage($"Note Expression getNoteExpressionInfo ({bus}, {channel}, {i}) return kResultFalse!");
                            return false;
                        }
                    }

                    if (noteExpressionPUIMapping != null)
                    {
                        for (var i = 0; i < maxPUI; i++)
                            puiMap.Map[i].NoteExpressionTypeID = (uint)NoteExpressionTypeIDs.InvalidTypeID;

                        if (noteExpressionPUIMapping.GetPhysicalUIMapping(bus, channel, puiMap) == kResultFalse)
                            testResult.AddMessage("Note Expression getPhysicalUIMapping ({bus}, {channel}, ...) return kResultFalse!");
                        else
                            for (var i = 0; i < maxPUI; i++)
                                testResult.AddMessage($"Note Expression PhysicalUIMapping: {puiMap.Map[i].NoteExpressionTypeID} => {puiMap.Map[i].PhysicalUITypeID}");
                    }
                }
            }

            return true;
        }
    }
}
