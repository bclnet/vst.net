﻿//#define TOUGHTESTS
using Steinberg.Vst3;
using System;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
{
    /// <summary>
    /// Test Variable Block Size.
    /// </summary>
    public class VariableBlockSizeTest : ProcessTest
    {
        public override string Name => "Variable Block Size";

        public VariableBlockSizeTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        public override bool Run(ITestResult testResult)
        {
            if (vstPlug == null || testResult == null || audioEffect == null) return false;
            if (!CanProcessSampleSize(testResult)) return true;

            PrintTestHeader(testResult);

            audioEffect.SetProcessing(true);

            var rand = new Random((int)DateTime.Now.Ticks);
            for (var i = 0; i <= TestDefaults.Instance.NumIterations; ++i)
            {
                var sampleFrames = rand.Next() % processSetup.MaxSamplesPerBlock;
                processData._.NumSamples = sampleFrames;
                if (i == 0) processData._.NumSamples = 0;
#if TOUGHTESTS
                else if (i == 1) processData._.NumSamples = -50000;
                else if (i == 2) processData._.NumSamples = processSetup.MaxSamplesPerBlock * 2;
#endif
                var result = audioEffect.Process(processData._);
                if (result != kResultOk
#if TOUGHTESTS
                    && i > 1)
#else
                    && i > 0)
#endif
                {
                    testResult.AddErrorMessage($"The component failed to process an audioblock of size {sampleFrames}");
                    audioEffect.SetProcessing(false);
                    return false;
                }
            }

            audioEffect.SetProcessing(false);
            return true;
        }
    }
}