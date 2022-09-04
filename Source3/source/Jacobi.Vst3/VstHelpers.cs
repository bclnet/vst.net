using System.Text;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3
{
    unsafe static partial class Helpers
    {
        /* Retrieve from a IBStream the state type, here the StateType::kProject
	    * return kResultTrue if the state is coming from a project,
	    * return kResultFalse if the state is coming from a preset,
	    * return kNotImplemented if the host does not implement such feature
        */
        public static TResult IsProjectState(this IBStream state)
        {
            if (state == null)
                return kInvalidArgument;

            if (state is not IStreamAttributes stream)
                return kNotImplemented;

            var list = stream.GetAttributes();
            if (list != null)
            {
                // get the current type (project/Default..) of this state
                StringBuilder str = new();
                if (list.GetString(PresetAttributes.StateType, str, 128 * sizeof(char)) == kResultTrue)
                {
                    var ascii = str.ToString();
                    return ascii == StateType.Project ? kResultTrue : kResultFalse;
                }
            }
            return kNotImplemented;
        }
    }
}
