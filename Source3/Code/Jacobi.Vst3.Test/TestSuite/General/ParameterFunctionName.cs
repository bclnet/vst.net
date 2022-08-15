using Jacobi.Vst3.Core;
using Jacobi.Vst3.Hosting;
using System.Collections.Generic;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Parameter Function Name.
    /// </summary>
    public class ParameterFunctionNameTest : TestBase
    {
        public override string Name => "Parameter Function Name";

        public ParameterFunctionNameTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            if (controller == null)
            {
                testResult.AddMessage("No Edit Controller supplied!");
                return true;
            }

            if (controller is not IParameterFunctionName iParameterFunctionName)
            {
                testResult.AddMessage("No IParameterFunctionName support");
                return true;
            }

            testResult.AddMessage("IParameterFunctionName supported.");

            var numParameters = controller.GetParameterCount();
            if (numParameters <= 0)
            {
                testResult.AddMessage("This component does not export any parameters!");
                return true;
            }

            // used for ID check
            var paramIds = new Dictionary<uint, int>();
            for (var i = 0; i < numParameters; ++i)
            {
                var result = controller.GetParameterInfo(i, out var paramInfo);
                if (result != kResultOk)
                {
                    testResult.AddErrorMessage($"Parameter {i,3}: is missing!!!");
                    return false;
                }

                var paramId = paramInfo.Id;
                if (paramId < 0)
                {
                    testResult.AddErrorMessage($"Parameter {i,3} (id={paramId}): Invalid Id!!!");
                    return false;
                }

                var search = paramIds.GetValueOrDefault(paramId);
                if (search != default)
                {
                    testResult.AddErrorMessage($"Parameter {paramId,3} (id={i}): ID already used by idx={search,3}!!!");
                    return false;
                }
                else paramIds[paramId] = i;
            } // end for each parameter

            //var iUnitInfo2 = controller as IUnitInfo;
            var arrayFunctionName = new string[] {
                FunctionNameType.CompGainReduction,
                FunctionNameType.CompGainReductionMax,
                FunctionNameType.CompGainReductionPeakHold,
                FunctionNameType.CompResetGainReductionMax,
                FunctionNameType.LowLatencyMode,
                FunctionNameType.Randomize,
                FunctionNameType.DryWetMix};
            var paramID = 0U;
            foreach (var item in arrayFunctionName)
                if (iParameterFunctionName.GetParameterIDFromFunctionName(UnitInfo.RootUnitId, item, ref paramID) == kResultTrue)
                {
                    testResult.AddMessage($"FunctionName {item} supported => paramID {paramID}");

                    var search = paramIds.GetValueOrDefault(paramID);
                    if (search == default)
                    {
                        testResult.AddErrorMessage($"Parameter (id={paramID}) for FunctionName {item}: not Found!!!");
                        return false;
                    }
                }

            return true;
        }
    }
}
