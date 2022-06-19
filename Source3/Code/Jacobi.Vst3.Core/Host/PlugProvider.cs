using Jacobi.Vst3.Core;
using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Host
{
    public class PlugProvider : ITestPlugProvider2
    {
        protected PluginFactory factory;
        protected IComponent component;
        protected IEditController controller;
        protected ClassInfo classInfo;
        protected bool plugIsGlobal;

        protected ConnectionProxy componentCP;
        protected ConnectionProxy controllerCP;

        public PlugProvider(PluginFactory factory, ClassInfo classInfo, bool plugIsGlobal = true)
        {
            this.factory = factory;
            this.component = null;
            this.controller = null;
            this.classInfo = classInfo;
            this.plugIsGlobal = plugIsGlobal;

            if (plugIsGlobal) SetupPlugin(PluginContextFactory.Instance.GetPluginContext());
        }
        ~PlugProvider() => TerminatePlugin();

        public IComponent GetComponent()
        {
            if (component == null) SetupPlugin(PluginContextFactory.Instance.GetPluginContext());
            //if (component != null) component.AddRef();
            return component;
        }

        public IEditController GetController()
        {
            //if (controller != null) controller.AddRef();
            // 'iController == 0' is allowed! In this case the plug has no controller
            return controller;
        }
        public int ReleasePlugIn(IComponent iComponent, IEditController iController)
        {
            if (iComponent != null) Marshal.ReleaseComObject(iComponent);
            if (iController != null) Marshal.ReleaseComObject(iController);

            if (!plugIsGlobal) TerminatePlugin();
            return TResult.S_OK;
        }

        public int GetSubCategories(IStringResult result) { result.SetText(classInfo.SubCategoriesString()); return TResult.S_True; }

        public int GetComponentUID(out Guid uid)
        {
            uid = classInfo.ID;
            return TResult.S_OK;
        }

        //--- from ITestPlugProvider2 ------------------
        public IPluginFactory GetPluginFactory()
            => factory.Get();

        //OBJ_METHODS(PlugProvider, FObject)
        //REFCOUNT_METHODS(FObject)
        //DEF_INTERFACES_2(ITestPlugProvider, ITestPlugProvider2, FObject)

        protected bool SetupPlugin(object hostContext)
        {
            var res = false;

            //---create Plug-in here!--------------
            // create its component part
            component = factory.CreateInstance<IComponent>(classInfo.ID);
            if (component != null)
            {
                // initialize the component with our context
                res = component.Initialize(hostContext) == TResult.S_OK;

                //:SKY
                // try to create the controller part from the component (for Plug-ins which did not succeed to separate component from controller)
                //Marshal.QueryInterface(component, ref interfaceId, out instance);
                //if (component.QueryInterface(IEditController.iid, (void**)&controller) != TResult.S_True)
                //{
                //    // ask for the associated controller class ID
                //    Guid controllerCID = new();
                //    if (component.GetControllerClassId(ref controllerCID) == TResult.S_True)
                //    {
                //        // create its controller part created from the factory
                //        controller = factory.CreateInstance<IEditController>(controllerCID);
                //        // initialize the component with our context
                //        if (controller != null) res = controller.Initialize(hostContext) == TResult.S_OK;
                //    }
                //}
            }
            else Console.Out?.Write($"Failed to create instance of {classInfo.Name}!\n");

            if (res) ConnectComponents();

            return res;
        }

        protected bool ConnectComponents()
        {
            if (component == null || controller == null) return false;

            var compICP = component as IConnectionPoint;
            var contrICP = controller as IConnectionPoint;
            if (compICP == null || contrICP == null) return false;

            var res = false;

            componentCP = new ConnectionProxy(compICP);
            controllerCP = new ConnectionProxy(contrICP);

            if (componentCP.Connect(contrICP) != TResult.S_True) { } // TODO: Alert or what for non conformant plugin ?
            else
            {
                if (controllerCP.Connect(compICP) != TResult.S_True) { } // TODO: Alert or what for non conformant plugin ?
                else res = true;
            }
            return res;
        }

        protected bool DisconnectComponents()
        {
            if (componentCP != null || controllerCP != null) return false;

            var res = componentCP.Disconnect();
            res &= controllerCP.Disconnect();

            componentCP = null;
            controllerCP = null;

            return res;
        }

        protected void TerminatePlugin()
        {
            DisconnectComponents();

            var controllerIsComponent = false;
            if (component != null)
            {
                controllerIsComponent = component is IEditController;
                component.Terminate();
            }

            if (controller != null && controllerIsComponent == false) controller.Terminate();

            component = null;
            controller = null;
        }
    }

    public class PluginContextFactory
    {
        public static readonly PluginContextFactory Instance = new();
        object context;

        public void SetPluginContext(object obj) => context = obj;
        public object GetPluginContext() => context;
    }
}
