using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using static Jacobi.Vst3.Core.PFactoryInfo;

namespace Jacobi.Vst3.Host
{
    public class FactoryInfo
    {
        PFactoryInfo _info;
        public FactoryInfo(PFactoryInfo info) => _info = info;
        public string Vendor => _info.Vendor;
        public string Url => _info.Url;
        public string Email => _info.Email;
        public FactoryFlags Flags => _info.Flags;
        public bool ClassesDiscardable => (_info.Flags & FactoryFlags.ClassesDiscardable) != 0;
        public bool LicenseCheck => (_info.Flags & FactoryFlags.LicenseCheck) != 0;
        public bool ComponentNonDiscardable => (_info.Flags & FactoryFlags.ComponentNonDiscardable) != 0;
    }

    public class ClassInfo
    {
        internal Data data;

        public ClassInfo() { }
        public ClassInfo(ref PClassInfo info)
        {
            data.ClassID = info.ClassId;
            data.Cardinality = info.Cardinality;
            data.Category = info.Category;
            data.Name = info.Name;
        }
        public ClassInfo(ref PClassInfo2 info)
        {
            data.ClassID = info.ClassId;
            data.Cardinality = info.Cardinality;
            data.Category = info.Category;
            data.Name = info.Name;
            data.Vendor = info.Vendor;
            data.Version = info.Version;
            data.SdkVersion = info.SdkVersion;
            ParseSubCategories(info.SubCategories);
            data.ClassFlags = info.ClassFlags;
        }
        public ClassInfo(ref PClassInfoW info)
        {
            data.ClassID = info.ClassId;
            data.Cardinality = info.Cardinality;
            data.Category = info.Category.Value;
            data.Name = info.Name;
            data.Vendor = info.Vendor;
            data.Version = info.Version;
            data.SdkVersion = info.SdkVersion;
            ParseSubCategories(info.SubCategories.Value);
            data.ClassFlags = info.ClassFlags;
        }

        public Guid ID => data.ClassID;
        public int Cardinality => data.Cardinality;
        public string Category => data.Category;
        public string Name => data.Name;
        public string Vendor => data.Vendor;
        public string Version => data.Version;
        public string SdkVersion => data.SdkVersion;
        public List<string> SubCategories => data.SubCategories;
        public string SubCategoriesString() => SubCategories.Count == 0 ? string.Empty : string.Join('|', SubCategories);
        public ComponentClassFlags ClassFlags => data.ClassFlags;

        internal struct Data
        {
            public Guid ClassID;
            public int Cardinality;
            public string Category;
            public string Name;
            public string Vendor;
            public string Version;
            public string SdkVersion;
            public List<string> SubCategories;
            public ComponentClassFlags ClassFlags;
        }

        void ParseSubCategories(string str) => data.SubCategories = str.Split("|").ToList();
    }

    public class PluginFactory
    {
        IPluginFactory _factory;

        public PluginFactory(IPluginFactory factory) => _factory = factory;

        void SetHostContext([MarshalAs(UnmanagedType.IUnknown), In] object context)
        {
            if (context is IPluginFactory3 f)
            {
                f.SetHostContext(context);
            }
        }

        public FactoryInfo Info
        {
            get
            {
                var info = new PFactoryInfo();
                _factory.GetFactoryInfo(ref info);
                return new FactoryInfo(info);
            }
        }

        public int ClassCount
        {
            get
            {
                var count = _factory.CountClasses();
                Debug.Assert(count >= 0);
                return count;
            }
        }

        const int kResultTrue = 0;
        public List<ClassInfo> ClassInfos
        {
            get
            {
                var count = ClassCount;
                var factoryInfo = (FactoryInfo)null;
                var result = new List<ClassInfo> { Capacity = count };
                var f3 = _factory as IPluginFactory3;
                var f2 = _factory as IPluginFactory2;
                var ci = new PClassInfo();
                var ci2 = new PClassInfo2();
                var ci3 = new PClassInfoW();
                for (var i = 0; i < count; i++)
                {
                    if (f3 != null && f3.GetClassInfoUnicode(i, ref ci3) == kResultTrue) result.Add(new ClassInfo(ref ci3));
                    else if (f2 != null && f2.GetClassInfo2(i, ref ci2) == kResultTrue) result.Add(new ClassInfo(ref ci2));
                    else if (_factory.GetClassInfo(i, ref ci) == kResultTrue) result.Add(new ClassInfo(ref ci));
                    var classInfo = result.LastOrDefault();
                    if (classInfo != null && string.IsNullOrEmpty(classInfo.Vendor))
                    {
                        if (factoryInfo == null) factoryInfo = Info;
                        classInfo.data.Vendor = factoryInfo.Vendor;
                    }
                }
                return result;
            }
        }

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

        protected IPluginFactory _factory;
        public abstract void Dispose();

        public static Module Create(string path, out string errorDescription) => ModuleWin32.Create(path, out errorDescription);
        public static List<string> GetModulePaths() => ModuleWin32.GetModulePaths();
        public static List<Snapshot> GetSnapshots(string modulePath) => ModuleWin32.GetSnapshots(modulePath);
        // get the path to the module info json file if it exists
        public static string GetModuleInfoPath(string modulePath) => ModuleWin32.GetModuleInfoPath(modulePath);

        public string Name { get; internal set; }
        public string Path { get; internal set; }
        public PluginFactory Factory { get; internal set; }

        protected abstract bool Load(string path, out string errorDescription);
    }
}
