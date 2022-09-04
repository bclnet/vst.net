//using Jacobi.Vst3;
//using Jacobi.Vst3.Hosting;
//using Jacobi.Vst3.Plugin;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Xml.Linq;
//using static Jacobi.Vst3.TResult;
//using ParamID = System.UInt32;
//using UnitID = System.Int32;
//using ProgramListID = System.Int32;
//using System.Threading;

//namespace Jacobi.Vst3.Plugin
//{
//    partial class Constants2
//    {
//        const int kMaxEvents = 2048;
//    }

//    public abstract class BaseEditorWrapper : IPlugFrame
//    {
//        protected IEditController mController;
//        protected IPlugView mView;
//        protected ViewRect mViewRect;

//        public BaseEditorWrapper(IEditController controller)
//        {
//        }

//        public static bool HasEditor(IEditController controller) => throw new NotImplementedException();

//        public bool GetRect(ViewRect rect) => throw new NotImplementedException();
//        public virtual bool Open(IntPtr ptr) => throw new NotImplementedException();
//        public virtual void Close() => throw new NotImplementedException();

//        public bool SetKnobMode(KnobMode val) => throw new NotImplementedException();

//        // IPlugFrame
//        public TResult ResizeView(IPlugView view, ref ViewRect newSize) => throw new NotImplementedException();

//        void CreateView() => throw new NotImplementedException();

//    }

//    public abstract class BaseWrapper : IHostApplication, IComponentHandler, IUnitHandler
//    {
//        const int kMaxProgramChangeParameters = 16;
//        ParamID[] mProgramChangeParameterIDs = new ParamID[kMaxProgramChangeParameters]; // for each MIDI channel
//        int[] mProgramChangeParameterIdxs = new int[kMaxProgramChangeParameters]; // for each MIDI channel

//        Guid mVst3EffectClassID;

//        // vst3 data
//        IAudioProcessor mProcessor;
//        IComponent mComponent;
//        IEditController mController;
//        IUnitInfo mUnitInfo;
//        IMidiMapping mMidiMapping;

//        BaseEditorWrapper mEditor;

//        PlugInterfaceSupport mPlugInterfaceSupport;

//        ConnectionProxy mProcessorConnection;
//        ConnectionProxy mControllerConnection;

//        SymbolicSampleSizes mVst3SampleSize = SymbolicSampleSizes.Sample32;
//        ProcessModes mVst3processMode = ProcessModes.Realtime;

//        string mName;
//        string mVendor;
//        string mSubCategories;
//        int mVersion = 0;

//        struct ParamMapEntry
//        {
//            ParamID vst3ID;
//            int vst3Index;
//        };

//        List<ParamMapEntry> mParameterMap;
//        Dictionary<ParamID, int> mParamIndexMap;
//        ParamID mBypassParameterID = Constants.NoParamId;
//        ParamID mProgramParameterID = Constants.NoParamId;
//        int mProgramParameterIdx = -1;

//        HostProcessData mProcessData;
//        ProcessContext mProcessContext;
//        ParameterChanges mInputChanges;
//        ParameterChanges mOutputChanges;
//        EventList mInputEvents;
//        EventList mOutputEvents;

//        ulong mMainAudioInputBuses = 0UL;
//        ulong mMainAudioOutputBuses = 0UL;

//        ParameterChangeTransfer mInputTransfer;
//        ParameterChangeTransfer mOutputTransfer;
//        ParameterChangeTransfer mGuiTransfer;

//        MemoryStream mChunk;

//        Timer mTimer;
//        IPluginFactory mFactory;

//        int mNumPrograms = 0;
//        float mSampleRate = 44100;
//        int mBlockSize = 256;
//        int mNumParams = 0;
//        int mCurProgram = -1;
//        uint mNumInputs = 0;
//        uint mNumOutputs = 0;

//        const int kMaxMidiMappingBusses = 4;
//        ParamID[,] mMidiCCMapping = new ParamID[kMaxMidiMappingBusses, 16];

//        bool mComponentInitialized = false;
//        bool mControllerInitialized = false;
//        bool mComponentsConnected = false;
//        bool mUseExportedBypass = true;

//        bool mActive = false;
//        bool mProcessing = false;
//        bool mHasEventInputBuses = false;
//        bool mHasEventOutputBuses = false;

//        bool mUseIncIndex = true;

//        public struct SVST3Config
//        {
//            public IPluginFactory factory;
//            public IAudioProcessor processor;
//            public IEditController controller;
//            public Guid vst3ComponentID;
//        }

//        public BaseWrapper(SVST3Config config)
//        {

//        }

//        public virtual bool Init() => throw new NotImplementedException();

//        public virtual void CanDoubleReplacing(bool val) { }
//        public virtual void SetInitialDelay(uint delay) { }
//        public virtual void NoTail(bool val) { }

//        public virtual void IoChanged() { }
//        public virtual void UpdateDisplay() { }
//        public virtual void SetNumInputs(uint inputs) => mNumInputs = inputs;
//        public virtual void SetNumOutputs(uint outputs) => mNumOutputs = outputs;
//        public abstract bool SizeWindow(int width, int height);
//        public virtual int GetChunk(out byte[] data, bool isPreset) => throw new NotImplementedException();
//        public virtual int SetChunk(byte[] data, int byteSize, bool isPreset) => throw new NotImplementedException();

//        public virtual bool GetEditorSize(out int width, out int height) => throw new NotImplementedException();

//        public bool IsActive() => mActive;
//        public uint GetNumInputs() => mNumInputs;
//        public uint GetNumOutputs() => mNumOutputs;

//        public BaseEditorWrapper GetEditor() => mEditor;


//        #region VST 3 Interfaces

//        // IHostApplication
//        public TResult CreateInstance(Guid cid, Guid iid, object obj) => throw new NotImplementedException();

//        // IComponentHandler
//        public TResult RestartComponent(int flags) => throw new NotImplementedException();

//        // IUnitHandler
//        public TResult notifyUnitSelection(UnitID unitId) => throw new NotImplementedException();
//        public TResult notifyProgramListChange(ProgramListID listId, int programIndex) => throw new NotImplementedException();

//        // ITimer
//        public void OnTimer(Timer timer) => throw new NotImplementedException();

//        #endregion
//    }

//    partial class Constants2
//    {
//        const byte kNoteOff = 0x80; ///< note, off velocity
//        const byte kNoteOn = 0x90; ///< note, on velocity
//        const byte kPolyPressure = 0xA0; ///< note, pressure
//        const byte kController = 0xB0; ///< controller, value
//        const byte kProgramChangeStatus = 0xC0; ///< program change
//        const byte kAfterTouchStatus = 0xD0; ///< channel pressure
//        const byte kPitchBendStatus = 0xE0; ///< lsb, msb

//        const float kMidiScaler = 1f / 127f;
//        const byte kChannelMask = 0x0F;
//        const byte kStatusMask = 0xF0;
//        const uint kDataMask = 0x7F;
//    }
//}
