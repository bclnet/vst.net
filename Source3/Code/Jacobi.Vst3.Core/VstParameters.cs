using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;

namespace Jacobi.Vst3.Plugin
{
    public class Parameter : ObservableObject
    {
        protected Parameter() { }

        public Parameter(ParameterValueInfo paramValueInfo)
        {
            ValueInfo = paramValueInfo;

            SetValue(ValueInfo.ParameterInfo.DefaultNormalizedValue, null, false);
        }

        public ParameterValueInfo ValueInfo { get; protected set; }

        public uint Id => ValueInfo.ParameterInfo.Id;

        public bool IsReadOnly { get; set; }

        public bool ResetToDefaultValue() => SetValue(ValueInfo.ParameterInfo.DefaultNormalizedValue, null, true);

        protected bool SetValue(double? normValue, double? plainValue, bool notify)
        {
            if (IsReadOnly) return false;

            var normChanged = false;
            var plainChanged = false;

            if (normValue.HasValue)
            {
                normChanged = SetNormalizedValue(normValue.Value);
                plainChanged = SetPlainValue(ToPlain(normValue.Value));
            }

            if (plainValue.HasValue)
            {
                normChanged = SetNormalizedValue(ToNormalized(plainValue.Value));
                plainChanged = SetPlainValue(plainValue.Value);
            }

            if (normChanged && notify) OnPropertyChanged(nameof(NormalizedValue));
            if (plainChanged && notify) OnPropertyChanged(nameof(PlainValue));

            return normChanged || plainChanged;
        }

        double _normalizedValue;

        public double NormalizedValue
        {
            get => _normalizedValue;
            set => SetValue(value, null, true);
        }

        bool SetNormalizedValue(double value)
        {
            value = value.Clamp(0.0, 1.0);

            if (_normalizedValue != value)
            {
                _normalizedValue = value;
                return true;
            }

            return false;
        }

        double _plainValue;

        public double PlainValue
        {
            get => _plainValue;
            set => SetValue(null, value, true);
        }

        bool SetPlainValue(double value)
        {
            value = value.Clamp(ValueInfo.MinValue, ValueInfo.MaxValue);

            if (_plainValue != value)
            {
                _plainValue = value;
                return true;
            }

            return false;
        }

        public virtual double ToNormalized(double plainValue)
        {
            plainValue = plainValue.Clamp(ValueInfo.MinValue, ValueInfo.MaxValue);

            var scale = ValueInfo.MaxValue - ValueInfo.MinValue;
            var offset = -ValueInfo.MinValue;

            return (plainValue + offset) / scale;
        }

        public virtual double ToPlain(double normValue)
        {
            normValue = normValue.Clamp(0.0, 1.0);

            var scale = ValueInfo.MaxValue - ValueInfo.MinValue;
            var offset = -ValueInfo.MinValue;

            return (normValue * scale) - offset;
        }

        public virtual string ToString(double normValue)
        {
            if (ValueInfo.ParameterInfo.StepCount == 1)
                return normValue >= 0.5 ? "On" : "Off";

            var value = ToPlain(normValue);

            if (ValueInfo.Precision > 0)
            {
                var format = new string('#', ValueInfo.Precision);
                return value.ToString("0." + format);
            }

            return value.ToString();
        }

        public virtual bool TryParse(string displayValue, out double normValue)
            => double.TryParse(displayValue, out normValue);
    }

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

    public class ListParameter<T> : Parameter
    {
        public ListParameter(ParameterValueInfo paramValueInfo)
            : base(paramValueInfo)
        {
            if ((paramValueInfo.ParameterInfo.Flags & Core.ParameterInfo.ParameterFlags.IsList) == 0)
                throw new ArgumentException("The specified ParameterInfo has no IsList flag.", nameof(paramValueInfo));
            Values = new List<T>();
        }

        public List<T> Values { get; protected set; }

        public override string ToString(double normValue)
        {
            var index = (int)ToPlain(normValue);
            return index >= 0 && index < Values.Count
                ? ConvertToString(Values[index])
                : String.Empty;
        }

        protected virtual string ConvertToString(T value)
            => value.ToString();

        public override bool TryParse(string displayValue, out double normValue)
        {
            var index = 0;

            foreach (var val in Values)
            {
                var str = ConvertToString(val);

                if (displayValue.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    normValue = ToNormalized(index);
                    return true;
                }

                index++;
            }

            normValue = 0.0;
            return false;
        }
    }

    public class ParameterCollection : KeyedCollectionWithIndex<UInt32, Parameter>
    {
        protected override uint GetKeyForItem(Parameter item)
        {
            if (item == null) return 0;
            return item.Id;
        }
    }
}
