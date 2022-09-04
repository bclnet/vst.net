using Steinberg.Vst3.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3.TestSuite
{
    /// <summary>
    /// PlugCompat
    /// </summary>
    public class PlugCompat
    {
        unsafe class StringStream : IBStream
        {
            MemoryStream str = new();

            public TResult Read(IntPtr buffer, int numBytes, out int numBytesRead)
            {
                numBytesRead = default;
                return kNotImplemented;
            }

            [return: MarshalAs(UnmanagedType.Error)]
            public TResult Write(IntPtr buffer, int numBytes, out int numBytesWritten)
            {
                str.Write(new ReadOnlySpan<byte>((void*)buffer, numBytes));
                numBytesWritten = numBytes;
                return kResultTrue;
            }

            public TResult Seek(long pos, SeekOrigin mode, out long result)
            {
                result = default;
                return kNotImplemented;
            }

            public TResult Tell(out long pos)
            {
                pos = default;
                return kNotImplemented;
            }

            public override string ToString()
            {
                str.Position = 0;
                return Encoding.ASCII.GetString(str.ToArray());
            }
        }

        public static bool CheckPluginCompatibility(Module module, IPluginCompatibility compat, TextWriter errorStream)
        {
            var failure = false;
            var moduleInfoPath = Module.GetModuleInfoPath(module.Path);
            if (moduleInfoPath != null)
            {
                failure = true;
                errorStream.Write("Error: The module contains a moduleinfo.json file and the module factory exports a IPluginCompatibility class. Only one is allowed, while the moduleinfo.json one is prefered.\n");
            }

            StringStream strStream = new();
            if (compat.GetCompatibilityJSON(strStream) != kResultTrue)
            {
                errorStream.Write("Error: Call to IPluginCompatiblity::getCompatibilityJSON (IBStream*) failed\n");
                failure = true;
            }
            // TODO: Check that the "New" classes are part of the Module;
            else if (ModuleInfoLib.ParseCompatibilityJson(strStream.ToString(), errorStream) != null) { }
            else failure = true;
            return !failure;
        }
    }
}
