using Jacobi.Vst3.Core;
using Jacobi.Vst3.Plugin;

namespace Jacobi.Vst3.TestPlugin
{
    public class PluginFactory : PluginClassFactory
    {
        public PluginFactory()
            : base("Jacobi Software", "obiwanjacobi@hotmail.com", "https://github.com/obiwanjacobi/vst.net")
            => RegisterClasses();

        void RegisterClasses()
        {
            var reg = Register(typeof(PluginComponent), ClassRegistration.ObjectClasses.AudioModuleClass);
            reg.Categories = new PlugType(PlugType.Fx);

            Register(typeof(MyEditController), ClassRegistration.ObjectClasses.ComponentControllerClass);
        }
    }
}
