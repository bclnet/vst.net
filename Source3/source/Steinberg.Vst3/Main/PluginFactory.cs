using System;
using System.Runtime.InteropServices;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Default Class Factory implementation.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    public class CPluginFactory : IPluginFactory3, IDisposable
    {
        static CPluginFactory gPluginFactory = null;

        protected PFactoryInfo factoryInfo;
        protected PClassEntry[] classes;
        protected int classCount;
        protected int maxClassCount;

        protected struct PClassEntry
        {
            public PClassInfo2 Info8;
            public PClassInfoW Info16;

            public Func<object, object> CreateFunc;
            public object Context;
            public bool IsUnicode;
        }

        public CPluginFactory(ref PFactoryInfo info)
            => factoryInfo = info;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (gPluginFactory == this)
                gPluginFactory = null;

            if (classes != null)
                Array.Clear(classes);
        }

        /// <summary>
        /// Registers a plug-in class with classInfo version 1, returns true for success.
        /// </summary>
        /// <returns></returns>
        public bool RegisterClass(ref PClassInfo info, Func<object, object> createFunc, object context = null)
        {
            if (/*info == default ||*/ createFunc == null)
                return false;

            PClassInfo2 info2 = info;
            return RegisterClass(ref info2, createFunc, context);
        }

        /// <summary>
        /// Registers a plug-in class with classInfo version 2, returns true for success.
        /// </summary>
        /// <returns></returns>
        public bool RegisterClass(ref PClassInfo2 info, Func<object, object> createFunc, object context = null)
        {
            if (/*info == default ||*/ createFunc == null)
                return false;

            if (classCount >= maxClassCount)
                if (!GrowClasses())
                    return false;

            ref PClassEntry entry = ref classes[classCount];
            entry.Info8 = info;
            entry.Info16.FromAscii(ref info);
            entry.CreateFunc = createFunc;
            entry.Context = context;
            entry.IsUnicode = false;

            classCount++;
            return true;
        }

        /// <summary>
        /// Registers a plug-in class with classInfo Unicode version, returns true for success.
        /// </summary>
        /// <returns></returns>
        public bool RegisterClass(ref PClassInfoW info, Func<object, object> createFunc, object context = null)
        {
            if (/*info == default ||*/ createFunc == null)
                return false;

            if (classCount >= maxClassCount)
                if (!GrowClasses())
                    return false;

            ref PClassEntry entry = ref classes[classCount];
            entry.Info16 = info;
            entry.CreateFunc = createFunc;
            entry.Context = context;
            entry.IsUnicode = true;

            classCount++;
            return true;
        }

        protected bool GrowClasses()
        {
            const int delta = 10;

            var size = maxClassCount + delta;
            var memory = classes;

            if (memory == null)
                memory = new PClassEntry[size];
            else
                Array.Resize(ref memory, size);

            if (memory == null)
                return false;

            classes = (PClassEntry[])memory;
            maxClassCount += delta;
            return true;
        }

        /// <summary>
        /// Check if a class for a given classId is already registered.
        /// </summary>
        /// <returns></returns>
        public bool IsClassRegistered(Guid cid)
        {
            for (var i = 0; i < classCount; i++)
                if (cid == classes[i].Info16.Cid)
                    return true;
            return false;
        }

        /// <summary>
        /// Remove all classes (no class exported)
        /// </summary>
        public void RemoveAllClasses()
            => classCount = 0;

        #region IPluginFactory

        public virtual TResult GetFactoryInfo(out PFactoryInfo info)
        {
            info = factoryInfo;
            return kResultOk;
        }

        public virtual int CountClasses()
            => classCount;

        public virtual TResult GetClassInfo(int index, out PClassInfo info)
        {
            if (index >= 0 && index < classCount)
            {
                if (classes[index].IsUnicode)
                {
                    info = default;
                    return kResultFalse;
                }

                info = classes[index].Info8;
                return kResultOk;
            }
            info = default;
            return kInvalidArgument;
        }

        public virtual TResult CreateInstance(ref Guid cid, ref Guid iid, out IntPtr obj)
        {
            for (var i = 0; i < classCount; i++)
                if (classes[i].Info16.Cid == cid)
                {
                    var instance = classes[i].CreateFunc(classes[i].Context);
                    if (instance != null)
                    {
                        var unk = Marshal.GetIUnknownForObject(instance);
                        try
                        {
                            if ((TResult)Marshal.QueryInterface(unk, ref cid, out obj) == kResultOk)
                                return kResultOk;
                        }
                        finally
                        {
                            Marshal.Release(unk);
                        }
                    }
                    break;
                }

            obj = IntPtr.Zero;
            return kNoInterface;
        }

        #endregion

        #region IPluginFactory2

        public virtual TResult GetClassInfo2(int index, out PClassInfo2 info)
        {
            if (index >= 0 && index < classCount)
            {
                if (classes[index].IsUnicode)
                {
                    info = default;
                    return kResultFalse;
                }

                info = classes[index].Info8;
                return kResultOk;
            }
            info = default;
            return kInvalidArgument;
        }

        #endregion

        #region IPluginFactory3

        public virtual TResult GetClassInfoUnicode(int index, out PClassInfoW info)
        {
            if (index >= 0 && index < classCount)
            {
                info = classes[index].Info16;
                return kResultOk;
            }
            info = default;
            return kInvalidArgument;
        }

        public virtual TResult SetHostContext(object context)
            => kNotImplemented;

        #endregion
    }
}
