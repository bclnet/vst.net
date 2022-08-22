using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.Hosting
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

            if (plugIsGlobal)
                SetupPlugin(PluginContextFactory.Instance.GetPluginContext());
        }
        ~PlugProvider()
            => TerminatePlugin();

        public IComponent GetComponent()
        {
            if (component == null)
                SetupPlugin(PluginContextFactory.Instance.GetPluginContext());

            //if (component != null)
            //    Console.WriteLine($"getComponent:{Marshal.AddRef(componentHandle)}");

            return component;
        }

        public IEditController GetController()
        {
            //if (controller != null)
            //    Console.WriteLine($"getController:{Marshal.AddRef(controllerHandle)}");

            // 'controller == null' is allowed! In this case the plug has no controller
            return controller;
        }

        public TResult ReleasePlugIn(IComponent component, IEditController controller)
        {
            if (component != null)
                Platform.ReleaseRcw(ref component);

            if (controller != null)
                Platform.ReleaseRcw(ref controller);

            if (!plugIsGlobal)
                TerminatePlugin();

            return kResultOk;
        }

        public TResult GetSubCategories(IStringResult result) { result.SetText(classInfo.SubCategoriesString()); return kResultTrue; }

        public TResult GetComponentUID(out Guid uid)
        {
            uid = classInfo.ID;
            return kResultOk;
        }

        //--- from ITestPlugProvider2 ------------------
        public IPluginFactory GetPluginFactory()
            => factory.Get();

        protected bool SetupPlugin(object hostContext)
        {
            var res = false;

            //---create Plug-in here!--------------
            // create its component part
            component = factory.CreateInstance<IComponent>(classInfo.ID);
            if (component != null)
            {
                // initialize the component with our context
                res = component.Initialize(hostContext) == kResultOk;

                // try to create the controller part from the component (for Plug-ins which did not succeed to separate component from controller)
                if (component is not IEditController)
                    // ask for the associated controller class ID
                    if (component.GetControllerClassId(out var controllerCID) == kResultTrue)
                    {
                        // create its controller part created from the factory
                        controller = factory.CreateInstance<IEditController>(controllerCID);
                        // initialize the component with our context
                        if (controller != null) res = controller.Initialize(hostContext) == kResultOk;
                    }
            }
            else Console.Out?.Write($"Failed to create instance of {classInfo.Name}!\n");

            if (res) ConnectComponents();

            return res;
        }

        protected bool ConnectComponents()
        {
            if (component == null || controller == null) return false;

            if (component is not IConnectionPoint compICP || controller is not IConnectionPoint contrICP) return false;

            var res = false;

            componentCP = new ConnectionProxy(compICP);
            controllerCP = new ConnectionProxy(contrICP);

            if (componentCP.Connect(contrICP) != kResultTrue) { } // TODO: Alert or what for non conformant plugin ?
            else
            {
                if (controllerCP.Connect(compICP) != kResultTrue) { } // TODO: Alert or what for non conformant plugin ?
                else res = true;
            }
            return res;
        }

        protected bool DisconnectComponents()
        {
            if (componentCP == null || controllerCP == null) return false;

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
