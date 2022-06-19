using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;
using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Test
{
    public class TestingPluginContext
    {
        public static readonly TestingPluginContext Instance = new();
        public static object Get() => Instance.context;
        public static void Set(object context) => Instance.context = context;
        public object context = null;
    }

    class TestDefaults
    {
        public static readonly TestDefaults Instance = new();
        public int NumIterations = 20;
        public int DefaultSampleRate = 44100;
        public int DefaultBlockSize = 64;
        public int MaxBlockSize = 8192;
        //public int BuffersAreEqual = 0;
        public int NumAudioBlocksToProcess = 3;
        public ulong ChannelIsSilent = 1;
    }

    public abstract class TestBase : ITest
    {
        protected ITestPlugProvider plugProvider;
        protected IComponent vstPlug;
        protected IEditController controller;

        TestBase() { }
        public TestBase(ITestPlugProvider plugProvider) => this.plugProvider = plugProvider;

        public abstract string Name { get; }

        public virtual bool Setup()
        {
            if (plugProvider != null)
            {
                vstPlug = plugProvider.GetComponent();
                controller = plugProvider.GetController();
                return ActivateMainIOBusses(true);
            }
            return false;
        }

        public abstract bool Run(ITestResult testResult);

        public virtual bool Teardown()
        {
            if (vstPlug != null)
            {
                ActivateMainIOBusses(false);
                plugProvider.ReleasePlugIn(vstPlug, controller);
            }
            return true;
        }

        public virtual bool ActivateMainIOBusses(bool val)
        {
            if (vstPlug != null)
            {
                if (vstPlug.GetBusCount(MediaTypes.Audio, BusDirections.Input) > 0) vstPlug.ActivateBus(MediaTypes.Audio, BusDirections.Input, 0, val);
                if (vstPlug.GetBusCount(MediaTypes.Audio, BusDirections.Output) > 0) vstPlug.ActivateBus(MediaTypes.Audio, BusDirections.Output, 0, val);
                return true;
            }
            return false;
        }

        public virtual void PrintTestHeader(ITestResult testResult) => testResult.AddMessage($"==={Name} ====================================");

        public string GetDescription() => string.Empty;
    }

    public abstract class TestEnh : TestBase
    {
        enum AudioDefaults
        {
            kBlockSize = 64,
            kMaxSamplesPerBlock = 8192,
            kSampleRate = 44100,
        }

        protected IAudioProcessor audioEffect;
        protected ProcessSetup processSetup;

        public TestEnh(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl) : base(plugProvider)
        {
            processSetup.ProcessMode = ProcessModes.Realtime;
            processSetup.SymbolicSampleSize = sampl;
            processSetup.MaxSamplesPerBlock = (int)AudioDefaults.kMaxSamplesPerBlock;
            processSetup.SampleRate = (double)AudioDefaults.kSampleRate;
        }

        public override bool Setup()
        {
            var res = base.Setup();
            if (vstPlug != null)
            {
                audioEffect = vstPlug as IAudioProcessor;
                //var check = vstPlug.QueryInterface(new Guid(Interfaces.IAudioProcessor), out audioEffect);
                //if (check != TResult.S_True)
                //    return false;
            }
            return res && audioEffect != null;
        }

        public override bool Teardown()
        {
            if (audioEffect != null) Marshal.ReleaseComObject(audioEffect);
            var res = base.Teardown();
            return res && audioEffect != null;
        }
    }

    public class ParamPoint
    {
        int offsetSamples = -1;
        double value;
        bool read;

        public void Set(int offsetSamples, double value)
        {
            this.offsetSamples = offsetSamples;
            this.value = value;
        }

        public void Get(out int offsetSamples, out double value)
        {
            offsetSamples = this.offsetSamples;
            value = this.value;
            read = true;
        }

        public bool WasRead() => read;
    }

    public class ParamChanges : IParamValueQueue
    {
        uint id = ParameterInfo.NoParamId;
        int numPoints;
        int numUsedPoints;
        int processedFrames;
        ParamPoint[] points;

        public void Init(uint id, int numPoints)
        {
            this.id = id;
            this.numPoints = numPoints;
            numUsedPoints = 0;
            points = new ParamPoint[this.numPoints];
            processedFrames = 0;
        }

        public bool SetPoint(int index, int offsetSamples, double value)
        {
            if (points != null && index >= 0 && index == numUsedPoints && index < numPoints)
            {
                points[index].Set(offsetSamples, value);
                numUsedPoints++;
                return true;
            }
            if (points == null) return true;
            return false;
        }

        public void ResetPoints()
        {
            numUsedPoints = 0;
            processedFrames = 0;
        }

        public int GetProcessedFrames() => processedFrames;
        public void SetProcessedFrames(int amount) => processedFrames = amount;
        public bool HavePointsBeenRead(bool atAll)
        {
            for (var i = 0; i < GetPointCount(); ++i)
                if (points[i].WasRead())
                {
                    if (atAll) return true;
                }
                else if (!atAll) return false;
            return !atAll;
        }

        //---for IParamValueQueue-------------------------
        public uint GetParameterId() => id;
        public int GetPointCount() => numUsedPoints;
        public int GetPoint(int index, ref int offsetSamples, ref double value)
        {
            if (points != null && index < numUsedPoints && index >= 0)
            {
                points[index].Get(out offsetSamples, out value);
                return TResult.S_True;
            }
            return TResult.S_False;
        }
        public int AddPoint(int offsetSamples, double value, ref int index) => TResult.S_False;
    }

    public class StringResult : IStringResult
    {
        string data;
        //uint __refCount;

        public string Get() => data;
        public void SetText(string text) => data = text;

        public int QueryInterface(Guid _iid, IntPtr obj)
        {
            //QUERY_INTERFACE (_iid, obj, FUnknown::iid, IStringResult)
            //QUERY_INTERFACE (_iid, obj, IStringResult::iid, IStringResult)
            obj = IntPtr.Zero;
            return TResult.E_NoInterface;
        }

        //uint addRef() { return ++__refCount; }

        //uint release()
        //{
        //    if (--__refCount == 0) return 0;
        //    return __refCount;
        //}
    }
}
