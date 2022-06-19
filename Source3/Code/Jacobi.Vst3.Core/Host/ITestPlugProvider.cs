using Jacobi.Vst3.Core;
using System;

namespace Jacobi.Vst3.Host
{
    // This class provides access to the component and the controller of a plug-in when running a unit test(see ITest).
    // You get this interface as the context argument in the ITestFactory::createTests method.
    public interface ITestPlugProvider
    {
        static readonly Guid iid = new("86BE70EE-4E99-430F-978F-1E6ED68FB5BA");

        /// <summary>
        /// get the component of the plug-in.
        /// The reference count of the component is increased in this function and you need to call
        /// releasePlugIn when done with the component.
        /// </summary>
        /// <returns></returns>
        public IComponent GetComponent();

        /// <summary>
        /// get the controller of the plug-in.
        /// The reference count of the controller is increased in this function and you need to call
        /// releasePlugIn when done with the controller.
        /// </summary>
        /// <returns></returns>
        public IEditController GetController();

        /// <summary>
        /// release the component and/or controller
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="edit"></param>
        /// <returns></returns>
        public int ReleasePlugIn(IComponent obj, IEditController edit);

        /// <summary>
        /// get the sub categories of the plug-in
        /// </summary>
        public int GetSubCategories(IStringResult result);

        /// <summary>
        /// get the component UID of the plug-in
        /// </summary>
        public int GetComponentUID(out Guid uid);
    }

    // Test Helper extension.
    public interface ITestPlugProvider2 : ITestPlugProvider
    {
        static readonly new Guid iid = new("C7C75364-7B83-43AC-A449-5B0A3E5A46C7");

        /// <summary>
        /// get the plugin factory.
        /// The reference count of the returned factory object is not increased when calling this function.
        /// </summary>
        public IPluginFactory GetPluginFactory();
    }
}
