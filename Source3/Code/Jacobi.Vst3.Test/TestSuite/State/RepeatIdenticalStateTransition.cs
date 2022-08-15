﻿using Jacobi.Vst3.Core;
using Jacobi.Vst3.Hosting;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.TestSuite
{
    public class RepeatIdenticalStateTransitionTest : TestEnh
    {
        public override string Name => "Repeat Identical State Transition";

        public RepeatIdenticalStateTransitionTest(ITestPlugProvider plugProvider) : base(plugProvider, SymbolicSampleSizes.Sample32) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null || audioEffect == null) return false;

            PrintTestHeader(testResult);

            var result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result != kResultFalse) return false;

            result = audioEffect.SetupProcessing(processSetup);
            if (result != kResultTrue) return false;

            result = vstPlug.SetActive(true);
            if (result != kResultOk) return false;

            result = vstPlug.SetActive(true);
            if (result != kResultFalse) return false;

            result = vstPlug.SetActive(false);
            if (result != kResultOk) return false;

            result = vstPlug.SetActive(false);
            if (result == kResultOk) return false;

            result = vstPlug.Terminate();
            if (result != kResultOk) return false;

            result = vstPlug.Terminate();
            if (result == kResultOk) return false;

            result = vstPlug.Initialize(TestingPluginContext.Get());
            if (result != kResultOk) return false;

            return true;
        }
    }
}
