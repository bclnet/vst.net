using Daw.Core;
using Daw.Vst;

namespace Daw
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OptionManager.Load();
            var repo = new Vst3Repository();
            repo.Refresh();
        }
    }
}
