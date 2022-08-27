using Jacobi.Vst3.Plugin;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    [Guid("9FC98F39-2723-4512-84FB-C4AD618A14FD")]
    public interface IDelayTestController
    {
        bool DoTest();
    }

    [DisplayName("A Delay Controller"), ClassInterface(ClassInterfaceType.None)]
    public class ADelayController : EditController, IDelayTestController
    {
        public bool DoTest()
        {
            throw new NotImplementedException();
        }
    }
}
