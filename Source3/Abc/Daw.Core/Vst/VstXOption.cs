using Jacobi.Vst3.Host;
using System.Collections.Generic;

namespace Daw.Vst
{
    public class Vst2Option
    {
        public List<string> Folders;
        public List<string> Dlls;
        public List<ClassInfo2> Classes;

        public static Vst2Option Default => new()
        {
            Folders = new List<string> { @"C:\Program Files\Native Instruments\VSTPlugins 64 bit" },
            Dlls = new List<string>(),
            Classes = new List<ClassInfo2>(),
        };
    }

    public class Vst3Option
    {
        public List<string> Vst3s;
        public List<ClassInfo> Classes;

        public static Vst3Option Default => new()
        {
            Vst3s = new List<string>(),
            Classes = new List<ClassInfo>(),
        };
    }
}
