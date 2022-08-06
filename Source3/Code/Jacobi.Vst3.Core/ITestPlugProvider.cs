using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// This class provides access to the component and the controller of a plug-in when running a unit test (see ITest).
    /// You get this interface as the context argument in the ITestFactory::createTests method.
    /// </summary>
    [ComImport, Guid(Interfaces.ITestPlugProvider), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestPlugProvider
    {
        /// <summary>
        /// get the component of the plug-in.
        /// The reference count of the component is increased in this function and you need to call
        /// releasePlugIn when done with the component.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IComponent GetComponent();

        /// <summary>
        /// get the controller of the plug-in.
        /// The reference count of the controller is increased in this function and you need to call
        /// releasePlugIn when done with the controller.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IEditController GetController();

        /// <summary>
        /// release the component and/or controller
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="edit"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ReleasePlugIn(
            [MarshalAs(UnmanagedType.Interface), In] IComponent obj,
            [MarshalAs(UnmanagedType.Interface), In] IEditController edit);

        /// <summary>
        /// get the sub categories of the plug-in
        /// </summary>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetSubCategories(
            [MarshalAs(UnmanagedType.Interface), In] IStringResult result);

        /// <summary>
        /// get the component UID of the plug-in
        /// </summary>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetComponentUID(
            [MarshalAs(UnmanagedType.Struct), Out] out Guid uid);
    }

    static partial class Interfaces
    {
        public const string ITestPlugProvider = "86BE70EE-4E99-430F-978F-1E6ED68FB5BA";
    }

    /// <summary>
    /// Test Helper extension.
    /// </summary>
    [ComImport, Guid(Interfaces.ITestPlugProvider2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITestPlugProvider2 : ITestPlugProvider
    {
        /// <summary>
        /// get the component of the plug-in.
        /// The reference count of the component is increased in this function and you need to call
        /// releasePlugIn when done with the component.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        new IComponent GetComponent();

        /// <summary>
        /// get the controller of the plug-in.
        /// The reference count of the controller is increased in this function and you need to call
        /// releasePlugIn when done with the controller.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        new IEditController GetController();

        /// <summary>
        /// release the component and/or controller
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="edit"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 ReleasePlugIn(
            [MarshalAs(UnmanagedType.Interface), In] IComponent obj,
            [MarshalAs(UnmanagedType.Interface), In] IEditController edit);

        /// <summary>
        /// get the sub categories of the plug-in
        /// </summary>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 GetSubCategories(
            [MarshalAs(UnmanagedType.Interface), In] IStringResult result);

        /// <summary>
        /// get the component UID of the plug-in
        /// </summary>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        new Int32 GetComponentUID(
            [MarshalAs(UnmanagedType.Struct), Out] out Guid uid);

        //-------------------------------------------------------------

        /// <summary>
        /// get the plugin factory.
        /// The reference count of the returned factory object is not increased when calling this function.
        /// </summary>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IPluginFactory GetPluginFactory();
    }

    static partial class Interfaces
    {
        public const string ITestPlugProvider2 = "C7C75364-7B83-43AC-A449-5B0A3E5A46C7";
    }
}
