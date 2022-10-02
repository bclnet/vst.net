using Steinberg.Vst3.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

//: ref https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-use-dom-utf8jsonreader-utf8jsonwriter?pivots=dotnet-6-0
namespace Steinberg.Vst3
{
    public partial class ModuleInfoLib
    {
        static void WriteSnapshots(List<ModuleInfo.Snapshot> snapshots, Utf8JsonWriter w)
        {
            w.WriteStartArray("Snapshots");
            foreach (var el in snapshots)
            {
                w.WriteStartObject();
                w.WriteNumber("Scale Factor", el.ScaleFactor);
                w.WriteString("Path", el.Path);
                w.WriteEndObject();
            }
            w.WriteEndArray();
        }

        static void WriteClassInfo(ModuleInfo.ClassInfo cls, Utf8JsonWriter w)
        {
            w.WriteStartObject();
            w.WriteString("CID", cls.Cid);
            w.WriteString("Category", cls.Category);
            w.WriteString("Name", cls.Name);
            w.WriteString("Vendor", cls.Vendor);
            w.WriteString("Version", cls.Version);
            w.WriteString("SDKVersion", cls.SdkVersion);
            var sc = cls.SubCategories;
            if (sc.Any())
            {
                w.WriteStartArray("Sub Categories");
                foreach (var cat in sc) w.WriteStringValue(cat);
                w.WriteEndArray();
            }
            w.WriteNumber("Class Flags", cls.Flags);
            w.WriteNumber("Cardinality", (int)cls.Cardinality);
            WriteSnapshots(cls.Snapshots, w);
            w.WriteEndObject();
        }

        static void WritePluginCompatibility(List<ModuleInfo.CompatibilityX> cls, Utf8JsonWriter w)
        {
            if (!cls.Any()) return;
            w.WriteStartArray("Compatibility");
            foreach (var el in cls)
            {
                w.WriteStartObject();
                w.WriteString("New", el.NewCID);
                w.WriteStartArray("Old");
                foreach (var oldEl in el.OldCID) w.WriteStringValue(oldEl);
                w.WriteEndArray();
                w.WriteEndObject();
            }
            w.WriteEndArray();
        }

        static void WriteFactoryInfo(ModuleInfo.FactoryInfoX fi, Utf8JsonWriter w)
        {
            w.WriteStartObject("Factory Info");
            w.WriteString("Vendor", fi.Vendor);
            w.WriteString("URL", fi.Url);
            w.WriteString("E-Mail", fi.Email);
            w.WriteStartObject("Flags");
            w.WriteBoolean("Unicode", (fi.Flags & PFactoryInfo.FactoryFlags.Unicode) != 0);
            w.WriteBoolean("Classes Discardable", (fi.Flags & PFactoryInfo.FactoryFlags.ClassesDiscardable) != 0);
            w.WriteBoolean("Component Non Discardable", (fi.Flags & PFactoryInfo.FactoryFlags.ComponentNonDiscardable) != 0);
            w.WriteEndObject();
            w.WriteEndObject();
        }

        public static ModuleInfo CreateModuleInfo(Module module, bool includeDiscardableClasses)
        {
            var factory = module.Factory;
            var factoryInfo = factory.Info;

            var info = new ModuleInfo
            {
                Name = module.Name,
                FactoryInfo = new ModuleInfo.FactoryInfoX
                {
                    Vendor = factoryInfo.Vendor,
                    Url = factoryInfo.Url,
                    Email = factoryInfo.Email,
                    Flags = factoryInfo.Flags
                }
            };
            var pos = info.Name.LastIndexOf('.');
            if (pos != -1) info.Name.Remove(pos);

            if (factoryInfo.ClassesDiscardable == false || (factoryInfo.ClassesDiscardable && includeDiscardableClasses))
            {
                var snapshots = Module.GetSnapshots(module.Path);
                foreach (var ci in factory.ClassInfos)
                {
                    var classInfo = new ModuleInfo.ClassInfo
                    {
                        Cid = ci.ID.ToPackedString(),
                        Category = ci.Category,
                        Name = ci.Name,
                        Vendor = ci.Vendor,
                        Version = ci.Version,
                        SdkVersion = ci.SdkVersion,
                        SubCategories = ci.SubCategories,
                        Snapshots = new(),
                        Cardinality = ci.Cardinality,
                        Flags = (uint)ci.ClassFlags
                    };

                    var snapshotIt = snapshots.SingleOrDefault(el => el.Uid == ci.ID);
                    if (snapshotIt != default)
                    {
                        foreach (var s in snapshotIt.Images)
                        {
                            var path = s.Path;
                            if (path.Contains(module.Path)) path = path.Remove(0, module.Path.Length + 1);
                            classInfo.Snapshots.Add(new ModuleInfo.Snapshot
                            {
                                ScaleFactor = s.ScaleFactor,
                                Path = path
                            });
                        }
                        snapshots.Remove(snapshotIt);
                    }
                    info.Classes.Add(classInfo);
                }
            }
            return info;
        }

        public static void OutputJson(ModuleInfo info, Stream stream)
        {
            var options = new JsonWriterOptions { Indented = true };
            using var w = new Utf8JsonWriter(stream, options);
            w.WriteStartObject();
            w.WriteString("Name", info.Name);
            w.WriteString("Version", info.Version);
            WriteFactoryInfo(info.FactoryInfo, w);
            WritePluginCompatibility(info.Compatibility, w);
            w.WriteStartArray("Classes");
            foreach (var cls in info.Classes) WriteClassInfo(cls, w);
            w.WriteEndArray();
            w.WriteEndObject();
        }
    }
}
