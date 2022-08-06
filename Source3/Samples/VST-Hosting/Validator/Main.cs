using Jacobi.Vst3.Utility;
using System.Threading.Tasks;
using static Jacobi.Vst3.Core.ModuleInit;

namespace Steinberg.Vst
{
    internal class MainX
    {
        static async Task<int> Main(string[] args)
        {
            var files = new string[]
            {
                @"C:\Program Files\Common Files\VST3\iZotope\Nectar 3 Elements.vst3",
                @"C:\Program Files\Common Files\VST3\iZotope\Neutron 3 Elements.vst3",
                @"C:\Program Files\Common Files\VST3\iZotope\Ozone 9 Elements.vst3",
                @"C:\Program Files\Common Files\VST3\Reason Rack Plugin.vst3\Contents\x86_64-win\Reason Rack Plugin.vst3",
            };

            //args = new[] { "version" };
            //args = new[] { "list" };
            //args = new[] { "snapshots" };
            //args = new[] { "selftest" };
            args = new[] { "f", files[1] };
            //args = new[] { "l", "e", "f", files[0] };
            VersionParserTest.Touch();

            InitModule();
            var result = await new Validator(args).Run();
            DeinitModule();
            return result;
        }
    }
}
