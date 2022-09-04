using Steinberg.Vst3.Utility;
using System.Threading.Tasks;
using static Steinberg.Vst3.ModuleInit;

namespace Steinberg.Vst
{
    internal class MainX
    {
        static readonly string[] vst3paths = new[]
        {
            @"C:\Program Files (x86)\Common Files\VST3\iZotope\Nectar 3 Elements.vst3",
            @"C:\_GITHUB\bclnet\vst.net\Source3\source\x64\Debug\Jacobi.Vst3.TestPlugin.vst3",
            @"C:\Program Files\Common Files\VST3\iZotope\Nectar 3 Elements.vst3",
            @"C:\Program Files\Common Files\VST3\iZotope\Neutron 3 Elements.vst3",
            @"C:\Program Files\Common Files\VST3\iZotope\Ozone 9 Elements.vst3",
            @"C:\Program Files\Common Files\VST3\Reason Rack Plugin.vst3\Contents\x86_64-win\Reason Rack Plugin.vst3",
        };

        static async Task<int> Main(string[] args)
        {
            //args = new[] { "version" };
            //args = new[] { "list" };
            //args = new[] { "snapshots" };
            //args = new[] { "selftest" };
            args = new[] { "f", vst3paths[1] };
            //args = new[] { "l", "e", "f", vst3paths[0] };
            VersionParserTest.Touch();

            InitModule();
            var result = await new Validator(args).Run();
            DeinitModule();
            return result;
        }
    }
}
