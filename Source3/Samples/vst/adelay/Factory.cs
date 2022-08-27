namespace Jacobi.Vst3
{
    public partial class Factory : PluginClassFactory
    {
        public Factory()
            : base(stringCompanyName, stringCompanyWeb, stringCompanyEmail, (PFactoryInfo.FactoryFlags)3)
            => RegisterClasses();

        void RegisterClasses()
        {
            //var reg = Register(typeof(ADelayIds), ClassRegistration.ObjectClasses.AudioModuleClass);
            //reg.Categories = new PlugType(PlugType.Fx);

            //Register(typeof(MyEditController), ClassRegistration.ObjectClasses.ComponentControllerClass);
        }
    }
}
