using Jacobi.Vst3.Utility;
using System.Threading.Tasks;
using static Jacobi.Vst3.Core.ModuleInit;

namespace Steinberg.Vst
{
    internal class MainX
    {
        static async Task<int> Main(string[] args)
        {
            //args = new[] { "version" };
            //args = new[] { "list" };
            //args = new[] { "snapshots" };
            //args = new[] { "selftest" };
            args = new[] { "f", "C:\\Program Files\\Common Files\\VST3\\iZotope\\Nectar 3 Elements.vst3" };
            //args = new[] { "l", "e", "f", "C:\\Program Files\\Common Files\\VST3\\iZotope\\Nectar 3 Elements.vst3" };
            VersionParserTest.Touch();

            InitModule();
            var result = await new Validator(args).Run();
            DeinitModule();
            return result;
        }
    }
}
