using System.Collections.Generic;
using ParamID = System.UInt32;
using ParamValue = System.Double;
using UnitID = System.Int32;

namespace Steinberg.Vst3
{
    // Description of a Parameter.
    public class Parameter : ObservableObject
    {
        protected ParameterInfo info;
        protected ParamValue valueNormalized;
        protected int precision = 4;

        protected Parameter() { }
        public Parameter(ParameterInfo info)
        {
            this.info = info;
            this.valueNormalized = info.DefaultNormalizedValue;
        }
        public Parameter(string title, ParamID tag, string units,
            ParamValue defaultValueNormalized, int stepCount, ParameterFlags flags,
            UnitID unitID, string shortTitle)
        {
            info.Title = title;
            if (units != null)
                info.Units = units;
            if (shortTitle != null)
                info.ShortTitle = shortTitle;

            info.StepCount = stepCount;
            info.DefaultNormalizedValue = valueNormalized = defaultValueNormalized;
            info.Flags = flags;
            info.Id = tag;
            info.UnitId = unitID;
        }

        /// <summary>
        /// Gets the info.
        /// </summary>
        public virtual ParameterInfo Info => info;

        /// <summary>
        /// Gets or sets its associated UnitId.
        /// </summary>
        public virtual UnitID UnitID
        {
            get => info.UnitId;
            set => info.UnitId = value;
        }

        /// <summary>
        /// Gets its normalized value [0.0, 1.0].
        /// </summary>
        public ParamValue Normalized
            => valueNormalized;

        /// Sets its normalized value [0.0, 1.0].
        public virtual bool SetNormalized(ParamValue v)
        {
            if (v > 1.0)
                v = 1.0;
            else if (v < 0.0)
                v = 0.0;

            if (v != valueNormalized)
            {
                valueNormalized = v;
                OnPropertyChanged(nameof(Normalized));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts a normalized value to a string.
        /// </summary>
        /// <param name="normValue"></param>
        /// <returns></returns>
        public virtual string ToString(ParamValue normValue)
        {
            if (info.StepCount == 1)
                return normValue > 0.5
                    ? "On"
                    : "Off";
            else
                return normValue.ToString($"0.{new string('#', precision)}");
        }

        /// <summary>
        /// Converts a string to a normalized value.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="normValue"></param>
        /// <returns></returns>
        public virtual bool FromString(string str, out ParamValue normValue)
            => ParamValue.TryParse(str, out normValue);

        /// <summary>
        /// Converts a normalized value to plain value (e.g. 0.5 to 10000.0Hz).
        /// </summary>
        /// <param name="valueNormalized"></param>
        /// <returns></returns>
        public virtual ParamValue ToPlain(ParamValue valueNormalized)
            => valueNormalized;

        /// <summary>
        /// Converts a plain value to a normalized value (e.g. 10000 to 0.5).
        /// </summary>
        /// <param name="plainValue"></param>
        /// <returns></returns>
        public virtual ParamValue ToNormalized(ParamValue plainValue)
            => plainValue;

        /// <summary>
        /// Gets or sets the current precision (used for string representation of float), (for example 4.34 with 2 as precision).
        /// </summary>
        /// <returns></returns>
        public int Precision
        {
            get => precision;
            set => precision = value;
        }
    }

    /// <summary>
    /// Description of a RangeParameter.
    /// </summary>
    public class RangeParameter : Parameter
    {
        protected ParamValue minPlain;
        protected ParamValue maxPlain;

        public RangeParameter(ParameterInfo paramInfo, ParamValue min, ParamValue max)
            : base(paramInfo)
        {
            minPlain = min;
            maxPlain = max;
        }

        public RangeParameter(string title, ParamID tag, string units = null,
            ParamValue minPlain = 0.0, ParamValue maxPlain = 1.0,
            ParamValue defaultValuePlain = 0.0, int stepCount = 0,
            ParameterFlags flags = ParameterFlags.CanAutomate, UnitID unitID = UnitInfo.RootUnitId,
            string shortTitle = null)
        : base()
        {
            info.Title = title;
            if (units != null)
                info.Units = units;
            if (shortTitle != null)
                info.ShortTitle = shortTitle;

            info.StepCount = stepCount;
            info.DefaultNormalizedValue = valueNormalized = ToNormalized(defaultValuePlain);
            info.Flags = flags;
            info.Id = tag;
            info.UnitId = unitID;
        }

        /// <summary>
        /// Gets or sets the minimum plain value, same as toPlain (0).
        /// </summary>
        public virtual ParamValue Min
        {
            get => minPlain;
            set => minPlain = value;
        }

        /// <summary>
        /// Gets or sets the maximum plain value, same as toPlain (1).
        /// </summary>
        /// <returns></returns>
        public virtual ParamValue Max
        {
            get => maxPlain;
            set => maxPlain = value;
        }

        /// <summary>
        /// Converts a normalized value to a string.
        /// </summary>
        /// <param name="_valueNormalized"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public override string ToString(ParamValue valueNormalized)
        {
            if (info.StepCount > 1)
            {
                var plain = (long)ToPlain(valueNormalized);
                return plain.ToString();
            }
            else
                return base.ToString(ToPlain(valueNormalized));
        }

        /// <summary>
        /// Converts a string to a normalized value.
        /// </summary>
        /// <returns></returns>
        public override bool FromString(string str, out ParamValue valueNormalized)
        {
            if (info.StepCount > 1)
            {
                if (long.TryParse(str, out var plainValue))
                {
                    valueNormalized = ToNormalized((ParamValue)plainValue);
                    return true;
                }
                valueNormalized = default;
                return false;
            }
            if (double.TryParse(str, out valueNormalized))
            {
                if (valueNormalized < Min)
                    valueNormalized = Min;
                else if (valueNormalized > Max)
                    valueNormalized = Max;
                valueNormalized = ToNormalized(valueNormalized);
                return true;
            }
            valueNormalized = default;
            return false;
        }

        /// <summary>
        /// Converts a normalized value to plain value (e.g. 0.5 to 10000.0Hz).
        /// </summary>
        /// <param name="_valueNormalized"></param>
        /// <returns></returns>
        public override ParamValue ToPlain(ParamValue valueNormalized)
        {
            if (info.StepCount > 1)
                return FUtils.FromNormalized<ParamValue>(valueNormalized, info.StepCount) + Min;
            return valueNormalized * (Max - Min) + Min;
        }

        /// <summary>
        /// Converts a plain value to a normalized value (e.g. 10000 to 0.5).
        /// </summary>
        /// <param name="plainValue"></param>
        /// <returns></returns>
        public override ParamValue ToNormalized(ParamValue plainValue)
        {
            if (info.StepCount > 1)
                return FUtils.ToNormalized<ParamValue>(plainValue - Min, info.StepCount);
            return (plainValue - Min) / (Max - Min);
        }
    }

    /// <summary>
    /// Description of a StringListParameter.
    /// </summary>
    public class StringListParameter : Parameter
    {
        protected List<string> strings = new();

        public StringListParameter(ParameterInfo paramInfo)
            : base(paramInfo) { }

        public StringListParameter(string title, ParamID tag, string units = null,
            ParameterFlags flags = ParameterFlags.CanAutomate | ParameterFlags.IsList,
            UnitID unitID = UnitInfo.RootUnitId, string shortTitle = null)
        {
            info.Title = title;
            if (units != null)
                info.Units = units;
            if (shortTitle != null)
                info.ShortTitle = shortTitle;

            info.StepCount = -1;
            info.DefaultNormalizedValue = 0;
            info.Flags = flags;
            info.Id = tag;
            info.UnitId = unitID;
        }

        /// <summary>
        /// Appends a string and increases the stepCount.
        /// </summary>
        public virtual void AppendString(string str)
        {
            strings.Add(str);
            info.StepCount++;
        }

        /// <summary>
        /// Replaces the string at index. Index must be between 0 and stepCount+1
        /// </summary>
        /// <param name="index"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public virtual bool ReplaceString(int index, string str)
        {
            var str2 = strings[index];
            if (str2 == null)
                return false;

            strings[index] = str;
            return true;
        }

        /// <summary>
        /// Converts a normalized value to a string.
        /// </summary>
        /// <param name="valueNormalized"></param>
        /// <param name="str"></param>
        public override string ToString(ParamValue valueNormalized)
        {
            var index = (int)ToPlain(valueNormalized);
            var valueString = strings[index];
            return valueString ?? string.Empty;
        }

        /// <summary>
        /// Converts a string to a normalized value.
        /// </summary>
        /// <returns></returns>
        public override bool FromString(string str, out ParamValue valueNormalized)
        {
            var index = 0;
            foreach (var it in strings)
            {
                if (it == str)
                {
                    valueNormalized = ToNormalized((ParamValue)index);
                    return true;
                }
                ++index;
            }
            valueNormalized = default;
            return false;
        }

        /// <summary>
        /// Converts a normalized value to plain value (e.g. 0.5 to 10000.0Hz).
        /// </summary>
        /// <param name="valueNormalized"></param>
        /// <returns></returns>
        public override ParamValue ToPlain(ParamValue valueNormalized)
            => info.StepCount <= 0
                ? 0
                : FUtils.FromNormalized<ParamValue>(valueNormalized, info.StepCount);

        /// <summary>
        /// Converts a plain value to a normalized value (e.g. 10000 to 0.5).
        /// </summary>
        /// <param name="plainValue"></param>
        /// <returns></returns>
        public override ParamValue ToNormalized(ParamValue plainValue)
            => info.StepCount <= 0
                ? 0
                : FUtils.ToNormalized<ParamValue>(plainValue, info.StepCount);
    }

    /// <summary>
    /// Collection of parameters.
    /// </summary>
    public class ParameterContainer
    {
        protected List<Parameter> parms;
        protected Dictionary<ParamID, int> id2index = new();

        /// <summary>
        /// Init param array.
        /// </summary>
        /// <param name="initialSize"></param>
        /// <param name="resizeDelta"></param>
        public void Init(int initialSize = 10, int resizeDelta = 100)
        {
            if (parms != null)
            {
                parms = new List<Parameter>();
                if (initialSize > 0)
                    parms.Capacity = initialSize;
            }
        }

        /// <summary>
        /// Adds a given parameter.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Parameter AddParameter(Parameter p)
        {
            if (parms == null)
                Init();
            id2index[p.Info.Id] = parms.Count;
            parms.Add(p);
            return p;
        }

        /// <summary>
        /// Creates and adds a new parameter from a ParameterInfo.
        /// </summary>
        /// <returns></returns>
        public Parameter AddParameter(ref ParameterInfo info)
        {
            if (parms == null)
                Init();
            var p = new Parameter(info);
            return AddParameter(p) != null ? p : null;
        }

        /// <summary>
        /// Gets parameter by ID.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Parameter GetParameter(ParamID tag)
        {
            if (parms != null)
                if (id2index.TryGetValue(tag, out var it))
                    return parms[it];
            return null;
        }

        /// <summary>
        /// Returns the count of parameters.
        /// </summary>
        /// <returns></returns>
        public int ParameterCount => parms != null ? parms.Count : 0;

        /// <summary>
        /// Gets parameter by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Parameter GetParameterByIndex(int index) => parms != null ? parms[index] : null;

        /// Removes all parameters.
        public void RemoveAll()
        {
            if (parms != null)
                parms.Clear();
            id2index.Clear();
        }

        /// <summary>
        /// Remove a specific parameter by ID.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool RemoveParameter(ParamID tag)
        {
            if (parms == null)
                return false;

            if (id2index.TryGetValue(tag, out var it))
            {
                parms.RemoveAt(it);
                id2index.Remove(tag);
            }
            return false;
        }

        /// <summary>
        /// Creates and adds a new parameter with given properties.
        /// </summary>
        /// <returns></returns>
        public Parameter AddParameter(string title, string units = null, int stepCount = 0,
            ParamValue defaultValueNormalized = 0.0,
            ParameterFlags flags = ParameterFlags.CanAutomate, int tag = -1,
            UnitID unitID = UnitInfo.RootUnitId, string shortTitle = null)
        {
            if (title == null)
                return null;

            ParameterInfo info = new();

            info.Title = title;
            if (units != null)
                info.Units = units;
            if (shortTitle != null)
                info.ShortTitle = shortTitle;

            info.StepCount = stepCount;
            info.DefaultNormalizedValue = defaultValueNormalized;
            info.Flags = flags;
            info.Id = (ParamID)(tag >= 0 ? tag : ParameterCount);
            info.UnitId = unitID;

            return AddParameter(ref info);
        }
    }
}
