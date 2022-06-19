using System.Threading.Tasks;

namespace Steinberg.Vst
{
    internal class MainX
    {
        static async Task<int> Main(string[] args)
        {
            //InitModule(args);
            var result = await new Validator(args).Run();
            //DeinitModule();
            return result;
        }
    }
}
