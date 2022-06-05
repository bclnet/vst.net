using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Host
{
    public class PlugProvider
    {
        public PlugProvider(PluginFactory factory, ClassInfo classInfo, bool plugIsGlobal)
        {

        }

        public IComponent GetComponent() => throw new NotImplementedException();
        public IEditController GetController() => throw new NotImplementedException();
    }
}
