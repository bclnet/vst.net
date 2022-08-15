using Jacobi.Vst3.Core;
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
}
