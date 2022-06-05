using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Host
{
    public class FactoryInfo
    {
        public string Vendor => throw new NotImplementedException();
        public string Url => throw new NotImplementedException();
        public string Email => throw new NotImplementedException();
        public int Flags => throw new NotImplementedException();
        public bool ClassesDiscardable => throw new NotImplementedException();
        public bool LicenseCheck => throw new NotImplementedException();
        public bool ComponentNonDiscardable => throw new NotImplementedException();
    }

    public class ClassInfo
    {
        public Guid ID => throw new NotImplementedException();
        public int Cardinality => throw new NotImplementedException();
        public string Category => throw new NotImplementedException();
        public string Name => throw new NotImplementedException();
        public string Vendor => throw new NotImplementedException();
        public string Version => throw new NotImplementedException();
        public string SdkVersion => throw new NotImplementedException();
        public string[] SubCategories { get; set; } = Array.Empty<string>();

        public string SubCategoriesString() => SubCategories.Length == 0 ? string.Empty : string.Join('|', SubCategories);

        public uint ClassFlags => throw new NotImplementedException();

        struct Data
        {
            Guid ClassID;
            int Cardinality;
            string Category;
            string Name;
            string Vendor;
            string Version;
            string SdkVersion;
            List<string> SubCategories;
            uint ClassFlags;
        }

        void ParseSubCategories(string str) => SubCategories = str.Split("|");
    }

    public class PluginFactory
    {
        IPluginFactory Factory;

        void SetHostContext([MarshalAs(UnmanagedType.IUnknown), In] Object context)
        {
            if (context is IPluginFactory3 f)
            {
                f.SetHostContext(context);
            }
        }

        public FactoryInfo Info => throw new NotImplementedException();
        public int ClassCount => throw new NotImplementedException();
        public List<ClassInfo> ClassInfos => throw new NotImplementedException();
        public T CreateInstance<T>(Guid classID) => throw new NotImplementedException();
    }

    public abstract class Module : IDisposable
    {
        public class Snapshot
        {
            public struct ImageDesc
            {
                public double ScaleFactor;
                public string Path;
            }

            public Guid Uid { get; internal set; }
            public List<ImageDesc> Images => new();

            static (int first, int second) RangeOfScaleFactor(string name)
            {
                var result = (first: -1, second: -1);
                var xIndex = name.LastIndexOf('x');
                if (xIndex == -1) return result;
                var indicatorIndex = name.LastIndexOf('_');
                if (xIndex < indicatorIndex) return result;
                result.first = indicatorIndex + 1;
                result.second = xIndex;
                return result;
            }

            internal static double? DecodeScaleFactor(string name)
            {
                var range = RangeOfScaleFactor(name);
                if (range.first == -1 || range.second == -1) return null;
                var tmp = name[range.first..(range.second - range.first)];
                return double.TryParse(tmp, out var z) ? z : null;
            }

            //------------------------------------------------------------------------
            internal static Guid? DecodeUID(string filename)
            {
                if (filename.Length < 45) return null;
                if (filename.IndexOf("_snapshot") != 32) return null;
                var uidStr = filename[..32];
                return Guid.Parse(uidStr);
            }
        }

        public abstract void Dispose();

        public static Module Create(string path, out string errorDescription) => ModuleWin32.Create(path, out errorDescription);
        public static List<string> GetModulePaths() => ModuleWin32.GetModulePaths();
        public static List<Snapshot> GetSnapshots(string modulePath) => ModuleWin32.GetSnapshots(modulePath);
        // get the path to the module info json file if it exists
        public static string GetModuleInfoPath(string modulePath) => ModuleWin32.GetModuleInfoPath(modulePath);

        public string Name { get; internal set; }
        public string Path { get; internal set; }
        public PluginFactory Factory { get;} = new PluginFactory(); 

        protected abstract bool Load(string path, out string errorDescription);
        protected IPluginFactory _factory;
    }
}
