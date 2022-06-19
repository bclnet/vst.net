using System;

namespace Jacobi.Vst3.Plugin
{
    public class RangeParameter : Parameter
    {
        public RangeParameter(ParameterValueInfo paramValueInfo)
            : base(paramValueInfo) { }

        public override double ToNormalized(double plainValue)
            => ValueInfo.ParameterInfo.StepCount > 1
                ? (plainValue - ValueInfo.MinValue) / ValueInfo.ParameterInfo.StepCount
                : base.ToNormalized(plainValue);

        public override double ToPlain(double normValue)
            => ValueInfo.ParameterInfo.StepCount > 1
                ? Math.Min(ValueInfo.ParameterInfo.StepCount, (normValue * (ValueInfo.ParameterInfo.StepCount + 1)) + ValueInfo.MinValue)
                : base.ToPlain(normValue);
    }
}
