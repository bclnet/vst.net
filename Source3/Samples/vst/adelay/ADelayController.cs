using Steinberg.Vst3;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static Steinberg.Tags;
using static Steinberg.Vst3.TResult;

namespace Steinberg
{
    [Guid("9FC98F39-2723-4512-84FB-C4AD618A14FD")]
    public interface IDelayTestController
    {
        bool DoTest();
    }

    [DisplayName("A Delay Controller"), ClassInterface(ClassInterfaceType.None)]
    public class ADelayController : EditController, IDelayTestController
    {
        /// <summary>
        /// create function required for plug-in factory, it will be called to create new instances of this controller
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        static object CreateInstance(object _) => (IEditController)new ADelayController();

        #region IPluginBase

        public override TResult Initialize(object context)
        {
            var result = base.Initialize(context);
            if (result == kResultTrue)
            {
                parameters.AddParameter("Bypass", null, 1, 0, ParameterFlags.CanAutomate | ParameterFlags.IsBypass, (int)kBypassId);

                parameters.AddParameter("Delay", "sec", 0, 1, ParameterFlags.CanAutomate, (int)kDelayId);
            }
            return kResultTrue;
        }

        #endregion

        #region EditController

#if TARGET_OS_IPHONE
        public override IPlugView CreateView(string name)
            => name == ViewType.Editor
                ? new ADelayEditorForIOS(this)
                : null;
#endif

        public override TResult SetComponentState(IBStream state)
        {
            // we receive the current state of the component (processor part)
            // we read only the gain and bypass value...
            if (state == null)
                return kResultFalse;

            var streamer = new IBStreamer(state, kLittleEndian);
            if (!streamer.ReadFloat(out var savedDelay))
                return kResultFalse;
            SetParamNormalized((uint)kDelayId, savedDelay);

            if (!streamer.ReadInt32(out var bypassState)) { } // could be an old version, continue 
            SetParamNormalized(kBypassId, bypassState ? 1 : 0);

            return kResultOk;
        }

        #endregion

        public bool DoTest()
        {
            // this is called when running thru the validator
            // we can now run our own test cases
            return true;
        }
    }
}
