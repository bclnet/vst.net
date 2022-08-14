using Jacobi.Vst3.Core;
using Jacobi.Vst3.Hosting;
using System.Collections.Generic;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Scan Parameters.
    /// </summary>
    public class ScanParametersTest : TestBase
    {
        public override string Name => "Scan Parameters";

        public ScanParametersTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller == null)
            {
                testResult.AddMessage("No Edit Controller supplied!");
                return true;
            }

            var numParameters = controller.GetParameterCount();
            if (numParameters <= 0)
            {
                testResult.AddMessage("This component does not export any parameters!");
                return true;
            }

            testResult.AddMessage($"This component exports {numParameters} parameter(s)");

            if (controller is IUnitInfo iUnitInfo2 && numParameters > 20)
                testResult.AddMessage("Note: it could be better to use UnitInfo in order to sort Parameters (>20).");

            // used for ID check
            var paramIds = new Dictionary<uint, int>();

            var foundBypass = false;
            for (var i = 0; i < numParameters; ++i)
            {
                var result = controller.GetParameterInfo(i, out var paramInfo);
                if (result != TResult.S_OK)
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3}: is missing!!!");
                    return false;
                }

                var paramId = paramInfo.Id;
                if (paramId < 0)
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}): Invalid Id!!!");
                    return false;
                }

                // check if ID is already used by another parameter
                var search = paramIds.GetValueOrDefault(paramId);
                if (search != default)
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}): ID already used by idx={search,3}!!!");
                    return false;
                }
                else paramIds[paramId] = i;

                string paramType;
                if (paramInfo.StepCount < 0)
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}): invalid stepcount (<0)!!!");
                    return false;
                }
                if (paramInfo.StepCount == 0) paramType = "Float";
                else if (paramInfo.StepCount == 1) paramType = "Toggle";
                else paramType = "Discrete";

                var paramTitle = paramInfo.Title;
                var paramUnits = paramInfo.Units;
                testResult.AddMessage(string.Format(
                    "   Parameter {0,3} (id={1}): [title=\"{2}\"] [unit=\"{3}\"] [type = {4}, default = {5}, unit = {6}]",
                    i, paramId, paramTitle, paramUnits, paramType,
                    paramInfo.DefaultNormalizedValue, paramInfo.UnitId));

                if (string.IsNullOrEmpty(paramTitle))
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}): has no title!!!");
                    return false;
                }

                if (paramInfo.DefaultNormalizedValue != -1d && (paramInfo.DefaultNormalizedValue < 0d || paramInfo.DefaultNormalizedValue > 1d))
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}): defaultValue is not normalized!!!");
                    return false;
                }
                var unitId = paramInfo.UnitId;
                if (unitId < -1)
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}): No appropriate unit ID!!!");
                    return false;
                }
                if (unitId >= -1)
                {
                    var iUnitInfo = controller as IUnitInfo;
                    if (iUnitInfo == null && unitId != 0)
                    {
                        testResult.AddErrorMessage($"IUnitInfo interface is missing, but ParameterInfo::unitID is not {UnitInfo.RootUnitId,3} (kRootUnitId).");
                        return false;
                    }
                    else if (iUnitInfo != null)
                    {
                        var found = false;
                        var uc = iUnitInfo.GetUnitCount();
                        for (var ui = 0; ui < uc; ++ui)
                        {
                            if (iUnitInfo.GetUnitInfo(ui, out var uinfo) != TResult.S_True)
                            {
                                testResult.AddErrorMessage($"IUnitInfo::getUnitInfo ({ui}..) failed.");
                                return false;
                            }
                            if (uinfo.Id == unitId) found = true;
                        }
                        if (!found && unitId != UnitInfo.RootUnitId)
                        {

                            testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}) has a UnitID ({unitId}), which isn't defined in IUnitInfo.");
                            return false;
                        }
                    }
                }

                //---check for incompatible flags---------------------
                // kCanAutomate and kIsReadOnly
                if ((paramInfo.Flags & ParameterInfo.ParameterFlags.CanAutomate) != 0 && (paramInfo.Flags & ParameterInfo.ParameterFlags.IsReadOnly) != 0)
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}) must not be kCanAutomate and kReadOnly at the same time.");
                    return false;
                }
                // kIsProgramChange and kIsReadOnly
                if ((paramInfo.Flags & ParameterInfo.ParameterFlags.IsProgramChange) != 0 && (paramInfo.Flags & ParameterInfo.ParameterFlags.IsReadOnly) != 0)
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}) must not be kIsProgramChange and kReadOnly at the same time.");
                    return false;
                }
                // kIsBypass only or kIsBypass and kCanAutomate only
                if ((paramInfo.Flags & ParameterInfo.ParameterFlags.IsBypass) != 0 && !(paramInfo.Flags == ParameterInfo.ParameterFlags.IsBypass || (paramInfo.Flags & ParameterInfo.ParameterFlags.CanAutomate) != 0))
                {
                    testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}) is kIsBypass and could have only kCanAutomate as other flag at the same time.");
                    return false;
                }

                //---maybe wrong combination of flags-------------------
                // kIsBypass but not kCanAutomate
                if (paramInfo.Flags == ParameterInfo.ParameterFlags.IsBypass)
                    testResult.AddMessage($"=>Parameter {i,3} (id={paramId}) is kIsBypass, but not kCanAutomate!");
                // kIsHidden and (kCanAutomate or not kIsReadOnly)
                if (paramInfo.Flags == ParameterInfo.ParameterFlags.IsHidden)
                {
                    if ((paramInfo.Flags & ParameterInfo.ParameterFlags.CanAutomate) != 0)
                        testResult.AddMessage($"=>Parameter {i,3} (id={paramId}) is kIsHidden and kCanAutomate!");
                    if ((paramInfo.Flags & ParameterInfo.ParameterFlags.IsReadOnly) == 0)
                        testResult.AddMessage($"=>Parameter {i,3} (id={paramId}) is kIsHidden and NOT kIsReadOnly!");
                }

                // kIsProgramChange and not kIsList
                if ((paramInfo.Flags & ParameterInfo.ParameterFlags.IsProgramChange) != 0 && (paramInfo.Flags & ParameterInfo.ParameterFlags.IsList) == 0)
                    testResult.AddMessage($"=>Parameter {i,3} (id={paramId}) is kIsProgramChange, but not a kIsList!");
                // kIsReadOnly and kIsWrapAround
                if ((paramInfo.Flags & ParameterInfo.ParameterFlags.IsReadOnly) != 0 && (paramInfo.Flags & ParameterInfo.ParameterFlags.IsWrapAround) != 0)
                    testResult.AddMessage($"=>Parameter {i,3} (id={paramId}) is kIsReadOnly, no need to be kIsWrapAround too!");

                //---check bypass--------------------------------------
                if ((paramInfo.Flags & ParameterInfo.ParameterFlags.IsBypass) != 0)
                {
                    if (!foundBypass) foundBypass = true;
                    else
                    {
                        testResult.AddErrorMessage($"=>Parameter {i,3} (id={paramId}): There can only be one bypass (kIsBypass).");
                        return false;
                    }
                }
            } // end for each parameter

            if (foundBypass == false)
            {
                StringResult subCat = new();
                plugProvider.GetSubCategories(subCat);
                if (subCat.Get().Contains("Instrument")) testResult.AddMessage("No bypass parameter found. This is an instrument.");
                else testResult.AddMessage("Warning: No bypass parameter found. Is this intended ?");
            }

            return true;
        }
    }
}
