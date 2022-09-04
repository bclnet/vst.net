using System;
using System.Collections.Generic;
using System.Text;
using static Steinberg.Vst3.TResult;
using ParamID = System.UInt32;
using ParamValue = System.Double;
using ProgramListID = System.Int32;
using UnitID = System.Int32;

namespace Steinberg.Vst3
{
    public abstract class EditController : ComponentBase, IEditController, IEditController2
    {
        protected IComponentHandler componentHandler;
        protected IComponentHandler2 componentHandler2;
        protected ParameterContainer parameters = new();
        protected static KnobMode hostKnobMode = KnobMode.CircularMode;

        #region ComponentBase

        public override TResult Initialize(object context)
            => base.Initialize(context);

        public override TResult Terminate()
        {
            //componentHandler?.Release();
            componentHandler = null;
            //componentHandler2?.Release();
            componentHandler2 = null;

            return base.Terminate();
        }

        #endregion

        #region IEditController

        public virtual TResult SetComponentState(IBStream state)
            => kNotImplemented;

        public virtual TResult SetState(IBStream state)
            => kNotImplemented;

        public virtual TResult GetState(IBStream state)
            => kNotImplemented;

        public virtual int GetParameterCount()
            => parameters.ParameterCount;

        public virtual TResult GetParameterInfo(int paramIndex, out ParameterInfo info)
        {
            Parameter parameter;
            if ((parameter = parameters.GetParameterByIndex(paramIndex)) != null)
            {
                info = parameter.Info;
                return kResultTrue;
            }
            info = default;
            return kResultFalse;
        }

        public virtual TResult GetParamStringByValue(ParamID tag, ParamValue valueNormalized, StringBuilder str)
        {
            Parameter parameter;
            if ((parameter = GetParameterObject(tag)) != null)
            {
                str.Append(parameter.ToString(valueNormalized));
                return kResultTrue;
            }
            return kResultFalse;
        }

        public virtual TResult GetParamValueByString(ParamID tag, string str, out ParamValue valueNormalized)
        {
            Parameter parameter;
            if ((parameter = GetParameterObject(tag)) != null)
                if (parameter.FromString(str, out valueNormalized))
                    return kResultTrue;
            valueNormalized = default;
            return kResultFalse;
        }

        public virtual ParamValue NormalizedParamToPlain(ParamID tag, ParamValue valueNormalized)
        {
            Parameter parameter;
            return (parameter = GetParameterObject(tag)) != null
                ? parameter.ToPlain(valueNormalized)
                : valueNormalized;
        }

        public virtual ParamValue PlainParamToNormalized(ParamID tag, ParamValue plainValue)
        {
            Parameter parameter;
            return (parameter = GetParameterObject(tag)) != null
                ? parameter.ToNormalized(plainValue)
                : plainValue;
        }

        public virtual double GetParamNormalized(ParamID tag)
        {
            Parameter parameter;
            return (parameter = GetParameterObject(tag)) != null
                ? parameter.Normalized
                : 0.0;
        }

        public virtual TResult SetParamNormalized(ParamID tag, ParamValue value)
        {
            Parameter parameter;
            if ((parameter = GetParameterObject(tag)) != null)
            {
                parameter.SetNormalized(value);
                return kResultTrue;
            }
            return kResultFalse;
        }

        public virtual TResult SetComponentHandler(IComponentHandler handler)
        {
            if (componentHandler == handler)
                return kResultTrue;

            //componentHandler?.Release();

            componentHandler = handler;
            //componentHandler?.AddRef();

            // try to get the extended version
            if (componentHandler2 != null)
            {
                //componentHandler2.Release();
                componentHandler2 = null;
            }

            if (handler != null)
                componentHandler2 = handler as IComponentHandler2;
            return kResultTrue;
        }

        public virtual IPlugView CreateView(string name)
            => null;

        #endregion

        #region IEditController2

        public virtual TResult SetKnobMode(KnobMode mode)
        {
            hostKnobMode = mode;
            return kNotImplemented;
        }

        public virtual TResult OpenHelp(bool onlyCheck)
            => kResultFalse;

        public virtual TResult OpenAboutBox(bool onlyCheck)
            => kResultFalse;

        #endregion

        public TResult BeginEdit(ParamID tag)
            => componentHandler != null
                ? componentHandler.BeginEdit(tag)
                : kResultFalse;

        public TResult PerformEdit(ParamID tag, ParamValue valueNormalized)
            => componentHandler != null
                ? componentHandler.PerformEdit(tag, valueNormalized)
                : kResultFalse;

        public TResult EndEdit(ParamID tag)
            => componentHandler != null
                ? componentHandler.EndEdit(tag)
                : kResultFalse;

        public TResult StartGroupEdit() => componentHandler2 != null
            ? componentHandler2.StartGroupEdit()
            : kNotImplemented;

        /// <summary>
        /// calls IComponentHandler2::finishGroupEdit() if host supports it
        /// </summary>
        /// <returns></returns>
        public TResult FinishGroupEdit()
            => componentHandler2 != null
                ? componentHandler2.FinishGroupEdit()
                : kNotImplemented;

        /// <summary>
        /// called from EditorView if it was destroyed
        /// </summary>
        /// <param name="editor"></param>
        public void EditorDestroyed(EditorView editor) { }

        /// <summary>
        /// called from EditorView if it was attached to a parent
        /// </summary>
        /// <param name="editor"></param>
        public void EditorAttached(EditorView editor) { }

        /// <summary>
        /// called from EditorView if it was removed from a parent
        /// </summary>
        /// <param name="editor"></param>
        public void EditorRemoved(EditorView editor) { }

        /// <summary>
        /// return host knob mode
        /// </summary>
        public static KnobMode HostKnobMode
            => hostKnobMode;

        /// <summary>
        /// Gets for a given tag the parameter object.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public virtual Parameter GetParameterObject(ParamID tag)
            => parameters.GetParameter(tag);

        /// <summary>
        /// Gets for a given tag the parameter information.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public TResult GetParameterInfoByTag(ParamID tag, out ParameterInfo info)
        {
            Parameter parameter;
            if ((parameter = GetParameterObject(tag)) != null)
            {
                info = parameter.Info;
                return kResultTrue;
            }
            info = default;
            return kResultFalse;
        }

        /// <summary>
        /// Calls IComponentHandler2::setDirty (state) if host supports it.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public TResult SetDirty(bool state)
            => componentHandler2 != null
                ? componentHandler2.SetDirty(state)
                : kNotImplemented;

        /// <summary>
        /// Calls IComponentHandler2::requestOpenEditor (name) if host supports it.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TResult RequestOpenEditor(string name)
            => componentHandler2 != null
                ? componentHandler2.RequestOpenEditor(name)
                : kNotImplemented;

        public IComponentHandler ComponentHandler
            => componentHandler;
    }

    /// <summary>
    /// View related to an edit controller.
    /// </summary>
    public class EditorView : CPluginView, IDisposable
    {
        protected EditController controller;

        public EditorView(EditController controller, ViewRect? size = null)
            : base(size)
        {
            this.controller = controller;
            //controller?.AddRef();
        }

        public void Dispose()
        {
            if (controller != null)
            {
                controller.EditorDestroyed(this);
                //controller.Release();
            }
        }

        /// <summary>
        /// Gets its controller part.
        /// </summary>
        /// <returns></returns>
        public EditController Controller
            => controller;

        #region CPluginView

        public override void AttachedToParent()
            => controller?.EditorAttached(this);

        public override void RemovedFromParent()
            => controller?.EditorRemoved(this);

        #endregion
    }

    // EditControllerWithUnits
    public abstract class EditControllerEx1 : EditController, IUnitInfo
    {
        protected List<Unit> units = new();
        protected List<ProgramList> programLists = new();
        protected Dictionary<ProgramListID, ProgramList> programIndexMap = new();
        protected UnitID selectedUnit;

        public EditControllerEx1()
        {
            //UpdateHandler.Instance();
        }

        #region ComponentBase

        public override TResult Terminate()
        {
            units.Clear();

            //foreach (var programList in programLists)
            //    programList?.RemoveDependent(this);
            programLists.Clear();
            programIndexMap.Clear();

            return base.Terminate();
        }

        /// <summary>
        /// Adds a given unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool AddUnit(Unit unit)
        {
            units.Add(unit);
            return true;
        }

        //: IUnitInfo
        public virtual TResult GetUnitInfo(int unitIndex, out UnitInfo info)
        {
            Unit unit;
            if ((unit = units[unitIndex]) != null)
            {
                info = unit.Info;
                return kResultTrue;
            }
            info = default;
            return kResultFalse;
        }

        //: units selection
        /// <summary>
        /// Notifies the host about the selected Unit.
        /// </summary>
        /// <returns></returns>
        public virtual TResult NotifyUnitSelection()
        {
            var result = kResultFalse;
            if (componentHandler is IUnitHandler unitHandler)
                result = unitHandler.NotifyUnitSelection(selectedUnit);
            return result;
        }

        /// <summary>
        /// Adds a given program list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool AddProgramList(ProgramList list)
        {
            programIndexMap[list.ID] = list;
            programLists.Add(list);
            //list.AddDependent(this);
            return true;
        }

        /// <summary>
        /// Returns the ProgramList associated to a given listId.
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public ProgramList GetProgramList(ProgramListID listId)
            => programIndexMap.TryGetValue(listId, out var it) ? it : null;

        /// <summary>
        /// Notifies the host about program list changes.
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="programIndex"></param>
        /// <returns></returns>
        public TResult NotifyProgramListChange(ProgramListID listId, int programIndex = ProgramListInfo.AllProgramInvalid)
        {
            var result = kResultFalse;
            if (componentHandler is IUnitHandler unitHandler)
                result = unitHandler.NotifyProgramListChange(listId, programIndex);
            return result;
        }

        #endregion

        #region IUnitInfo

        public virtual int GetUnitCount()
            => units.Count;

        // GetUnitInfo (Above)

        public virtual int GetProgramListCount()
            => programLists.Count;

        public virtual TResult GetProgramListInfo(int listIndex, out ProgramListInfo info)
        {
            if (listIndex < 0 || listIndex >= programLists.Count)
            {
                info = default;
                return kResultFalse;
            }

            info = programLists[listIndex].Info;
            return kResultTrue;
        }

        public virtual TResult GetProgramName(ProgramListID listId, int programIndex, StringBuilder name)
            => programIndexMap.TryGetValue(listId, out var it)
            ? it.GetProgramName(programIndex, name)
            : kResultFalse;

        public virtual TResult SetProgramName(ProgramListID listId, int programIndex, string name)
            => programIndexMap.TryGetValue(listId, out var it)
            ? it.SetProgramName(programIndex, name)
            : kResultFalse;

        public virtual TResult GetProgramInfo(ProgramListID listId, int programIndex, string attributeId, StringBuilder attributeValue)
            => programIndexMap.TryGetValue(listId, out var it)
            ? it.GetProgramInfo(programIndex, attributeId, attributeValue)
            : kResultFalse;

        public virtual TResult HasProgramPitchNames(ProgramListID listId, int programIndex)
            => programIndexMap.TryGetValue(listId, out var it)
            ? it.HasPitchNames(programIndex)
            : kResultFalse;

        public virtual TResult GetProgramPitchName(ProgramListID listId, int programIndex, short midiPitch, StringBuilder name)
            => programIndexMap.TryGetValue(listId, out var it)
            ? it.GetPitchName(programIndex, midiPitch, name)
            : kResultFalse;

        #endregion

        #region units selection

        public virtual UnitID GetSelectedUnit() => selectedUnit;

        public virtual TResult SelectUnit(UnitID unitId)
        {
            selectedUnit = unitId;
            return kResultTrue;
        }

        public virtual TResult GetUnitByBus(MediaType type, BusDirection dir, int busIndex, int channel, out UnitID unitId)
        {
            unitId = default;
            return kNotImplemented;
        }

        public virtual TResult SetUnitProgramData(int listOrUnitId, int programIndex, IBStream data)
            => kNotImplemented;

        //: NotifyUnitSelection (Above)

        #endregion

        #region IDependent

        public virtual void Update(object changedUnknown, int message)
        {
            var programList = (ProgramList)changedUnknown;
            if (programList != null)
                if (componentHandler is IUnitHandler unitHandler)
                    unitHandler.NotifyProgramListChange(programList.ID, ProgramListInfo.AllProgramInvalid);
        }

        #endregion
    }

    /// <summary>
    /// Unit element.
    /// </summary>
    public class Unit
    {
        protected UnitInfo info;

        protected Unit() { }
        public Unit(string name, UnitID unitId, UnitID parentId = UnitInfo.RootUnitId, int programListId = UnitInfo.NoProgramListId)
        {
            info.Name = name;
            info.Id = unitId;
            info.ParentUnitId = parentId;
            info.ProgramListId = programListId;
        }
        public Unit(ref UnitInfo unit)
            => info = unit;

        /// <summary>
        /// Returns its info.
        /// </summary>
        public ref UnitInfo Info => ref info;

        /// <summary>
        /// Gets or sets a new Unit ID.
        /// </summary>
        public UnitID ID
        {
            get => info.Id;
            set => info.Id = value;
        }

        /// <summary>
        /// Gets or sets a new Unit Name.
        /// </summary>
        public string Name
        {
            get => info.Name;
            set => info.Name = value;
        }

        /// <summary>
        /// Gets or sets a new ProgramList ID.
        /// </summary>
        /// <param name="newID"></param>
        public ProgramListID ProgramListID
        {
            get => info.ProgramListId;
            set => info.ProgramListId = value;
        }
    }

    public class ProgramList : ObservableObject
    {
        protected ProgramListInfo info;
        protected UnitID unitId;
        protected List<string> programNames = new();
        protected List<Dictionary<string, string>> programInfos = new();
        protected Parameter parameter;

        public ProgramList(string name, ProgramListID listId, UnitID unitId)
        {
            this.unitId = unitId;
            //parameter = null;
            info.Name = name;
            info.Id = listId;
            //info.programCount = 0;
        }
        public ProgramList(ProgramList programList)
        {
            info = programList.info;
            unitId = programList.unitId;
            programNames = programList.programNames;
            //parameter = null;
        }

        public ref ProgramListInfo Info => ref info;
        public ProgramListID ID => info.Id;
        public string Name => info.Name;
        public int Count => info.ProgramCount;

        /// <summary>
        /// Adds a program to the end of the list. returns the index of the program.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual int AddProgram(string name)
        {
            ++info.ProgramCount;
            programNames.Add(name);
            programInfos.Add(new());
            return programNames.Count - 1;
        }

        /// <summary>
        /// Sets a program attribute value.
        /// </summary>
        /// <param name="programIndex"></param>
        /// <param name="attributeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool SetProgramInfo(int programIndex, string attributeId, StringBuilder value)
        {
            if (programIndex >= 0 && programIndex < programNames.Count)
            {
                programInfos[programIndex].Add(attributeId, value.ToString());
                return true;
            }
            return false;
        }

        public virtual TResult GetProgramInfo(int programIndex, string attributeId, StringBuilder value)
        {
            if (programIndex >= 0 && programIndex < programNames.Count)
                if (programInfos[programIndex].TryGetValue(attributeId, out var it))
                    if (!string.IsNullOrEmpty(it))
                    {
                        value.Append(it);
                        return kResultTrue;
                    }
            return kResultFalse;
        }

        public virtual TResult HasPitchNames(int programIndex)
            => kResultFalse;

        public virtual TResult GetPitchName(int programIndex, short midiPitch, StringBuilder name)
            => kResultFalse;

        public virtual TResult GetProgramName(int programIndex, StringBuilder name)
        {
            if (programIndex >= 0 && programIndex < programNames.Count)
            {
                name.Append(programNames[programIndex]);
                return kResultTrue;
            }
            return kResultFalse;
        }

        public virtual TResult SetProgramName(int programIndex, string name)
        {
            if (programIndex >= 0 && programIndex < programNames.Count)
            {
                programNames[programIndex] = name;
                if (parameter != null)
                    ((StringListParameter)parameter).ReplaceString(programIndex, name);
                return kResultTrue;
            }
            return kResultFalse;
        }

        /// <summary>
        /// Creates and returns the program parameter.
        /// </summary>
        /// <returns></returns>
        public virtual Parameter GetParameter()
        {
            if (parameter == null)
            {
                var listParameter = new StringListParameter(
                    info.Name, (ParamID)info.Id, null,
                    ParameterFlags.CanAutomate | ParameterFlags.IsList | ParameterFlags.IsProgramChange,
                    unitId);
                foreach (var programName in programNames)
                    listParameter.AppendString(programName);
                parameter = listParameter;
            }
            return parameter;
        }
    }

    public class ProgramListWithPitchNames : ProgramList
    {
        protected List<Dictionary<short, string>> pitchNames = new();

        public ProgramListWithPitchNames(string name, ProgramListID listId, UnitID unitId)
            : base(name, listId, unitId) { }

        //:ProgramList
        public override int AddProgram(string name)
        {
            var index = base.AddProgram(name);
            if (index >= 0)
                pitchNames.Add(new());
            return index;
        }

        /// <summary>
        /// Sets a name for the given program index and a given pitch.
        /// </summary>
        /// <param name="programIndex"></param>
        /// <param name="pitch"></param>
        /// <param name="pitchName"></param>
        /// <returns></returns>
        public bool SetPitchName(int programIndex, short pitch, string pitchName)
        {
            if (programIndex < 0 || programIndex >= Count)
                return false;

            var map = pitchNames[programIndex];
            var nameChanged = !(map.TryGetValue(pitch, out var res) && res == pitchName);
            map[pitch] = pitchName;

            if (nameChanged)
                OnPropertyChanged(nameof(pitchNames));
            return true;
        }

        /// <summary>
        /// Removes the PitchName entry for the given program index and a given pitch. Returns true if it was found and removed.
        /// </summary>
        /// <param name="programIndex"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public bool RemovePitchName(int programIndex, short pitch)
        {
            var result = false;
            if (programIndex >= 0 && programIndex < Count)
                result = pitchNames[programIndex].Remove(pitch);
            if (result)
                OnPropertyChanged(nameof(pitchNames));
            return result;
        }

        #region ProgramList

        public override TResult HasPitchNames(int programIndex)
            => programIndex >= 0 && programIndex < Count
                ? pitchNames[programIndex].Count == 0 ? kResultFalse : kResultTrue
                : kResultFalse;

        public override TResult GetPitchName(int programIndex, short midiPitch, StringBuilder name)
        {
            if (programIndex >= 0 && programIndex < Count)
                if (pitchNames[programIndex].TryGetValue(midiPitch, out var it))
                {
                    name.Append(it);
                    return kResultTrue;
                }
            return kResultFalse;
        }

        #endregion
    }
}
