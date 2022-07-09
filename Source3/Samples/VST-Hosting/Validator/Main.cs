using Jacobi.Vst3.Utility;
using System.Threading.Tasks;
using static Jacobi.Vst3.Core.ModuleInit;

namespace Steinberg.Vst
{
    internal class MainX
    {
        static async Task<int> Main(string[] args)
        {
            VersionParserTest.Touch();
            InitModule();
            var result = await new Validator(args).Run();
            DeinitModule();
            return result;
        }
    }
}
