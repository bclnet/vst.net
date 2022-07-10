﻿using Jacobi.Vst3.Common;
using Jacobi.Vst3.Core;
using System;
using System.Text;

namespace Jacobi.Vst3.Plugin
{
    public abstract class EditControllerWithUnits : EditController, IUnitInfo
    {
        public override int SetComponentHandler(IComponentHandler handler)
        {
            UnitHandler = handler as IUnitHandler;

            return base.SetComponentHandler(handler);
        }

        private ProgramListCollection _programLists;

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

        private UnitCollection _units;

        public UnitCollection Units
        {
            get
            {
                if (_units == null) _units = new UnitCollection();

                return _units;
            }
            protected set => _units = value;
        }

        private Unit _rootUnit;
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

        private Unit _selectedUnit;
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

        public virtual int GetUnitInfo(int unitIndex, out UnitInfo info)
        {
            if (unitIndex >= 0 && unitIndex < Units.Count)
            {
                var unit = Units.GetAt(unitIndex);

                info.Id = unit.Info.Id;
                info.Name = unit.Info.Name;
                info.ParentUnitId = unit.Info.ParentUnitId;
                info.ProgramListId = unit.Info.ProgramListId;

                return TResult.S_OK;
            }
            info = default;
            return TResult.E_InvalidArg;
        }

        public virtual int GetProgramListCount()
            => ProgramLists.Count;

        public virtual int GetProgramListInfo(int listIndex, out ProgramListInfo info)
        {
            if (listIndex >= 0 && listIndex < ProgramLists.Count)
            {
                var programList = ProgramLists.GetAt(listIndex);

                info.Id = programList.Id;
                info.Name = programList.Name;
                info.ProgramCount = programList.Count;

                return TResult.S_OK;
            }

            info = default;
            return TResult.E_InvalidArg;
        }

        public virtual int GetProgramName(int listId, int programIndex, StringBuilder name)
        {
            if (ProgramLists.Contains(listId))
            {
                var programList = ProgramLists[listId];

                if (programIndex >= 0 && programIndex < programList.Count)
                {
                    var program = programList[programIndex];

                    name.Append(program.Name);

                    return TResult.S_OK;
                }
            }

            return TResult.E_InvalidArg;
        }

        public virtual int GetProgramInfo(int listId, int programIndex, string attributeId, StringBuilder attributeValue)
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

                        return TResult.S_OK;
                    }

                    return TResult.S_False;
                }
            }

            return TResult.E_InvalidArg;
        }

        public virtual int HasProgramPitchNames(int listId, int programIndex)
        {
            if (ProgramLists.Contains(listId))
            {
                var programList = ProgramLists[listId];

                if (programIndex >= 0 && programIndex < programList.Count)
                {
                    var program = programList[programIndex];

                    return program is ProgramWithPitchNames ? TResult.S_True : TResult.S_False;
                }
            }

            return TResult.E_InvalidArg;
        }

        public virtual int GetProgramPitchName(int listId, int programIndex, short midiPitch, StringBuilder name)
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

                        return TResult.S_OK;
                    }

                    return TResult.S_False;
                }
            }

            return TResult.E_InvalidArg;
        }

        public virtual int GetSelectedUnit()
            => SelectedUnit != null ? SelectedUnit.Info.Id : 0;

        public virtual int SelectUnit(int unitId)
        {
            if (Units.Contains(unitId))
            {
                SelectedUnit = Units[unitId];

                return TResult.S_OK;
            }

            return TResult.E_InvalidArg;
        }

        public virtual int GetUnitByBus(MediaTypes type, BusDirections dir, int busIndex, int channel, out int unitId)
        {
            unitId = default;
            return TResult.E_NotImplemented;
        }

        public virtual int SetUnitProgramData(int listOrUnitId, int programIndex, IBStream data)
            => TResult.E_NotImplemented;

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
