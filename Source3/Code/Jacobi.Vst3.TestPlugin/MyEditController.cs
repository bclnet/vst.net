using Jacobi.Vst3.Core;
using Jacobi.Vst3.Plugin;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.TestPlugin
{
    [DisplayName("My Edit Controller"), Guid("D74D670B-28B8-4AB2-9180-D4D12B52F54B"), ClassInterface(ClassInterfaceType.None)]
    public class MyEditController : EditControllerEx1
    {
        public MyEditController()
        {
            RootUnit = new RootUnit("Root", null);
            Units.Add(RootUnit);

            Parameters.Add(new ByPassParameter(RootUnit.Info.Id, 1));
            Parameters.Add(new GainParameter(RootUnit.Info.Id, 2));
        }
    }

    public class GainParameter : Parameter
    {
        public GainParameter(int unitId, uint paramId)
            => ValueInfo = new ParameterValueInfo
            {
                ParameterInfo = new ParameterInfo
                {
                    DefaultNormalizedValue = 0.45,
                    Flags = ParameterInfo.ParameterFlags.CanAutomate,
                    Id = paramId,
                    ShortTitle = "Gain",
                    StepCount = 0,
                    Title = "Gain",
                    UnitId = unitId,
                    Units = "dB",
                },
                MinValue = 0.0,
                MaxValue = 10.0,
                Precision = 2
            };
    }
}
