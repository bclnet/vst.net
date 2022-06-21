using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

//: ref https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsondocument.parse?view=net-6.0
namespace Jacobi.Vst3.Core.ModuleInfo
{
    public partial class ModuleInfoLib
    {
        class parse_error : Exception
        {
            string str;
            public parse_error(string str, string value) => this.str = str;
            public parse_error(string str, JsonProperty value) => this.str = str;
            public parse_error(string str, JsonElement value) => this.str = str;
        }

        class ModuleInfoJsonParser
        {
            public ModuleInfo info = new();

            enum ParseFactoryInfoBits
            {
                Vendor = 1 << 0,
                URL = 1 << 1,
                EMail = 1 << 2,
                Flags = 1 << 3,
            }
            void ParseFactoryInfo(JsonElement value)
            {
                ParseFactoryInfoBits parsed = 0;
                var obj = value.EnumerateObject();
                foreach (var el in obj)
                {
                    var elementName = el.Name;
                    if (elementName == "Vendor")
                    {
                        if ((parsed & ParseFactoryInfoBits.Vendor) != 0) throw new parse_error("Only one 'Vendor' key allowed", el);
                        parsed |= ParseFactoryInfoBits.Vendor;
                        info.FactoryInfo.Vendor = el.Value.GetString();
                    }
                    else if (elementName == "URL")
                    {
                        if ((parsed & ParseFactoryInfoBits.URL) != 0) throw new parse_error("Only one 'URL' key allowed", el);
                        parsed |= ParseFactoryInfoBits.URL;
                        info.FactoryInfo.Url = el.Value.GetString();
                    }
                    else if (elementName == "E-Mail")
                    {
                        if ((parsed & ParseFactoryInfoBits.EMail) != 0) throw new parse_error("Only one 'E-Mail' key allowed", el);
                        parsed |= ParseFactoryInfoBits.EMail;
                        info.FactoryInfo.Email = el.Value.GetString();
                    }
                    else if (elementName == "Flags")
                    {
                        if ((parsed & ParseFactoryInfoBits.Flags) != 0) throw new parse_error("Only one 'Flags' key allowed", el);
                        var flags = el.Value.EnumerateObject();
                        //if (!flags) throw new parse_error("Expect 'Flags' to be a JSON Object", el);
                        foreach (var flag in flags)
                        {
                            var flagName = flag.Name;
                            var flagValue = flag.Value.GetBoolean();
                            if (!flagValue) throw new parse_error("Flag must be a boolean", flag);
                            if (flagName == "Classes Discardable")
                            {
                                if (flagValue) info.FactoryInfo.Flags |= PFactoryInfo.FactoryFlags.ClassesDiscardable;
                            }
                            else if (flagName == "Component Non Discardable")
                            {
                                if (flagValue) info.FactoryInfo.Flags |= PFactoryInfo.FactoryFlags.ComponentNonDiscardable;
                            }
                            else if (flagName == "Unicode")
                            {
                                if (flagValue) info.FactoryInfo.Flags |= PFactoryInfo.FactoryFlags.Unicode;
                            }
                            else throw new parse_error("Unknown flag", flagName);
                        }
                        parsed |= ParseFactoryInfoBits.Flags;
                    }
                }
                if ((parsed & ParseFactoryInfoBits.Vendor) == 0) throw new parse_error("Missing 'Vendor' in Factory Info", value);
                if ((parsed & ParseFactoryInfoBits.URL) == 0) throw new parse_error("Missing 'URL' in Factory Info", value);
                if ((parsed & ParseFactoryInfoBits.EMail) == 0) throw new parse_error("Missing 'EMail' in Factory Info", value);
                if ((parsed & ParseFactoryInfoBits.Flags) == 0) throw new parse_error("Missing 'Flags' in Factory Info", value);
            }

            enum ParseClassesBits
            {
                CID = 1 << 0,
                Category = 1 << 1,
                Name = 1 << 2,
                Vendor = 1 << 3,
                Version = 1 << 4,
                SDKVersion = 1 << 5,
                SubCategories = 1 << 6,
                ClassFlags = 1 << 7,
                Snapshots = 1 << 8,
                Cardinality = 1 << 9,
            }
            void ParseClasses(JsonElement value)
            {
                var array = value.EnumerateArray();
                //if (!array) throw new parse_error("Expect Classes Array", value);
                foreach (var classInfoEl in array)
                {
                    var classInfo = classInfoEl.EnumerateObject();
                    //if (!classInfo) throw new parse_error("Expect Class Object", classInfoEl);
                    ModuleInfo.ClassInfo ci = new();
                    ParseClassesBits parsed = 0;
                    foreach (var el in classInfo)
                    {
                        var elementName = el.Name;
                        if (elementName == "CID")
                        {
                            if ((parsed & ParseClassesBits.CID) != 0) throw new parse_error("Only one 'CID' key allowed", el);
                            ci.Cid = el.Value.GetString();
                            parsed |= ParseClassesBits.CID;
                        }
                        else if (elementName == "Category")
                        {
                            if ((parsed & ParseClassesBits.Category) != 0) throw new parse_error("Only one 'Category' key allowed", el);
                            ci.Category = el.Value.GetString();
                            parsed |= ParseClassesBits.Category;
                        }
                        else if (elementName == "Name")
                        {
                            if ((parsed & ParseClassesBits.Name) != 0) throw new parse_error("Only one 'Name' key allowed", el);
                            ci.Name = el.Value.GetString();
                            parsed |= ParseClassesBits.Name;
                        }
                        else if (elementName == "Vendor")
                        {
                            if ((parsed & ParseClassesBits.Vendor) != 0) throw new parse_error("Only one 'Vendor' key allowed", el);
                            ci.Vendor = el.Value.GetString();
                            parsed |= ParseClassesBits.Vendor;
                        }
                        else if (elementName == "Version")
                        {
                            if ((parsed & ParseClassesBits.Version) != 0) throw new parse_error("Only one 'Version' key allowed", el);
                            ci.Version = el.Value.GetString();
                            parsed |= ParseClassesBits.Version;
                        }
                        else if (elementName == "SDKVersion")
                        {
                            if ((parsed & ParseClassesBits.SDKVersion) != 0) throw new parse_error("Only one 'SDKVersion' key allowed", el);
                            ci.SdkVersion = el.Value.GetString();
                            parsed |= ParseClassesBits.SDKVersion;
                        }
                        else if (elementName == "Sub Categories")
                        {
                            if ((parsed & ParseClassesBits.SubCategories) != 0) throw new parse_error("Only one 'Sub Categories' key allowed", el);
                            var subCatArr = el.Value.EnumerateArray();
                            //if (!subCatArr) throw new parse_error("Expect Array here", el);
                            foreach (var catEl in subCatArr)
                            {
                                var cat = catEl.GetString();
                                ci.SubCategories.Add(cat);
                            }
                            parsed |= ParseClassesBits.SubCategories;
                        }
                        else if (elementName == "Class Flags")
                        {
                            if ((parsed & ParseClassesBits.ClassFlags) != 0) throw new parse_error("Only one 'Class Flags' key allowed", el);
                            ci.Flags = el.Value.GetUInt32();
                            parsed |= ParseClassesBits.ClassFlags;
                        }
                        else if (elementName == "Cardinality")
                        {
                            if ((parsed & ParseClassesBits.Cardinality) != 0) throw new parse_error("Only one 'Cardinality' key allowed", el);
                            ci.Cardinality = el.Value.GetInt32();
                            parsed |= ParseClassesBits.Cardinality;
                        }
                        else if (elementName == "Snapshots")
                        {
                            if ((parsed & ParseClassesBits.Snapshots) != 0) throw new parse_error("Only one 'Snapshots' key allowed", el);
                            var snapArr = el.Value.EnumerateArray();
                            //if (snapArr == null) throw new parse_error("Expect Array here", el);
                            foreach (var snapEl in snapArr)
                            {
                                var snap = snapEl.EnumerateObject();
                                //if (!snap) throw new parse_error("Expect Object here", snapEl);
                                ModuleInfo.Snapshot snapshot = new();
                                foreach (var spEl in snap)
                                {
                                    var spElName = spEl.Name;
                                    if (spElName == "Path") snapshot.Path = spEl.Value.GetString();
                                    else if (spElName == "Scale Factor") snapshot.ScaleFactor = spEl.Value.GetDouble();
                                    else throw new parse_error("Unexpected key", spEl);
                                }
                                if (snapshot.ScaleFactor == 0f || string.IsNullOrEmpty(snapshot.Path)) throw new parse_error("Missing Snapshot keys", snapEl);
                                ci.Snapshots.Add(snapshot);
                            }
                            parsed |= ParseClassesBits.Snapshots;
                        }
                        else throw new parse_error("Unexpected key", el);
                    }
                    if ((parsed & ParseClassesBits.CID) == 0) throw new parse_error("'CID' key missing", classInfoEl);
                    if ((parsed & ParseClassesBits.Category) == 0) throw new parse_error("'Category' key missing", classInfoEl);
                    if ((parsed & ParseClassesBits.Name) == 0) throw new parse_error("'Name' key missing", classInfoEl);
                    if ((parsed & ParseClassesBits.Vendor) == 0) throw new parse_error("'Vendor' key missing", classInfoEl);
                    if ((parsed & ParseClassesBits.Version) == 0) throw new parse_error("'Version' key missing", classInfoEl);
                    if ((parsed & ParseClassesBits.SDKVersion) == 0) throw new parse_error("'SDK Version' key missing", classInfoEl);
                    if ((parsed & ParseClassesBits.ClassFlags) == 0) throw new parse_error("'Class Flags' key missing", classInfoEl);
                    if ((parsed & ParseClassesBits.Cardinality) == 0) throw new parse_error("'Cardinality' key missing", classInfoEl);
                    info.Classes.Add(ci);
                }
            }

            internal void ParseCompatibility(JsonElement value)
            {
                var arr = value.EnumerateArray();
                //if (!arr) throw new parse_error("Expect Array here", value);
                foreach (var el in arr)
                {
                    var obj = el.EnumerateObject();
                    //if (!obj) throw new parse_error("Expect Object here", el);

                    ModuleInfo.Compatibility_ compat = new();
                    foreach (var objEl in obj)
                    {
                        var elementName = objEl.Name;
                        if (elementName == "New") compat.NewCID = objEl.Value.GetString();
                        else if (elementName == "Old")
                        {
                            var oldElArr = objEl.Value.EnumerateArray();
                            //if (!oldElArr) throw new parse_error("Expect Array here", objEl);
                            foreach (var old in oldElArr) compat.OldCID.Add(old.GetString());
                        }
                    }
                    if (string.IsNullOrEmpty(compat.NewCID)) throw new parse_error("Expect New CID here", el);
                    if (compat.OldCID?.Any() != true) throw new parse_error("Expect Old CID here", el);
                    info.Compatibility.Add(compat);
                }
            }

            enum ParseBits
            {
                Name = 1 << 0,
                Version = 1 << 1,
                FactoryInfo = 1 << 2,
                Compatibility = 1 << 3,
                Classes = 1 << 4,
            }
            public void Parse(JsonElement doc)
            {
                ParseBits parsed = 0;
                var docObj = doc.EnumerateObject();
                //if (!docObj) throw new parse_error("Unexpected", doc);
                foreach (var el in docObj)
                {
                    var elementName = el.Name;
                    if (elementName == "Name")
                    {
                        if ((parsed & ParseBits.Name) != 0) throw new parse_error("Only one 'Name' key allowed", el);
                        parsed |= ParseBits.Name;
                        info.Name = el.Value.GetString();
                    }
                    else if (elementName == "Version")
                    {
                        if ((parsed & ParseBits.Version) != 0) throw new parse_error("Only one 'Version' key allowed", el);
                        parsed |= ParseBits.Version;
                        info.Version = el.Value.GetString();
                    }
                    else if (elementName == "Factory Info")
                    {
                        if ((parsed & ParseBits.FactoryInfo) != 0) throw new parse_error("Only one 'Factory Info' key allowed", el);
                        ParseFactoryInfo(el.Value);
                        parsed |= ParseBits.FactoryInfo;
                    }
                    else if (elementName == "Compatibility")
                    {
                        if ((parsed & ParseBits.Compatibility) != 0) throw new parse_error("Only one 'Compatibility' key allowed", el);
                        ParseCompatibility(el.Value);
                        parsed |= ParseBits.Compatibility;
                    }
                    else if (elementName == "Classes")
                    {
                        if ((parsed & ParseBits.Classes) != 0) throw new parse_error("Only one 'Classes' key allowed", el);
                        ParseClasses(el.Value);
                        parsed |= ParseBits.Classes;
                    }
                    else throw new parse_error("Unexpected JSON Token", el);
                }
                if ((parsed & ParseBits.Name) == 0) throw new InvalidDataException("'Name' key missing");
                if ((parsed & ParseBits.Version) == 0) throw new InvalidDataException("'Version' key missing");
                if ((parsed & ParseBits.FactoryInfo) == 0) throw new InvalidDataException("'Factory Info' key missing");
                if ((parsed & ParseBits.Classes) == 0) throw new InvalidDataException("'Classes' key missing");
            }
        }

        public static ModuleInfo ParseJson(string jsonData, TextWriter optErrorOutput)
        {
            var docVar = JsonDocument.Parse(jsonData);
            var doc = docVar.RootElement;
            try
            {
                var parser = new ModuleInfoJsonParser();
                parser.Parse(doc);
                return parser.info;
            }
            catch (Exception ex)
            {
                optErrorOutput?.WriteLine(ex.Message);
                return null;
            }
        }

        public static List<ModuleInfo.Compatibility_> ParseCompatibilityJson(string jsonData, TextWriter optErrorOutput)
        {
            var docVar = JsonDocument.Parse(jsonData);
            var doc = docVar.RootElement;
            try
            {
                ModuleInfoJsonParser parser = new();
                parser.ParseCompatibility(doc);
                return parser.info.Compatibility;
            }
            catch (Exception ex)
            {
                optErrorOutput?.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
