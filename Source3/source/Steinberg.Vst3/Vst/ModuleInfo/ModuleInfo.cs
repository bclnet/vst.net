using System.Collections.Generic;

namespace Steinberg.Vst3
{
    public class ModuleInfo
    {
        public struct FactoryInfoX
        {
            public string Vendor;
            public string Url;
            public string Email;
            public PFactoryInfo.FactoryFlags Flags;
        }

        public struct Snapshot
        {
            public double ScaleFactor;
            public string Path;
        }

        public struct ClassInfo
        {
            public string Cid;
            public string Category;
            public string Name;
            public string Vendor;
            public string Version;
            public string SdkVersion;
            public List<string> SubCategories;
            public List<Snapshot> Snapshots;
            public PClassInfo.ClassCardinality Cardinality;
            public uint Flags;
        }

        public struct CompatibilityX
        {
            public string NewCID;
            public List<string> OldCID;
        }

        public string Name;
        public string Version;
        public FactoryInfoX FactoryInfo;
        public List<ClassInfo> Classes = new();
        public List<CompatibilityX> Compatibility = new();
    }
}
