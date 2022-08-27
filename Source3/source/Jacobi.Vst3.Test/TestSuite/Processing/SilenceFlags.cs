using Jacobi.Vst3;
using System;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Silence Flags.
    /// </summary>
    public unsafe class SilenceFlagsTest : ProcessTest
    {
        public override string Name => "Silence Flags";

        public SilenceFlagsTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            if (vstPlug == null || testResult == null || audioEffect == null) return false;
            if (!CanProcessSampleSize(testResult)) return true;

            PrintTestHeader(testResult);

            if (processData._.Inputs != IntPtr.Zero)
            {
                audioEffect.SetProcessing(true);

                for (var inputsIndex = 0; inputsIndex < processData._.NumInputs; inputsIndex++)
                {
                    var numSilenceFlagsCombinations = (1 << processData._.InputsX[inputsIndex].NumChannels) - 1;
                    for (var flagCombination = 0; flagCombination <= numSilenceFlagsCombinations; flagCombination++)
                    {
                        processData._.InputsX[inputsIndex].SilenceFlags = (ulong)flagCombination;
                        var result = audioEffect.Process(processData._);
                        if (result != kResultOk)
                        {
                            testResult.AddErrorMessage("The component failed to process bus {inputsIndex} with silence flag combination {flagCombination}!");
                            audioEffect.SetProcessing(false);
                            return false;
                        }
                    }
                }
            }
            else if (processData._.NumInputs > 0)
            {
                testResult.AddErrorMessage("ProcessData::inputs are 0 but ProcessData::numInputs are nonzero.");
                return false;
            }

            audioEffect.SetProcessing(false);
            return true;
        }
    }
}
