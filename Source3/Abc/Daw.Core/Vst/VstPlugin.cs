using Daw.Core;
using Jacobi.Vst.Core;
using Jacobi.Vst.Host.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Daw.Vst
{
    public enum PluginState
    {
        Empty, // (= Unloaded)
        Unloading,
        Deactivated,
        Activated
    }

    [Serializable]
    public class VstPreset
    {
        public PluginState State;
        public string Name;
        public byte[] Data; // Chunk or Parameters data
    }

    public enum MidiChannel
    {
        ChannelAll = 0,
        Channel1 = 1,
        Channel2 = 2,
        Channel3 = 3,
        Channel4 = 4,
        Channel5 = 5,
        Channel6 = 6,
        Channel7 = 7,
        Channel8 = 8,
        Channel9 = 9,
        Channel10 = 10,
        Channel11 = 11,
        Channel12 = 12,
        Channel13 = 13,
        Channel14 = 14,
        Channel15 = 15,
        Channel16 = 16,
    }

    public enum ExpressionPedalFunction
    {
        EffectControl,
        VolumeControl,
        None,
    }

    public class PluginStateChangeEventArgs : EventArgs
    {
        public PluginState PluginState { get; private set; }
        public PluginStateChangeEventArgs(PluginState pluginState) => PluginState = pluginState;
    }

    public class VstPlugin
    {
        AudioBufferInfo _parentInBuffers;
        AudioBufferInfo _parentOutBuffers;
        int _asioBuffSize;
        PluginState _state;
        VstPluginContext _vstPluginContext;
        ManualResetEventSlim _unloadingCompleteEvent = new(false);
        bool _editorIsOpen = false;
        bool _dbgIsEffect = false;

        public event EventHandler<PluginStateChangeEventArgs> StateChanged;
        public event EventHandler OnEditorOpening;
        public event EventHandler OnEditorOpened;
        public event EventHandler OnEditorClosed;

        public VstPlugin(AudioBufferInfo parentInBuffers, AudioBufferInfo parentOutBuffers, int asioBuffSize, bool dbg)
        {
            _dbgIsEffect = dbg;
            _parentInBuffers = parentInBuffers;
            _parentOutBuffers = parentOutBuffers;
            _asioBuffSize = asioBuffSize;
        }
        ~VstPlugin()
        {
            if (InputBuffers != null) InputBuffers = null;
            if (OutputBuffers != null) OutputBuffers = null;
        }

        public AudioBufferInfo OutputBuffers { get; set; }
        public AudioBufferInfo InputBuffers { get; set; }
        public string PluginName { get; private set; }
        public string ProgramName { get; private set; }

        public PluginState State
        {
            get => _state;
            private set { _state = value; StateChanged(this, new PluginStateChangeEventArgs(value)); }
        }

        public MidiChannel MidiChannel { get; set; }
        public int EditorOpenedTimer { get; set; }
        public bool UseExtendedEffectRange { get; private set; }

        public Panel EditorPanel { get; set; }

        public VstPreset VstPreset
        {
            get
            {
                var preset = new VstPreset { Name = PluginName, State = State };
                if (preset.State != PluginState.Empty) preset.Data = GetData();
                return preset;
            }
            set
            {
                if (value.State != PluginState.Empty) { if (value.Data != null) SetData(value.Data); ProgramName = _vstPluginContext.PluginCommandStub.Commands.GetProgramName(); }
                else ProgramName = string.Empty;
                if (value.State == PluginState.Activated) Activate();
            }
        }

        public VstPluginContext VstPluginContext => _vstPluginContext;

        public void AttachVstPluginContext(VstPluginContext ctx, string name)
        {
            _vstPluginContext = ctx;
            var commands = ctx.PluginCommandStub.Commands;
            // Create big enough buffers to fit all plugin types
            var inSize = 32; // value.PluginInfo.AudioInputCount;
            var outSize = 64; // value.PluginInfo.AudioOutputCount;
            if (_asioBuffSize != 0)
            {
                if (InputBuffers == null || inSize != InputBuffers.Count)
                {
                    InputBuffers = null; // Dispose first if already existed!
                    InputBuffers = new AudioBufferInfo(inSize, _parentInBuffers);
                }
                if (OutputBuffers == null || outSize != OutputBuffers.Count)
                {
                    OutputBuffers = null; // Dispose first if already existed!
                    OutputBuffers = new AudioBufferInfo(outSize, _parentOutBuffers);
                }
            }
            commands.MainsChanged(true);
            commands.StartProcess();
            State = PluginState.Deactivated;
            PluginName = name;
            ProgramName = commands.GetProgramName();
            UseExtendedEffectRange = PluginName == "Nexus";
        }

        public void DetachVstPluginContext()
        {
            PluginName = string.Empty;
            ProgramName = string.Empty;
            State = PluginState.Empty;
            var commands = _vstPluginContext.PluginCommandStub.Commands;
            commands.StopProcess();
            commands.MainsChanged(false);
            _vstPluginContext = null;
        }

        public virtual void Deactivate()
        {
            if (VstPluginContext == null) throw new Exception("shouldnt be here");
            if (State != PluginState.Activated) throw new Exception("Wrong state change for deactivate!");
            State = PluginState.Deactivated;
        }

        public void FinishUnloading()
        {
            _unloadingCompleteEvent.Set();
            State = PluginState.Empty;
            PluginName = string.Empty;
            ProgramName = string.Empty;
        }

        public void Unload()
        {
            if (State == PluginState.Unloading || State == PluginState.Empty) throw new Exception("Shouldn't be here!");
            if (State == PluginState.Activated) Deactivate();
            if (State == PluginState.Deactivated)
            {
                // Start unload
                CloseEditor();
                _unloadingCompleteEvent.Reset();
                State = PluginState.Unloading;
                // Wait for asio callback to handle plugin deactivation to prevent a lock!
                if (!_unloadingCompleteEvent.Wait(100)) { } // Asio driver not firing events and setting mUnloadingCompleteEvent: ignore
                var commands = _vstPluginContext.PluginCommandStub.Commands;
                commands.StopProcess();
                commands.MainsChanged(false);
                commands.Close();
                _vstPluginContext = null;
            }
        }

        public void Activate()
        {
            if (State != PluginState.Deactivated) throw new Exception("Wrong state change for activate!");
            State = PluginState.Activated;
        }

        public void ShowEditor()
        {
            if (VstPluginContext != null && /* EditorPanel.IsHandleCreated && */ EditorPanel.Handle != IntPtr.Zero && !_editorIsOpen)
            {
                OnEditorOpening?.Invoke(this, null); // Force editor close on other plugins
                var commands = VstPluginContext.PluginCommandStub.Commands;
                commands.EditorOpen(EditorPanel.Handle);
                commands.EditorGetRect(out var rect);
                EditorPanel.Size = new Size(rect.Width, rect.Height);
                EditorOpenedTimer = 0;
                _editorIsOpen = true;
            }
            if (_editorIsOpen) OnEditorOpened?.Invoke(this, null);
        }

        public void CloseEditor()
        {
            if (VstPluginContext != null && _editorIsOpen)
            {
                var commands = VstPluginContext.PluginCommandStub.Commands;
                commands.EditorIdle();
                commands.EditorClose();
                _editorIsOpen = false;
                OnEditorClosed?.Invoke(this, null); // Let other controls know this control wil load plugin UI editor
            }
        }

        byte[] GetData()
        {
            var commands = _vstPluginContext.PluginCommandStub.Commands;
            if ((_vstPluginContext.PluginInfo.Flags & VstPluginFlags.ProgramChunks) == VstPluginFlags.ProgramChunks) { ProgramName = commands.GetProgramName(); return commands.GetChunk(false); }
            else { ProgramName = commands.GetProgramName(); return GetParameterData(); }
        }

        void SetData(byte[] data)
        {
            var commands = _vstPluginContext.PluginCommandStub.Commands;
            if ((_vstPluginContext.PluginInfo.Flags & VstPluginFlags.ProgramChunks) == VstPluginFlags.ProgramChunks) commands.SetChunk(data, false);
            else SetParameters(data);
        }

        void SetParameters(byte[] data)
        {
            var commands = _vstPluginContext.PluginCommandStub.Commands;
            try
            {
                var parameterSet = VstParameterSet.FromByteArray(data);
                commands.SetProgram(parameterSet.ProgramNumber);
                foreach (var parameter in parameterSet.Parameters) commands.SetParameter(parameter.Index, parameter.Value);
            }
            catch (Exception ex) { Console.WriteLine($"Cannot import VST parameters: {ex.Message}"); }
        }

        [Serializable]
        class VstParameterSet
        {
            public int ProgramNumber;
            public List<VstParameter> Parameters = new();

            public byte[] ToByteArray()
            {
                byte[] data;
                using (var s = new MemoryStream())
                    try
                    {
                        new BinaryFormatter().Serialize(s, this);
                        data = s.ToArray();
                    }
                    catch (SerializationException ex) { Console.WriteLine($"Failed to serialize VstParameterSet. Reason: {ex.Message}"); throw; }
                return data;
            }

            public static VstParameterSet FromByteArray(byte[] data)
            {
                VstParameterSet parameterSet;
                using (var s = new MemoryStream(data))
                    try
                    {
                        parameterSet = (VstParameterSet)new BinaryFormatter().Deserialize(s);
                    }
                    catch (SerializationException ex) { Console.WriteLine($"Failed to deserialize VstParameterSet. Reason: {ex.Message}"); throw; }
                return parameterSet;
            }
        }

        [Serializable]
        class VstParameter
        {
            public int Index;
            public float Value;
        }

        byte[] GetParameterData()
        {
            const int MAX_PARAMETERS = 1000;
            var parameterSet = new VstParameterSet();
            var emptyCount = 0;
            var parameterCount = 0;
            var commands = VstPluginContext.PluginCommandStub.Commands;
            try
            {
                parameterSet.ProgramNumber = commands.GetProgram();
                for (var i = 0; i < MAX_PARAMETERS; i++)
                {
                    var parameterValue = commands.GetParameter(i);
                    var display = commands.GetParameterDisplay(i);
                    var label = commands.GetParameterLabel(i);
                    var name = commands.GetParameterName(i);
                    //if (parameterValue == 0.0f && (display == "0.000000" || display == "?") && label == string.Empty)
                    if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(label))
                    {
                        var p = new VstParameter { Index = i, Value = parameterValue };
                        emptyCount = 0;
                        parameterSet.Parameters.Add(p);
                        parameterCount++;
                        if (parameterCount == MAX_PARAMETERS) { Console.WriteLine($"More than {MAX_PARAMETERS} parameters in VST!"); break; }
                    }
                    // 100 x nothing?
                    else if (emptyCount++ == 100) break; // Assume all parameters are read; break 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot export VST parameters: {ex.Message}");
            }
            return parameterSet.ToByteArray();
        }
    }
}
