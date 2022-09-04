using System;
using System.Collections.Generic;
using static Steinberg.Vst3.TResult;
using NoteExpressionTypeID = System.UInt32;
using NoteExpressionValue = System.Double;
using ParamValue = System.Double;
using PhysicalUITypeID = System.UInt32;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Note expression type object.
    /// </summary>
    public class NoteExpressionType
    {
        protected NoteExpressionTypeInfo info;
        protected Parameter associatedParameter;
        protected int precision;
        protected PhysicalUITypeID physicalUITypeID = (PhysicalUITypeID)PhysicalUITypeIDs.InvalidPUITypeID;

        public NoteExpressionType()
            => precision = 4;
        public NoteExpressionType(ref NoteExpressionTypeInfo info)
        {
            precision = 4;
            this.info = info;
        }
        public NoteExpressionType(NoteExpressionTypeID typeId, string title, string shortTitle,
            string units, int unitId, NoteExpressionValue defaultValue,
            NoteExpressionValue minimum, NoteExpressionValue maximum, int stepCount,
            NoteExpressionTypeFlags flags = 0, int precision = 4)
        {
            this.precision = precision;
            info.TypeId = typeId;
            if (title != null)
                info.Title = title;
            if (shortTitle != null)
                info.ShortTitle = shortTitle;
            if (units != null)
                info.ShortTitle = units;
            info.UnitId = unitId;
            info.ValueDesc.DefaultValue = defaultValue;
            info.ValueDesc.Minimum = minimum;
            info.ValueDesc.Maximum = maximum;
            info.ValueDesc.StepCount = stepCount;
            info.Flags = flags;
        }
        public NoteExpressionType(NoteExpressionTypeID typeId, string title, string shortTitle,
            string units, int unitId, Parameter associatedParameter,
            NoteExpressionTypeFlags flags = 0)
        {
            this.associatedParameter = associatedParameter;
            precision = 4;
            info.TypeId = typeId;
            if (title != null)
                info.Title = title;
            if (shortTitle != null)
                info.ShortTitle = shortTitle;
            if (units != null)
                info.ShortTitle = units;
            info.UnitId = unitId;
            info.ValueDesc.DefaultValue = 0.5;
            info.ValueDesc.Minimum = 0.0;
            info.ValueDesc.Maximum = 0.1;
            info.Flags = flags;
            if (associatedParameter != null)
            {
                info.ValueDesc.StepCount = associatedParameter.Info.StepCount;
                info.ValueDesc.DefaultValue = associatedParameter.Info.DefaultNormalizedValue;
                info.AssociatedParameterId = associatedParameter.Info.Id;
                info.Flags |= NoteExpressionTypeFlags.AssociatedParameterIDValid;
            }
        }

        /// <summary>
        /// get the underlying NoteExpressionTypeInfo struct
        /// </summary>
        public NoteExpressionTypeInfo Info => info;

        /// <summary>
        /// convert a note expression value to a readable string
        /// </summary>
        /// <param name="valueNormalized"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public virtual TResult GetStringByValue(NoteExpressionValue valueNormalized, out string str)
        {
            if (associatedParameter != null)
            {
                str = associatedParameter.ToString(valueNormalized);
                return kResultTrue;
            }
            if (info.ValueDesc.StepCount > 0)
            {
                var value = Math.Min(info.ValueDesc.StepCount, (int)(valueNormalized * (info.ValueDesc.StepCount + 1)));
                str = value.ToString();
            }
            else
                str = valueNormalized.ToString($"0.{new string('#', precision)}");
            return kResultTrue;
        }

        /// <summary>
        /// convert a readable string to a note expression value
        /// </summary>
        /// <returns></returns>
        public virtual TResult GetValueByString(string str, out NoteExpressionValue valueNormalized)
        {
            valueNormalized = default;
            if (associatedParameter != null)
                return associatedParameter.FromString(str, out valueNormalized)
                    ? kResultTrue
                    : kResultFalse;
            if (info.ValueDesc.StepCount > 0)
            {
                if (int.TryParse(str, out var value2) && value2 <= info.ValueDesc.StepCount)
                {
                    valueNormalized = (NoteExpressionValue)value2 / (NoteExpressionValue)info.ValueDesc.StepCount;
                    return kResultTrue;
                }
                return kResultFalse;
            }
            var value = double.TryParse(str, out var z) ? z : default;
            if (value < info.ValueDesc.Minimum)
                return kResultFalse;
            if (value > info.ValueDesc.Maximum)
                return kResultFalse;
            valueNormalized = value;
            return kResultTrue;
        }

        /// <summary>
        /// Gets or sets the current precision (used for string representation of float), (for example 4.34 with 2 as precision)
        /// </summary>
        public int Precision
        {
            get => precision;
            set => precision = value;
        }

        public TResult GetPhysicalUIType(out PhysicalUITypeID physicalUITypeID)
        {
            physicalUITypeID = this.physicalUITypeID;
            return kResultTrue;
        }

        public TResult SetPhysicalUITypeID(PhysicalUITypeID physicalUITypeID)
        {
            this.physicalUITypeID = physicalUITypeID;
            return kResultTrue;
        }
    }

    /// <summary>
    /// Note expression type object representing a custom range.
    /// </summary>
    public class RangeNoteExpressionType : NoteExpressionType
    {
        protected NoteExpressionValue plainMin;
        protected NoteExpressionValue plainMax;

        public RangeNoteExpressionType(NoteExpressionTypeID typeId, string title,
            string shortTitle, string units, int unitId,
            NoteExpressionValue defaultPlainValue, NoteExpressionValue plainMin,
            NoteExpressionValue plainMax, NoteExpressionTypeFlags flags = 0, int precision = 4)
            : base(typeId, title, shortTitle, units, unitId, 0, 0, 1, 0, flags, precision)
        {
            this.plainMin = plainMin;
            this.plainMax = plainMax;
            info.ValueDesc.DefaultValue = (defaultPlainValue - Min) / (Max - Min);
        }

        /// <summary>
        /// Gets or sets the minimum plain value
        /// </summary>
        /// <returns></returns>
        public virtual ParamValue Min
        {
            get => plainMin;
            set => plainMin = value;
        }

        /// <summary>
        /// Gets or sets the maximum plain value
        /// </summary>
        public virtual ParamValue Max
        {
            get => plainMax;
            set => plainMin = value;
        }

        public override TResult GetStringByValue(NoteExpressionValue valueNormalized, out string str)
        {
            NoteExpressionValue plain = valueNormalized * (Max - Min) + Min;
            str = plain.ToString($"0.{new string('#', precision)}");
            return kResultTrue;
        }

        public override TResult GetValueByString(string str, out NoteExpressionValue valueNormalized)
        {
            if (double.TryParse(str, out var value))
            {
                value = (value - Min) / (Max - Min);
                if (value >= 0.0 && value <= 1.0)
                {
                    valueNormalized = value;
                    return kResultTrue;
                }
            }
            valueNormalized = default;
            return kResultFalse;
        }
    }

    /// <summary>
    /// Collection of note expression types.
    /// </summary>
    public class NoteExpressionTypeContainer
    {
        protected List<NoteExpressionType> noteExps = new();

        protected NoteExpressionType Find(NoteExpressionTypeID typeId)
        {
            foreach (var it in noteExps)
                if (it.Info.TypeId == typeId)
                    return it;
            return null;
        }

        /// <summary>
        /// add a note expression type. The container owns the type. No need to release it afterwards.
        /// </summary>
        /// <param name="noteExpType"></param>
        /// <returns></returns>
        public bool AddNoteExpressionType(NoteExpressionType noteExpType)
        {
            noteExps.Add(noteExpType);
            return true;
        }

        /// <summary>
        /// remove a note expression type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public bool RemoveNoteExpressionType(NoteExpressionTypeID typeId)
        {
            var it = Find(typeId);
            if (it != null)
            {
                noteExps.Remove(it);
                return true;
            }
            return false;
        }


        /// <summary>
        /// remove all note expression types
        /// </summary>
        public void RemoveAll()
            => noteExps.Clear();

        /// <summary>
        /// get a note expression type object by ID
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public NoteExpressionType GetNoteExpressionType(NoteExpressionTypeID typeId)
            => Find(typeId);

        /// <summary>
        /// get the number of note expression types
        /// </summary>
        /// <returns></returns>
        public int GetNoteExpressionCount()
            => noteExps.Count;

        /// <summary>
        /// get note expression info
        /// </summary>
        /// <param name="noteExpressionIndex"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public TResult GetNoteExpressionInfo(int noteExpressionIndex, out NoteExpressionTypeInfo info)
        {
            if (noteExpressionIndex < 0 || noteExpressionIndex >= noteExps.Count)
            {
                info = default;
                return kInvalidArgument;
            }
            info = noteExps[noteExpressionIndex].Info;
            return kResultTrue;
        }

        /// <summary>
        /// convert a note expression value to a readable string
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valueNormalized"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public TResult GetNoteExpressionStringByValue(NoteExpressionTypeID id, NoteExpressionValue valueNormalized, out string str)
        {
            str = default;
            var noteExpType = GetNoteExpressionType(id);
            return noteExpType != null
                ? noteExpType.GetStringByValue(valueNormalized, out str)
                : kResultFalse;
        }


        /// <summary>
        /// convert a string to a note expression value
        /// </summary>
        /// <param name="id"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public TResult GetNoteExpressionValueByString(NoteExpressionTypeID id, string str, out NoteExpressionValue valueNormalized)
        {
            valueNormalized = default;
            var noteExpType = GetNoteExpressionType(id);
            return noteExpType != null
                ? noteExpType.GetValueByString(str, out valueNormalized)
                : kResultFalse;
        }

        /// <summary>
        /// get the Physical UI Type associated to a given Note Expression Id
        /// </summary>
        /// <param name="physicalUITypeID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public TResult GetMappedNoteExpression(PhysicalUITypeID physicalUITypeID, out NoteExpressionTypeID id)
        {
            id = (NoteExpressionTypeID)NoteExpressionTypeIDs.InvalidTypeID;
            foreach (var item in noteExps)
                if (item.GetPhysicalUIType(out var tmp) == kResultTrue)
                    if (tmp == physicalUITypeID)
                    {
                        id = item.Info.TypeId;
                        break;
                    }
            return kResultTrue;
        }
    }
}
