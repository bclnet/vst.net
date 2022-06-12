using Daw.Vst;

namespace Daw.Core
{
    public class Option
    {
        public Vst2Option Vst2;
        public Vst3Option Vst3;

        public static Option Default => new()
        {
            Vst2 = Vst2Option.Default,
            Vst3 = Vst3Option.Default,
        };
    }
}
