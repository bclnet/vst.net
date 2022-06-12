using Jacobi.Vst.Core;
using Jacobi.Vst.Core.Host;
using System;

namespace Daw.Vst
{
    internal class PluginCalledEventArgs : EventArgs
    {
        public PluginCalledEventArgs(string message) => Message = message;
        public string Message { get; private set; }
    }

    /// <summary>
    /// The VstHostCommandBase class represents the part of the host that a plugin can call.
    /// </summary>
    public class Vst2HostCommandBase : IVstHostCommandStub
    {
        public Vst2HostCommandBase() => Commands = new VstHostCommands20(this);
        public IVstPluginContext PluginContext { get; set; }
        public IVstHostCommands20 Commands { get; }
    }

    public class VstHostCommands20 : IVstHostCommands20
    {
        IVstHostCommandStub parent;

        public VstHostCommands20(IVstHostCommandStub parent) => this.parent = parent;

        void RaisePluginCalled(string message) { }

        // IVstHostCommands20

        public bool BeginEdit(int index) { RaisePluginCalled($"BeginEdit({index})"); return false; }

        public VstCanDoResult CanDo(string cando)
        {
            switch (cando)
            {
                case "sendVstTimeInfo": return VstCanDoResult.Yes;
                case "sizeWindow": return VstCanDoResult.No;
            }
            RaisePluginCalled($"CanDo({cando})");
            return VstCanDoResult.Unknown;
        }

        public bool CloseFileSelector(VstFileSelect fileSelect) { RaisePluginCalled($"CloseFileSelector({fileSelect.Command})"); return false; }

        public bool EndEdit(int index) { RaisePluginCalled($"EndEdit({index})"); return false; }

        public VstAutomationStates GetAutomationState() { RaisePluginCalled("GetAutomationState()"); return VstAutomationStates.Off; }

        public int GetBlockSize() { RaisePluginCalled("GetBlockSize()"); return 1024; }

        public string GetDirectory() { RaisePluginCalled("GetDirectory()"); return null; }

        public int GetInputLatency() { RaisePluginCalled("GetInputLatency()"); return 0; }

        public VstHostLanguage GetLanguage() { RaisePluginCalled("GetLanguage()"); return VstHostLanguage.NotSupported; }

        public int GetOutputLatency() { RaisePluginCalled("GetOutputLatency()"); return 0; }

        public VstProcessLevels GetProcessLevel() { RaisePluginCalled("GetProcessLevel()"); return VstProcessLevels.Unknown; }

        public string GetProductString() { RaisePluginCalled("GetProductString()"); return "VST.NET"; }

        public float GetSampleRate() { RaisePluginCalled("GetSampleRate()"); return 44.1f; }

        protected VstTimeInfo VstTimeInfo = new();
        protected virtual int BPM { get; set; } = 120;
        public VstTimeInfo GetTimeInfo(VstTimeInfoFlags filterFlags)
        {
            VstTimeInfo.SampleRate = 44100.0;
            VstTimeInfo.Tempo = (double)BPM;
            VstTimeInfo.PpqPosition = (VstTimeInfo.SamplePosition / VstTimeInfo.SampleRate) * (VstTimeInfo.Tempo / 60.0);
            VstTimeInfo.NanoSeconds = 0.0;
            VstTimeInfo.BarStartPosition = 0.0;
            VstTimeInfo.CycleStartPosition = 0.0;
            VstTimeInfo.CycleEndPosition = 0.0;
            VstTimeInfo.TimeSignatureNumerator = 4;
            VstTimeInfo.TimeSignatureDenominator = 4;
            VstTimeInfo.SmpteOffset = 0;
            VstTimeInfo.SmpteFrameRate = new VstSmpteFrameRate();
            VstTimeInfo.SamplesToNearestClock = 0;
            VstTimeInfo.Flags = VstTimeInfoFlags.TempoValid | VstTimeInfoFlags.PpqPositionValid | VstTimeInfoFlags.TransportPlaying;
            return VstTimeInfo;
        }

        public string GetVendorString() { RaisePluginCalled("GetVendorString()"); return "Contoso"; }

        public int GetVendorVersion() { RaisePluginCalled("GetVendorVersion()"); return 1000; }

        public bool IoChanged() { RaisePluginCalled("IoChanged()"); return false; }

        public bool OpenFileSelector(VstFileSelect fileSelect) { RaisePluginCalled($"OpenFileSelector({fileSelect.Command})"); return false; }

        public bool ProcessEvents(VstEvent[] events) { RaisePluginCalled($"ProcessEvents({events.Length})"); return false; }

        public bool SizeWindow(int width, int height) { RaisePluginCalled($"SizeWindow({width}, {height})"); return false; }

        public bool UpdateDisplay() { RaisePluginCalled("UpdateDisplay()"); return false; }

        // IVstHostCommands10

        public int GetCurrentPluginID() { RaisePluginCalled("GetCurrentPluginID()"); return parent.PluginContext.PluginInfo.PluginID; }

        public int GetVersion() { RaisePluginCalled("GetVersion()"); return 1000; }

        public void ProcessIdle() => RaisePluginCalled("ProcessIdle()");

        public void SetParameterAutomated(int index, float value) => RaisePluginCalled($"SetParameterAutomated({index}, {value})");
    }
}
