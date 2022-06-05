using Jacobi.Vst3.Core.Common;
using Jacobi.Vst3.Host;
using Jacobi.Vst3.Plugin;
using System;
using System.Linq;

namespace AudioHost
{
    internal class Program
    {
        //static IUnknown gStandardPluginContext = new Jacobi.Vst3.HostApplication();

        static Module _module;
        static PlugProvider _plugProvider;
        //static AudioClient _vst3Processor;

        static void startAudioClient(string path, Guid? effectID, uint flags)
        {
            _module = Module.Create(path, out var error);
            if (_module == null)
            {
                var reason = $"Could not create Module for file:{path}\nError: {error}";
                // EditorHost::IPlatform::instance().kill (-1, reason);
                return;
            }
            var factory = _module.Factory;
            foreach (var classInfo in factory.ClassInfos)
            {
                if (classInfo.Category == PluginClassFactory.AudioModuleClassCategory)
                {
                    if (effectID != null && effectID.Value != classInfo.ID) continue;
                    _plugProvider = new PlugProvider(factory, classInfo, true);
                    break;
                }
            }
            if (_plugProvider == null)
            {
                var error2 = effectID != null
                    ? $"No VST3 Audio Module Class with UID {effectID} found in file {path}"
                    : $"No VST3 Audio Module Class found in file {path}";
                // EditorHost::IPlatform::instance().kill (-1, error);
                return;
            }

            var component = _plugProvider.GetComponent();
            var controller = _plugProvider.GetController();
            //midiMapping = new FUnknownPtr<IMidiMapping>(controller);

            //! TODO: Query the plugProvider for a proper name which gets displayed in JACK.
            //_vst3Processor = AudioClient.Create("VST 3 SDK", component, midiMapping);
        }

        static void Init(string[] cmdArgs)
        {
            if (cmdArgs.Length == 0)
            {
                //var helpText = @"(
                //    usage: AudioHost pluginPath
                //)";
                return;
            }

            var uid = (Guid?)null;
            var flags = 0U;
            startAudioClient(cmdArgs.Last(), uid, flags);
        }

        void Terminate()
        {
        }

        static void Main(string[] args)
        {
            Init(args);
            Console.WriteLine("Press <enter> to continue . . .");
            Console.ReadKey();
        }
    }
}
