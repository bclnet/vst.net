using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;
using System.Text;

namespace Jacobi.Vst3.Test.Unit
{
    public class ProgramInfoTest : TestBase
    {
        public override string Name => "Scan Programs";

        public ProgramInfoTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller is IUnitInfo iUnitInfo)
            {
                var programListCount = iUnitInfo.GetProgramListCount();
                if (programListCount == 0)
                {
                    testResult.AddMessage("This component does not export any programs.");
                    return true;
                }
                else if (programListCount < 0)
                {
                    testResult.AddErrorMessage("IUnitInfo::getProgramListCount () returned a negative number.");
                    return false;
                }

                // used to check double IDs
                var programListIds = new int[programListCount];

                for (var programListIndex = 0; programListIndex < programListCount; programListIndex++)
                {
                    // get programm list info
                    ProgramListInfo programListInfo = new();
                    if (iUnitInfo.GetProgramListInfo(programListIndex, ref programListInfo) == TResult.S_OK)
                    {
                        var programListId = programListInfo.Id;
                        programListIds[programListIndex] = programListId;
                        if (programListId < 0)
                        {
                            testResult.AddErrorMessage($"Programlist {programListIndex,3}: Invalid ID!!!");
                            return false;
                        }

                        // check if ID is already used by another parameter
                        for (var idIndex = 0; idIndex < programListIndex; idIndex++)
                            if (programListIds[idIndex] == programListIds[programListIndex])
                            {
                                testResult.AddErrorMessage($"Programlist {programListIndex,3}: ID already used!!!");
                                return false;
                            }

                        var programListName = programListInfo.Name;
                        if (string.IsNullOrEmpty(programListName))
                        {
                            testResult.AddErrorMessage($"Programlist {programListIndex,3} (id={programListId}): No name!!!");
                            return false;
                        }

                        var programCount = programListInfo.ProgramCount;
                        if (programCount <= 0)
                        {
                            testResult.AddMessage($"Programlist {programListIndex,3} (id={programListId}): \"{programListName}\" No programs!!! (programCount is null!)");
                            // return false;
                        }

                        testResult.AddMessage($"Programlist {programListIndex,3} (id={programListId}):  \"{programListName}\" ({programCount} programs).");

                        for (var programIndex = 0; programIndex < programCount; programIndex++)
                        {
                            StringBuilder programName = new();
                            if (iUnitInfo.GetProgramName(programListId, programIndex, programName) == TResult.S_OK)
                            {
                                if (programName[0] == 0)
                                {

                                    testResult.AddErrorMessage($"Programlist {programListIndex,3}03d->Program {programIndex,3}: has no name!!!");
                                    return false;
                                }

                                var programNameUTF8 = programName.ToString();
                                var msg = $"Programlist {programListIndex,3}->Program {programIndex,3}: \"{programNameUTF8}\"";

                                StringBuilder programInfo = new();
                                if (iUnitInfo.GetProgramInfo(programListId, programIndex, AttributeIds.Instrument, programInfo) == TResult.S_OK)
                                    msg += $" (instrument = \"{programInfo}\")";

                                testResult.AddMessage(msg);

                                if (iUnitInfo.HasProgramPitchNames(programListId, programIndex) == TResult.S_OK)
                                {
                                    testResult.AddMessage($" => \"{programNameUTF8}\": supports PitchNames");

                                    StringBuilder pitchName = new();
                                    for (short midiPitch = 0; midiPitch < 128; midiPitch++)
                                    {
                                        if (iUnitInfo.GetProgramPitchName(programListId, programIndex, midiPitch, pitchName) == TResult.S_OK)
                                            msg = $"   => MIDI Pitch {midiPitch} => \"{pitchName}\"";
                                        testResult.AddMessage(msg);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                testResult.AddMessage("This component does not export any programs.");

                // check if not more than 1 program change parameter is defined
                var numPrgChanges = 0;
                for (var i = 0; i < controller.GetParameterCount(); ++i)
                {
                    ParameterInfo paramInfo = new();
                    if (controller.GetParameterInfo(i, paramInfo) != TResult.S_OK)
                        if ((paramInfo.Flags & ParameterInfo.ParameterFlags.IsProgramChange) != 0) numPrgChanges++;
                }
                if (numPrgChanges > 1) testResult.AddErrorMessage($"More than 1 programChange Parameter ({numPrgChanges}) without support of IUnitInfo!!!");
            }
            return true;
        }
    }
}
