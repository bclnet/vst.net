using Jacobi.Vst3.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3
{
    [ClassInterface(ClassInterfaceType.None)]
    public class PluginClassFactory : IPluginFactory, IPluginFactory2, IPluginFactory3, IServiceContainerSite, IDisposable
    {
        readonly List<ClassRegistration> _registrations = new();

        public PluginClassFactory(string vendor, string url, string email, PFactoryInfo.FactoryFlags flags = PFactoryInfo.FactoryFlags.NoFlags)
        {
            Vendor = vendor;
            Url = url;
            Email = email;
            Flags = flags | PFactoryInfo.FactoryFlags.Unicode;
        }

        public string Vendor { get; private set; }
        public string Url { get; private set; }
        public string Email { get; private set; }
        public PFactoryInfo.FactoryFlags Flags { get; private set; }
        public Version SdkVersion { get; protected set; } = Constants.Vst3SdkVersion;
        public ServiceContainer ServiceContainer { get; protected set; } = new ServiceContainer();
        public Version DefaultVersion { get; set; } = ReflectionExtensions.GetExportAssembly().GetAssemblyVersion();

        public ClassRegistration Register(Type classType, ClassRegistration.ObjectClasses objClass)
            => Register(new ClassRegistration { ClassType = classType, ObjectClass = objClass });

        public ClassRegistration Register(ClassRegistration registration)
        {
            if (registration.ClassTypeId == Guid.Empty && registration.ClassType.GUID == Guid.Empty)
                throw new ArgumentException("The ClassTypeId property is not set and the ClassType (class) does not have a GuidAttribute set.", nameof(registration.ClassType));
            if (string.IsNullOrEmpty(registration.DisplayName) && registration.ClassType.GetDisplayName() == null)
                throw new ArgumentException("The DisplayName property is not set and the ClassType (class) does not have a DisplayNameAttribute set.", nameof(registration.ClassType));

            EnrichRegistration(registration);

            _registrations.Add(registration);

            return registration;
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

        public virtual TResult GetFactoryInfo(out PFactoryInfo info)
        {
            info.Email = Email;
            info.Flags = Flags;
            info.Url = Url;
            info.Vendor = Vendor;

            return kResultOk;
        }

        public virtual int CountClasses()
            => _registrations.Count;

        public virtual TResult GetClassInfo(int index, out PClassInfo info)
        {
            info = default;
            if (!IsValidRegIndex(index)) return kInvalidArgument;

            var reg = _registrations[index];

            FillClassInfo(ref info, reg);

            return kResultOk;
        }

        protected virtual void FillClassInfo(ref PClassInfo info, ClassRegistration reg)
        {
            info.Cardinality = PClassInfo.ClassCardinality.ManyInstances;
            info.Category = ObjectClassToCategory(reg.ObjectClass);
            info.Cid = reg.ClassTypeId;
            info.Name = reg.DisplayName;
        }

        public virtual TResult CreateInstance(ref Guid classId, ref Guid interfaceId, out IntPtr instance)
        {
            // seems not every host is programmed defensively...
            //if (instance != IntPtr.Zero) return E_Pointer;

            var reg = Find(classId);
            if (reg != null)
            {
                var obj = CreateObjectInstance(reg);
                var unk = Marshal.GetIUnknownForObject(obj);
                try
                {
                    return (TResult)Marshal.QueryInterface(unk, ref interfaceId, out instance);
                }
                finally
                {
                    Marshal.Release(unk);
                }
            }

            instance = IntPtr.Zero;
            return E_ClassNotReg;
        }

        #endregion

        #region IPluginFactory2 Members

        public virtual TResult GetClassInfo2(int index, out PClassInfo2 info)
        {
            info = default;
            if (!IsValidRegIndex(index)) return kInvalidArgument;

            var reg = _registrations[index];

            FillClassInfo2(ref info, reg);

            return kResultOk;
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

        public virtual TResult GetClassInfoUnicode(int index, out PClassInfoW info)
        {
            info = default;
            if (!IsValidRegIndex(index)) return kInvalidArgument;

            var reg = _registrations[index];

            FillClassInfoW(ref info, reg);

            return kResultOk;
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

        public virtual TResult SetHostContext(object context)
        {
            ServiceContainer.Unknown = context;
            return kResultOk;
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

        bool IsValidRegIndex(int index) => index >= 0 && index < _registrations.Count;

        static string FormatSdkVersionString(Version sdkVersion) => $"VST {sdkVersion}";

        static string ObjectClassToCategory(ClassRegistration.ObjectClasses objClass) => objClass switch
        {
            ClassRegistration.ObjectClasses.AudioModuleClass => Constants.kVstAudioEffectClass,
            ClassRegistration.ObjectClasses.ComponentControllerClass => Constants.kVstComponentControllerClass,
            ClassRegistration.ObjectClasses.TestClass => Constants.kTestClass,
            _ => throw new InvalidEnumArgumentException(nameof(objClass), (int)objClass, typeof(ClassRegistration.ObjectClasses)),
        };
    }
}
