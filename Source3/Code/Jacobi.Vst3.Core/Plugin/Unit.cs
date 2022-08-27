using Jacobi.Vst3.Core;

namespace Jacobi.Vst3.Plugin
{
    

    public class RootUnit : Unit
    {
        public RootUnit(string name, ProgramList programList)
            : base(UnitInfo.RootUnitId, name, null, programList) { }
    }
}
