using System.Threading.Tasks;

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
            //args = new[] { "-validate", "-path", vst3paths[3] };
            args = new[] { "-create", "-version", "1", "-path", vst3paths[3] };

            var result = await new ModuleInfoTool(args).Run();
            return result;
        }
    }
}
