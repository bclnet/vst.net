﻿using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;
using System;
using System.Collections.Generic;

namespace Jacobi.Vst3.TestSuite
{
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
                ParameterInfo paramInfo = new();
                var result = controller.GetParameterInfo(i, ref paramInfo);
                if (result != TResult.S_OK)
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
                FunctionNameTypes.CompGainReduction,
                FunctionNameTypes.CompGainReductionMax,
                FunctionNameTypes.CompGainReductionPeakHold,
                FunctionNameTypes.CompResetGainReductionMax,
                FunctionNameTypes.LowLatencyMode,
                FunctionNameTypes.Randomize,
                FunctionNameTypes.DryWetMix};
            var paramID = 0U;
            foreach (var item in arrayFunctionName)
                if (iParameterFunctionName.GetParameterIDFromFunctionName(UnitInfo.RootUnitId, item, ref paramID) == TResult.S_True)
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
