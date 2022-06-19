﻿using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jacobi.Vst3.TestSuite
{
    public class AutomationTest : ProcessTest, IParameterChanges
    {
        protected uint bypassId = ParameterInfo.NoParamId;
        protected readonly List<ParamChanges> paramChanges = new();
        protected int countParamChanges;
        protected int everyNSamples;
        protected int numParams;
        protected bool sampleAccuracy;
        protected bool onceExecuted;

        public override string Name => $"Accuracy: {(sampleAccuracy ? "Sample" : "Block")}{(numParams < 1 ? ", All Parameters" : $", {numParams} Parameters")}, Change every{everyNSamples} Samples";

        public AutomationTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl, int everyNSamples, int numParams, bool sampleAccuracy) : base(plugProvider, sampl)
        {
            this.everyNSamples = everyNSamples;
            this.numParams = numParams;
            this.sampleAccuracy = sampleAccuracy;
        }

        //int QueryInterface(Guid _iid, object obj)
        //{
        //    QUERY_INTERFACE(_iid, obj, FUnknown::iid, IParameterChanges);
        //    QUERY_INTERFACE(_iid, obj, Vst::IParameterChanges::iid, IParameterChanges);
        //    return base.QueryInterface(_iid, obj);
        //}

        public override bool Setup()
        {
            onceExecuted = false;
            if (!base.Setup()) return false;

            if (controller == null) return false;
            if (numParams < 1 || numParams > controller.GetParameterCount())
                numParams = controller.GetParameterCount();
            if (audioEffect != null && numParams > 0)
            {
                ParameterInfo inf = new();
                for (var i = 0; i < numParams; ++i)
                {
                    paramChanges.Add(new ParamChanges());
                    var r = controller.GetParameterInfo(i, ref inf);
                    if (r != TResult.S_True) return false;
                    if ((inf.Flags & ParameterInfo.ParameterFlags.CanAutomate) != 0) paramChanges[i].Init(inf.Id, processSetup.MaxSamplesPerBlock);
                }

                for (var i = 0; i < controller.GetParameterCount(); ++i)
                {
                    var r = controller.GetParameterInfo(i, ref inf);
                    if (r != TResult.S_True) return false;
                    if ((inf.Flags & ParameterInfo.ParameterFlags.IsBypass) != 0)
                    {
                        bypassId = inf.Id;
                        break;
                    }
                }
                return true;
            }
            return numParams == 0;
        }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null) return false;

            PrintTestHeader(testResult);

            if (numParams == 0) testResult.AddMessage("No Parameters present.");
            var ret = base.Run(testResult);
            return ret;
        }

        public override bool Teardown()
        {
            paramChanges.Clear();
            return base.Teardown();
        }

        protected override bool PreProcess(ITestResult testResult)
        {
            if (testResult == null) return false;
            if (!paramChanges.Any()) return numParams == 0;

            var rand = new Random((int)DateTime.Now.Ticks);
            var check = true;
            for (var i = 0; i < numParams; ++i)
            {
                paramChanges[i].ResetPoints();
                var point = 0;
                for (var pos = 0; pos < processData._.NumSamples; pos++)
                {
                    var add = (rand.Next() % everyNSamples) == 0;
                    if (!onceExecuted)
                        if (pos == 0)
                        {
                            add = true;
                            if (!sampleAccuracy) onceExecuted = true;
                        }
                        else if (pos == 1 && sampleAccuracy)
                        {
                            add = true;
                            onceExecuted = true;
                        }
                    if (add) check &= paramChanges[i].SetPoint(point++, pos, ((float)(rand.Next() % 1000000000)) / 1000000000.0);
                }
                if (check) processData._.InputParameterChanges = this.CastT();
            }
            return check;
        }

        protected override bool PostProcess(ITestResult testResult)
        {
            if (testResult == null) return false;
            if (!paramChanges.Any()) return numParams == 0;

            for (var i = 0; i < numParams; ++i)
                if ((paramChanges[i].GetPointCount() > 0) && !paramChanges[i].HavePointsBeenRead(!sampleAccuracy))
                {
                    testResult.AddMessage(sampleAccuracy
                        ? "   Not all points have been read via IParameterChanges"
                        : "   No point at all has been read via IParameterChanges");
                    return true; // should not be a problem
                }
            return true;
        }

        public int GetParameterCount()
            => !paramChanges.Any() ? numParams : paramChanges.Count;

        public IntPtr GetParameterData(int index) //: IParamValueQueue
            => index >= 0 && index < GetParameterCount() ? paramChanges[index].CastT() : IntPtr.Zero;

        public IntPtr AddParameterData(uint id, ref int index) //: IParamValueQueue
            => IntPtr.Zero;
    }

    // Test Parameters Flush (no Buffer).
    public class FlushParamTest : AutomationTest
    {
        public override string Name => "Process Test";

        public FlushParamTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl)
            : base(plugProvider, sampl, 100, 1, false) { }

        protected virtual void PrepareProcessData()
        {
            processData._.NumSamples = 0;
            processData._.NumInputs = 0;
            processData._.NumOutputs = 0;
            processData._.Inputs = IntPtr.Zero;
            processData._.Outputs = IntPtr.Zero;
        }

        public override bool Run(ITestResult testResult)
        {
            if (vstPlug == null || testResult == null || audioEffect == null) return false;

            if (!CanProcessSampleSize(testResult)) return true;

            PrintTestHeader(testResult);

            UnprepareProcessing();

            PrepareProcessData();

            audioEffect.SetProcessing(true);

            PreProcess(testResult);

            var result = audioEffect.Process(ref processData._);
            if (result != TResult.S_OK)
            {
                testResult.AddErrorMessage("The component failed to process without audio buffers!");

                audioEffect.SetProcessing(false);
                return false;
            }

            PostProcess(testResult);

            audioEffect.SetProcessing(false);
            return true;
        }
    }

    // Test Parameters Flush 2 (no Buffer).
    public unsafe class FlushParamTest2 : FlushParamTest
    {
        protected int numInputs;
        protected int numOutputs;
        protected int numChannelsIn;
        protected int numChannelsOut;

        public override string Name => "Parameters Flush 2 (only numChannel==0)";

        public FlushParamTest2(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl) { }

        protected override void PrepareProcessData()
        {
            PrepareProcessing();
            processData._.NumSamples = 0;

            // remember original processData config
            Platform.Swap<int>(ref numInputs, ref processData._.NumInputs);
            Platform.Swap<int>(ref numOutputs, ref processData._.NumOutputs);
            if (processData._.Inputs != IntPtr.Zero) Platform.Swap<int>(ref numChannelsIn, ref ((AudioBusBuffers*)processData._.Inputs)[0].NumChannels);
            if (processData._.Outputs != IntPtr.Zero) Platform.Swap<int>(ref numChannelsOut, ref ((AudioBusBuffers*)processData._.Outputs)[0].NumChannels);
        }

        public override bool Teardown()
        {
            // restore original processData config for correct deallocation
            Platform.Swap<int>(ref numInputs, ref processData._.NumInputs);
            Platform.Swap<int>(ref numOutputs, ref processData._.NumOutputs);
            if (processData._.Inputs != IntPtr.Zero) Platform.Swap<int>(ref numChannelsIn, ref ((AudioBusBuffers*)processData._.Inputs)[0].NumChannels);
            if (processData._.Outputs != IntPtr.Zero) Platform.Swap<int>(ref numChannelsOut, ref ((AudioBusBuffers*)processData._.Outputs)[0].NumChannels);

            return base.Teardown();
        }
    }

    // Test Parameters Flush 3 (no Buffer, no parameter change).
    public class FlushParamTest3 : FlushParamTest2
    {
        public override string Name => "Parameters Flush 2 (no Buffer, no parameter change)";

        public FlushParamTest3(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider, sampl)
            => paramChanges.Clear();
    }
}
