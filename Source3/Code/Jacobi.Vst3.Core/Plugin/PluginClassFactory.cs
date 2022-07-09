using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Plugin
{
    [ClassInterface(ClassInterfaceType.None)]
    public class PluginClassFactory : IPluginFactory, IPluginFactory2, IPluginFactory3, IServiceContainerSite, IDisposable
    {
        private readonly List<ClassRegistration> _registrations = new List<ClassRegistration>();

        //public const string AudioModuleClassCategory = "Audio Module Class";
        //public const string ComponentControllerClassCategory = "Component Controller Class";
        //public const string TestClassCategory = "Test Class";
        //public static readonly VstVersion Vst3SdkVersion = new Version(3, 6, 14);

        public PluginClassFactory(string vendor, string email, string url)
            : this(vendor, email, url, PFactoryInfo.FactoryFlags.NoFlags) { }

        public PluginClassFactory(string vendor, string email, string url, PFactoryInfo.FactoryFlags flags)
            : this(vendor, email, url, flags, Constants.Vst3SdkVersion) { }

        public PluginClassFactory(string vendor, string email, string url, PFactoryInfo.FactoryFlags flags, Version sdkVersion)
        {
            Vendor = vendor;
            Email = email;
            Url = url;
            Flags = flags | PFactoryInfo.FactoryFlags.Unicode;
            SdkVersion = sdkVersion;

            DefaultVersion = ReflectionExtensions.GetExportAssembly().GetAssemblyVersion();

            ServiceContainer = new ServiceContainer();
        }

        public string Vendor { get; private set; }

        public string Url { get; private set; }

        public string Email { get; private set; }

        public PFactoryInfo.FactoryFlags Flags { get; private set; }

        public Version SdkVersion { get; private set; }

        public ServiceContainer ServiceContainer { get; protected set; }

        public Version DefaultVersion { get; set; }

        public ClassRegistration Register(Type classType, ClassRegistration.ObjectClasses objClass)
        {
            var reg = new ClassRegistration
            {
                ClassType = classType,
                ObjectClass = objClass,
            };

            Register(reg);

            return reg;
        }

        public void Register(ClassRegistration registration)
        {
            if (registration.ClassTypeId == Guid.Empty && registration.ClassType.GUID == Guid.Empty)
                throw new ArgumentException("The ClassTypeId property is not set and the ClassType (class) does not have a GuidAttribute set.", nameof(registration.ClassType));
            if (string.IsNullOrEmpty(registration.DisplayName) && registration.ClassType.GetDisplayName() == null)
                throw new ArgumentException("The DisplayName property is not set and the ClassType (class) does not have a DisplayNameAttribute set.", nameof(registration.ClassType));

            EnrichRegistration(registration);

            _registrations.Add(registration);
        }

        public bool Unregister(Type classType)
        {
            foreach (var reg in _registrations)
                if (reg.ClassType.FullName == classType.FullName)
                {
                    _registrations.Remove(reg);
                    return true;
                }

            return false;
        }

        public ClassRegistration Find(Guid classId)
            => (from reg in _registrations
                where reg.ClassTypeId == classId
                select reg).FirstOrDefault();

        protected virtual object CreateObjectInstance(ClassRegistration registration)
        {
            object instance = registration.CreatorCallback != null
                ? registration.CreatorCallback(ServiceContainer, registration.ClassType)
                : Activator.CreateInstance(registration.ClassType);

            // link-up service container hierarchy
            if (instance is IServiceContainerSite site && site.ServiceContainer != null) site.ServiceContainer.ParentContainer = ServiceContainer;

            return instance;
        }

        protected virtual void EnrichRegistration(ClassRegistration registration)
        {
            // internals
            if (registration.ClassTypeId == Guid.Empty) registration.ClassTypeId = registration.ClassType.GUID;
            if (string.IsNullOrEmpty(registration.DisplayName)) registration.DisplayName = registration.ClassType.GetDisplayName();
            if (string.IsNullOrEmpty(registration.Vendor)) registration.Vendor = Vendor;
            if (registration.Version == null) registration.Version = DefaultVersion;
        }

        #region IPluginFactory Members

        public virtual int GetFactoryInfo(out PFactoryInfo info)
        {
            info = default;
            info.Email = Email;
            info.Flags = Flags;
            info.Url = Url;
            info.Vendor = Vendor;

            return TResult.S_OK;
        }

        public virtual int CountClasses()
            => _registrations.Count;

        public virtual int GetClassInfo(int index, out PClassInfo info)
        {
            info = default;
            if (!IsValidRegIndex(index)) return TResult.E_InvalidArg;

            var reg = _registrations[index];
            
            FillClassInfo(ref info, reg);

            return TResult.S_OK;
        }

        protected virtual void FillClassInfo(ref PClassInfo info, ClassRegistration reg)
        {
            info.Cardinality = PClassInfo.ClassCardinality.ManyInstances;
            info.Category = ObjectClassToCategory(reg.ObjectClass);
            info.Cid = reg.ClassTypeId;
            info.Name = reg.DisplayName;
        }

        public virtual int CreateInstance(ref Guid classId, ref Guid interfaceId, out IntPtr instance)
        {
            // seems not every host is programmed defensively...
            //if (instance != IntPtr.Zero) return TResult.E_Pointer;

            var reg = Find(classId);
            if (reg != null)
            {
                var obj = CreateObjectInstance(reg);
                var unk = Marshal.GetIUnknownForObject(obj);
                try
                {
                    return Marshal.QueryInterface(unk, ref interfaceId, out instance);
                }
                finally
                {
                    Marshal.Release(unk);
                }
            }

            instance = IntPtr.Zero;
            return TResult.E_ClassNotReg;
        }

        #endregion

        #region IPluginFactory2 Members

        public virtual int GetClassInfo2(int index, out PClassInfo2 info)
        {
            info = default;
            if (!IsValidRegIndex(index)) return TResult.E_InvalidArg;

            var reg = _registrations[index];

            FillClassInfo2(ref info, reg);

            return TResult.S_OK;
        }

        protected virtual void FillClassInfo2(ref PClassInfo2 info, ClassRegistration reg)
        {
            info.Cardinality = PClassInfo.ClassCardinality.ManyInstances;
            info.Category = ObjectClassToCategory(reg.ObjectClass);
            info.ClassFlags = reg.ClassFlags;
            info.Cid = reg.ClassTypeId;
            info.Name = reg.DisplayName;
            info.SdkVersion = FormatSdkVersionString(SdkVersion);
            info.SubCategories = reg.Categories.ToString();
            info.Vendor = reg.Vendor;
            info.Version = reg.Version.ToString();
        }

        #endregion

        #region IPluginFactory3 Members

        public virtual int GetClassInfoUnicode(int index, out PClassInfoW info)
        {
            info = default;
            if (!IsValidRegIndex(index)) return TResult.E_InvalidArg;

            var reg = _registrations[index];

            FillClassInfoW(ref info, reg);

            return TResult.S_OK;
        }

        protected virtual void FillClassInfoW(ref PClassInfoW info, ClassRegistration reg)
        {
            info.Cardinality = PClassInfo.ClassCardinality.ManyInstances;
            info.Category.Value = ObjectClassToCategory(reg.ObjectClass);
            info.ClassFlags = reg.ClassFlags;
            info.Cid = reg.ClassType.GUID;
            info.Name = reg.DisplayName;
            info.SdkVersion = FormatSdkVersionString(SdkVersion);
            info.SubCategories.Value = reg.Categories.ToString();
            info.Vendor = reg.Vendor;
            info.Version = reg.Version.ToString();
        }

        public virtual int SetHostContext(object context)
        {
            ServiceContainer.Unknown = context;
            return TResult.S_OK;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeAll)
        {
            _registrations.Clear();
            ServiceContainer?.Dispose();
        }

        #endregion

        private bool IsValidRegIndex(int index) => index >= 0 && index < _registrations.Count;

        private static string FormatSdkVersionString(Version sdkVersion) => $"VST {sdkVersion}";

        private static string ObjectClassToCategory(ClassRegistration.ObjectClasses objClass) => objClass switch
        {
            ClassRegistration.ObjectClasses.AudioModuleClass => Constants.kVstAudioEffectClass,
            ClassRegistration.ObjectClasses.ComponentControllerClass => Constants.kVstComponentControllerClass,
            ClassRegistration.ObjectClasses.TestClass => Constants.kTestClass,
            _ => throw new InvalidEnumArgumentException(nameof(objClass), (int)objClass, typeof(ClassRegistration.ObjectClasses)),
        };
    }
}
