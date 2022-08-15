using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.Hosting
{
    /// <summary>
    /// Example implementation of IPlugInterfaceSupport.
    /// </summary>
    public class PlugInterfaceSupport : IPlugInterfaceSupport
    {
        List<Guid> _FUIDArray = new();

        public PlugInterfaceSupport()
        {
            // add minimum set

            //---VST 3.0.0--------------------------------
            AddPlugInterfaceSupported(Interfaces.IComponent);
            AddPlugInterfaceSupported(Interfaces.IAudioProcessor);
            AddPlugInterfaceSupported(Interfaces.IEditController);
            AddPlugInterfaceSupported(Interfaces.IConnectionPoint);

            AddPlugInterfaceSupported(Interfaces.IUnitInfo);
            AddPlugInterfaceSupported(Interfaces.IUnitData);
            AddPlugInterfaceSupported(Interfaces.IProgramListData);

            //---VST 3.0.1--------------------------------
            AddPlugInterfaceSupported(Interfaces.IMidiMapping);

            //---VST 3.1----------------------------------
            AddPlugInterfaceSupported(Interfaces.IEditController2);

            /*
            //---VST 3.0.2--------------------------------
            AddPlugInterfaceSupported(Interfaces.IParameterFinder);

            //---VST 3.1----------------------------------
            AddPlugInterfaceSupported(Interfaces.IAudioPresentationLatency);

            //---VST 3.5----------------------------------
            AddPlugInterfaceSupported(Interfaces.IKeyswitchController);
            AddPlugInterfaceSupported(Interfaces.IContextMenuTarget);
            AddPlugInterfaceSupported(Interfaces.IEditControllerHostEditing);
            AddPlugInterfaceSupported(Interfaces.IXmlRepresentationController);
            AddPlugInterfaceSupported(Interfaces.INoteExpressionController);

            //---VST 3.6.5--------------------------------
            AddPlugInterfaceSupported(Interfaces.ChannelContext.IInfoListener);
            AddPlugInterfaceSupported(Interfaces.IPrefetchableSupport);
            AddPlugInterfaceSupported(Interfaces.IAutomationStateiid);

            //---VST 3.6.11--------------------------------
            AddPlugInterfaceSupported(Interfaces.INoteExpressionPhysicalUIMapping);

            //---VST 3.6.12--------------------------------
            AddPlugInterfaceSupported(Interfaces.IMidiLearn);

            //---VST 3.7-----------------------------------
            AddPlugInterfaceSupported(Interfaces.IProcessContextRequirements);
            AddPlugInterfaceSupported(Interfaces.IParameterFunctionName);
            AddPlugInterfaceSupported(Interfaces.IProgress);
            */
        }

        public TResult IsPlugInterfaceSupported(Guid iid) => _FUIDArray.Contains(iid) ? kResultTrue : kResultFalse;
        public void AddPlugInterfaceSupported(string iid) => _FUIDArray.Add(Guid.Parse(iid));
        public bool RemovePlugInterfaceSupported(string iid)
        {
            var uid = Guid.Parse(iid);
            var it_ = _FUIDArray.Find(x => x == uid);
            if (it_ == default) return false;
            _FUIDArray.Remove(it_);
            return true;
        }
    }
}
