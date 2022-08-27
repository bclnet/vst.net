using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.Plugin
{
    public abstract class EditController : ConnectionPoint, IEditController, IEditController2
    {
        protected EditController() { }

        public ParameterCollection Parameters { get; private set; } = new ParameterCollection();

        public IComponentHandler ComponentHandler { get; private set; }

        public IComponentHandler2 ComponentHandler2 { get; private set; }

        public IComponentHandler3 ComponentHandler3 { get; private set; }

        public override TResult Terminate()
        {
            ComponentHandler = null;
            ComponentHandler2 = null;
            ComponentHandler3 = null;

            return base.Terminate();
        }

        #region IEditController Members

        public virtual TResult SetComponentState(IBStream state)
        {
            Trace.WriteLine("IEditController.SetComponentState");

            return kNotImplemented;
        }

        public virtual TResult SetState(IBStream state)
        {
            Trace.WriteLine("IEditController.SetState");

            var reader = new VstStreamReader(state);
            reader.ReadParameters(Parameters);

            return kResultOk;
        }

        public virtual TResult GetState(IBStream state)
        {
            Trace.WriteLine("IEditController.GetState");

            var writer = new VstStreamWriter(state);
            writer.WriteParameters(Parameters);

            return kResultOk;
        }

        public virtual int GetParameterCount()
        {
            Trace.WriteLine("IEditController.GetParameterCount");

            return Parameters.Count;
        }

        public virtual TResult GetParameterInfo(int paramIndex, out ParameterInfo info)
        {
            Trace.WriteLine($"IEditController.GetParameterInfo {paramIndex}");

            if (paramIndex >= 0 && paramIndex < Parameters.Count)
            {
                var param = Parameters.GetAt(paramIndex);

                info.DefaultNormalizedValue = param.ValueInfo.ParameterInfo.DefaultNormalizedValue;
                info.Flags = param.ValueInfo.ParameterInfo.Flags;
                info.Id = param.ValueInfo.ParameterInfo.Id;
                info.ShortTitle = param.ValueInfo.ParameterInfo.ShortTitle;
                info.StepCount = param.ValueInfo.ParameterInfo.StepCount;
                info.Title = param.ValueInfo.ParameterInfo.Title;
                info.UnitId = param.ValueInfo.ParameterInfo.UnitId;
                info.Units = param.ValueInfo.ParameterInfo.Units;

                return kResultOk;
            }

            info = default;
            return kInvalidArgument;
        }

        public virtual TResult GetParamStringByValue(uint paramId, double valueNormalized, StringBuilder str)
        {
            Trace.WriteLine($"IEditController.GetParamStringByValue {paramId}, {valueNormalized}");

            if (Parameters.Contains(paramId))
            {
                var param = Parameters[paramId];

                str.Append(param.ToString(valueNormalized));
            }

            return kInvalidArgument;
        }

        public virtual TResult GetParamValueByString(uint paramId, string str, out double valueNormalized)
        {
            Trace.WriteLine($"IEditController.GetParamValueByString {paramId}, {str}");

            valueNormalized = default;
            if (Parameters.Contains(paramId))
            {
                var param = Parameters[paramId];

                if (param.TryParse(str, out double val))
                {
                    valueNormalized = val;

                    return kResultOk;
                }

                return kResultFalse;
            }

            return kInvalidArgument;
        }

        public virtual double NormalizedParamToPlain(uint paramId, double valueNormalized)
        {
            Trace.WriteLine($"IEditController.NormalizedParamToPlain {paramId}, {valueNormalized}");

            if (Parameters.Contains(paramId))
            {
                var param = Parameters[paramId];

                return param.ToPlain(valueNormalized);
            }

            return 0.0;
        }

        public virtual double PlainParamToNormalized(uint paramId, double plainValue)
        {
            Trace.WriteLine($"IEditController.PlainParamToNormalized {paramId}, {plainValue}");

            if (Parameters.Contains(paramId))
            {
                var param = Parameters[paramId];

                return param.ToNormalized(plainValue);
            }

            return 0.0;
        }

        public virtual double GetParamNormalized(uint paramId)
        {
            Trace.WriteLine($"IEditController.GetParamNormalized {paramId}");

            if (Parameters.Contains(paramId))
            {
                var param = Parameters[paramId];

                return param.NormalizedValue;
            }

            return 0.0;
        }

        public virtual TResult SetParamNormalized(uint paramId, double value)
        {
            Trace.WriteLine($"IEditController.SetParamNormalized {paramId}, {value}");

            if (Parameters.Contains(paramId))
            {
                var param = Parameters[paramId];

                param.NormalizedValue = value;

                return kResultOk;
            }

            return kInvalidArgument;
        }

        public virtual TResult SetComponentHandler(IComponentHandler handler)
        {
            if (handler == null)
            {
                Trace.WriteLine("IEditController.SetComponentHandler [null]");
            }
            else
            {
                Trace.WriteLine("IEditController.SetComponentHandler [ptr]");
            }

            ComponentHandler = handler;
            ComponentHandler2 = handler as IComponentHandler2;
            ComponentHandler3 = handler as IComponentHandler3;

            return kResultOk;
        }

        public virtual IPlugView CreateView(string name)
        {
            Trace.WriteLine($"IEditController.CreateView {name}");

            return null;
        }

        #endregion

        #region IEditController2 Members

        public virtual TResult SetKnobMode(KnobMode mode)
        {
            Trace.WriteLine($"IEditController2.SetKnobMode {mode}");

            return kNotImplemented;
        }

        public virtual TResult OpenHelp(bool onlyCheck)
        {
            Trace.WriteLine("IEditController2.OpenHelp");

            return kNotImplemented;
        }

        public virtual TResult OpenAboutBox(bool onlyCheck)
        {
            Trace.WriteLine("IEditController2.OpenAboutBox");

            return kNotImplemented;
        }

        #endregion
    }

    public class Unit
    {
        protected Unit(int id, string name, int parentId, int programListId)
        {
            Info.Id = id;
            Info.Name = name;
            Info.ParentUnitId = parentId;
            Info.ProgramListId = programListId;
        }

        public Unit(int id, string name, Unit parent, ProgramList programList)
            : this(id, name,
                    parent == null ? UnitInfo.NoParentUnitId : parent.Info.Id,
                    programList == null ? UnitInfo.NoProgramListId : programList.Id)
        {
            if (parent != null)
            {
                //Parent = parent; // collection takes care of this
                parent.Children.Add(this);
            }

            ProgramList = programList;
        }

        public UnitInfo Info;

        public Unit Parent { get; set; }

        UnitCollection _children;

        public UnitCollection Children
        {
            get
            {
                if (_children == null) Children = new UnitCollection();

                return _children;
            }
            protected set
            {
                _children = value;

                if (_children != null) _children.Parent = this;
            }
        }

        public ProgramList ProgramList { get; protected set; }
    }

    public class ProgramList : Collection<Program>
    {
        public ProgramList(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public Program AddProgram(string name)
        {
            var program = new Program(name);

            Add(program);

            return program;
        }

        public ProgramWithPitchNames AddProgramWithPitchNames(string name)
        {
            var program = new ProgramWithPitchNames(name);

            Add(program);

            return program;
        }
    }

    public class ProgramWithPitchNames : Program
    {
        readonly Dictionary<int, string> _pitchNames = new();

        public ProgramWithPitchNames(string name)
            : base(name) { }

        public IDictionary<int, string> PitchNames => _pitchNames;
    }

    // EditControllerWithUnits
    public abstract class EditControllerEx1 : EditController, IUnitInfo
    {
        public override TResult SetComponentHandler(IComponentHandler handler)
        {
            UnitHandler = handler as IUnitHandler;

            return base.SetComponentHandler(handler);
        }

        ProgramListCollection _programLists;

        public ProgramListCollection ProgramLists
        {
            get
            {
                if (_programLists == null) _programLists = new ProgramListCollection();
                return _programLists;
            }
            protected set => _programLists = value;
        }

        public IUnitHandler UnitHandler { get; private set; }

        UnitCollection _units;

        public UnitCollection Units
        {
            get
            {
                if (_units == null) _units = new UnitCollection();

                return _units;
            }
            protected set => _units = value;
        }

        Unit _rootUnit;
        public Unit RootUnit
        {
            get => _rootUnit;
            protected set
            {
                if (value != null && value.Info.Id != UnitInfo.RootUnitId)
                    throw new ArgumentException($"The Id of the RootUnit must be {UnitInfo.RootUnitId}.", nameof(value));
                _rootUnit = value;
            }
        }

        Unit _selectedUnit;
        public Unit SelectedUnit
        {
            get => _selectedUnit;
            set
            {
                if (_selectedUnit != value)
                {
                    _selectedUnit = value;
                    OnPropertyChanged(nameof(SelectedUnit));
                    UnitHandler?.NotifyUnitSelection(_selectedUnit != null ? _selectedUnit.Info.Id : 0);
                }
            }
        }

        #region IUnitInfo members

        public virtual int GetUnitCount()
            => Units.Count;

        public virtual TResult GetUnitInfo(int unitIndex, out UnitInfo info)
        {
            if (unitIndex >= 0 && unitIndex < Units.Count)
            {
                var unit = Units.GetAt(unitIndex);

                info.Id = unit.Info.Id;
                info.Name = unit.Info.Name;
                info.ParentUnitId = unit.Info.ParentUnitId;
                info.ProgramListId = unit.Info.ProgramListId;

                return kResultOk;
            }
            info = default;
            return kInvalidArgument;
        }

        public virtual int GetProgramListCount()
            => ProgramLists.Count;

        public virtual TResult GetProgramListInfo(int listIndex, out ProgramListInfo info)
        {
            if (listIndex >= 0 && listIndex < ProgramLists.Count)
            {
                var programList = ProgramLists.GetAt(listIndex);

                info.Id = programList.Id;
                info.Name = programList.Name;
                info.ProgramCount = programList.Count;

                return kResultOk;
            }

            info = default;
            return kInvalidArgument;
        }

        public virtual TResult GetProgramName(int listId, int programIndex, StringBuilder name)
        {
            if (ProgramLists.Contains(listId))
            {
                var programList = ProgramLists[listId];

                if (programIndex >= 0 && programIndex < programList.Count)
                {
                    var program = programList[programIndex];

                    name.Append(program.Name);

                    return kResultOk;
                }
            }

            return kInvalidArgument;
        }

        public virtual TResult GetProgramInfo(int listId, int programIndex, string attributeId, StringBuilder attributeValue)
        {
            if (ProgramLists.Contains(listId))
            {
                var programList = ProgramLists[listId];

                if (programIndex >= 0 && programIndex < programList.Count)
                {
                    var program = programList[programIndex];

                    if (program.AttributeValues.ContainsKey(attributeId))
                    {
                        attributeValue.Append(program[attributeId]);

                        return kResultOk;
                    }

                    return kResultFalse;
                }
            }

            return kInvalidArgument;
        }

        public virtual TResult HasProgramPitchNames(int listId, int programIndex)
        {
            if (ProgramLists.Contains(listId))
            {
                var programList = ProgramLists[listId];

                if (programIndex >= 0 && programIndex < programList.Count)
                {
                    var program = programList[programIndex];

                    return program is ProgramWithPitchNames ? kResultTrue : kResultFalse;
                }
            }

            return kInvalidArgument;
        }

        public virtual TResult GetProgramPitchName(int listId, int programIndex, short midiPitch, StringBuilder name)
        {
            if (ProgramLists.Contains(listId))
            {
                var programList = ProgramLists[listId];

                if (programIndex >= 0 && programIndex < programList.Count)
                {
                    if (programList[programIndex] is ProgramWithPitchNames program &&
                        program.PitchNames.ContainsKey(midiPitch))
                    {
                        name.Append(program.PitchNames[midiPitch]);

                        return kResultOk;
                    }

                    return kResultFalse;
                }
            }

            return kInvalidArgument;
        }

        public virtual int GetSelectedUnit()
            => SelectedUnit != null ? SelectedUnit.Info.Id : 0;

        public virtual TResult SelectUnit(int unitId)
        {
            if (Units.Contains(unitId))
            {
                SelectedUnit = Units[unitId];

                return kResultOk;
            }

            return kInvalidArgument;
        }

        public virtual TResult GetUnitByBus(MediaTypes type, BusDirections dir, int busIndex, int channel, out int unitId)
        {
            unitId = default;
            return kNotImplemented;
        }

        public virtual TResult SetUnitProgramData(int listOrUnitId, int programIndex, IBStream data)
            => kNotImplemented;

        #endregion

        // ---------------------------------------------------------------------

        public class ProgramListCollection : KeyedCollectionWithIndex<int, ProgramList>
        {
            protected override int GetKeyForItem(ProgramList item)
            {
                if (item == null) return 0;

                return item.Id;
            }
        }
    }
}
