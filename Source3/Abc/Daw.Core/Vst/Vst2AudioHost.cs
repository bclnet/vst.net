using Jacobi.Vst.Core;
using Jacobi.Vst.Host.Interop;
using System;
using System.IO;

namespace Daw.Vst
{
    public unsafe class Vst2AudioHost : Vst2HostCommandBase
    {
        public ClassInfo2 Load(FileInfo file)
        {
            try
            {
                using var ctx = VstPluginContext.Create(file.FullName, this);
                ctx.Set("PluginPath", file.FullName);
                ctx.Set("HostCmdStub", this);
                var info = new ClassInfo2
                {
                    Name = file.Name.Replace(".dll", string.Empty, StringComparison.OrdinalIgnoreCase),
                    FriendlyName = ctx.PluginCommandStub.Commands.GetEffectName(),
                    Path = file.FullName,
                    AudioInputs = ctx.PluginInfo.AudioInputCount,
                    AudioOutputs = ctx.PluginInfo.AudioOutputCount,
                    CanReceiveVstMidiEvent = ctx.PluginCommandStub.Commands.CanDo(VstCanDoHelper.ToString(VstPluginCanDo.ReceiveVstMidiEvent)) == VstCanDoResult.Yes,
                };
                if ((ctx.PluginInfo.Flags & VstPluginFlags.ProgramChunks) != VstPluginFlags.ProgramChunks) Console.WriteLine($"{info.Name} does not support chunks");
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}