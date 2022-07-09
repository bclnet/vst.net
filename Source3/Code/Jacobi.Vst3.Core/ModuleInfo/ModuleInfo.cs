using System.Collections.Generic;

namespace Jacobi.Vst3.Core.ModuleInfo
{
    public class ModuleInfo
    {
		public struct FactoryInfo_
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

		public struct Compatibility_
		{
			public string NewCID;
			public List<string> OldCID;
		}

		public string Name;
		public string Version;
		public FactoryInfo_ FactoryInfo;
		public List<ClassInfo> Classes = new();
		public List<Compatibility_> Compatibility = new();
	}
}
