using Jacobi.Vst3;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.TestSuite
{
    public class UnitStructureTest : TestBase
    {
        public override string Name => "Check Unit Structure";

        public UnitStructureTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller is IUnitInfo iUnitInfo)
            {
                var unitCount = iUnitInfo.GetUnitCount();
                if (unitCount <= 0) testResult.AddMessage("No units found, while controller implements IUnitInfo !!!");

                var rootFound = false;
                for (var unitIndex = 0; unitIndex < unitCount; unitIndex++)
                {
                    if (iUnitInfo.GetUnitInfo(unitIndex, out var unitInfo) == kResultOk)
                    {
                        // check parent Id
                        if (unitInfo.ParentUnitId != UnitInfo.NoParentUnitId) //-1: connected to root
                        {
                            var noParent = true;
                            for (var i = 0; i < unitCount; ++i)
                            {
                                if (iUnitInfo.GetUnitInfo(i, out var tmpInfo) == kResultOk)
                                    if (unitInfo.ParentUnitId == tmpInfo.Id)
                                    {
                                        noParent = false;
                                        break;
                                    }
                            }
                            if (noParent && unitInfo.ParentUnitId != UnitInfo.RootUnitId)
                            {
                                testResult.AddErrorMessage($"Unit {unitInfo.Id,3}: Parent does not exist!!");
                                return false;
                            }
                        }
                        else if (!rootFound)
                        {
                            // root Unit have always the rootID
                            if (unitInfo.Id != UnitInfo.RootUnitId)
                            {
                                // we should have a root unit id
                                testResult.AddErrorMessage($"Unit {unitInfo.Id,3}: Should be the Root Unit => id should be {UnitInfo.RootUnitId,3}!!");
                                return false;
                            }
                            rootFound = true;
                        }
                        else
                        {
                            testResult.AddErrorMessage("Unit {unitInfo.Id,3}: Has no parent, but there is a root already.");
                            return false;
                        }
                    }
                    else
                    {
                        testResult.AddErrorMessage($"Unit {unitInfo.Id,3}: No unit info.");
                        return false;
                    }
                }
                testResult.AddMessage("All units have valid parent IDs.");
            }
            else testResult.AddMessage("This component does not support IUnitInfo!");
            return true;
        }
    }
}
