using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;

namespace Jacobi.Vst3.TestSuite
{
    public class UnitInfoTest : TestBase
    {
        public override string Name => "Scan Units";

        public UnitInfoTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller is IUnitInfo iUnitInfo)
            {
                var unitCount = iUnitInfo.GetUnitCount();
                if (unitCount <= 0) testResult.AddMessage("No units found, while controller implements IUnitInfo !!!");
                else testResult.AddMessage($"This component has {unitCount} unit(s).");

                var unitIds = new int[unitCount];
                for (var unitIndex = 0; unitIndex < unitCount; unitIndex++)
                {
                    if (iUnitInfo.GetUnitInfo(unitIndex, out var unitInfo) == TResult.S_OK)
                    {
                        var unitId = unitInfo.Id;
                        unitIds[unitIndex] = unitId;
                        if (unitId < 0)
                        {
                            testResult.AddErrorMessage($"Unit {unitIndex,3}: Invalid ID!");
                            return false;
                        }

                        // check if ID is already used by another unit
                        for (var idIndex = 0; idIndex < unitIndex; idIndex++)
                            if (unitIds[idIndex] == unitIds[unitIndex])
                            {
                                testResult.AddErrorMessage($"Unit {unitIndex,3}: ID already used!!!");
                                return false;
                            }

                        var unitName = unitInfo.Name;
                        if (string.IsNullOrEmpty(unitName))
                        {
                            testResult.AddErrorMessage($"Unit {unitIndex,3}: No name!");
                            return false;
                        }

                        var parentUnitId = unitInfo.ParentUnitId;
                        if (parentUnitId < -1)
                        {
                            testResult.AddErrorMessage($"Unit {unitIndex,3}: Invalid parent ID!");
                            return false;
                        }
                        else if (parentUnitId == unitId)
                        {

                            testResult.AddErrorMessage($"Unit {unitIndex,3}: Parent ID is equal to Unit ID!");
                            return false;
                        }

                        var unitProgramListId = unitInfo.ProgramListId;
                        if (unitProgramListId < -1)
                        {
                            testResult.AddErrorMessage($"Unit {unitIndex,3}: Invalid programlist ID!");
                            return false;
                        }

                        testResult.AddMessage("   Unit{unitIndex,3} (ID = {unitId}): \"{unitName}\" (parent ID = {parentUnitId}, programlist ID = {unitProgramListId})");

                        // test select Unit
                        if (iUnitInfo.SelectUnit(unitIndex) == TResult.S_True)
                        {
                            var newSelected = iUnitInfo.GetSelectedUnit();
                            if (newSelected != unitIndex)
                                testResult.AddMessage($"The host has selected Unit ID = {unitIndex} but getSelectedUnit returns ID = {newSelected}!!!");
                        }
                    }
                    else testResult.AddMessage($"Unit{unitIndex,3}: No unit info!");
                }
            }
            else testResult.AddMessage("This component has no units.");
            return true;
        }
    }
}
