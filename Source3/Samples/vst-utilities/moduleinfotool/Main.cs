using System.Threading.Tasks;

namespace Steinberg.Vst
{
    internal class MainX
    {
        static async Task<int> Main(string[] args)
        {
            args = new[] { "create", "version", "1" };
            //args = new[] { "list" };

            var result = await new ModuleInfoTool(args).Run();
            return result;
        }
    }
}
