using Jacobi.Vst3.Core;
using System;
using System.Diagnostics;
using static Jacobi.Vst3.Core.TResult;

namespace Jacobi.Vst3.Plugin
{
    public abstract class Component : ConnectionPoint, IComponent
    {
        protected abstract BusCollection GetBusCollection(MediaTypes mediaType, BusDirections busDir);

        public Guid ControlledClassId { get; protected set; }

        public bool IsActive { get; protected set; }

        #region IComponent Members

        public virtual TResult GetControllerClassId(out Guid controllerClassId)
        {
            Trace.WriteLine("IComponent.GetControllerClassId");

            controllerClassId = ControlledClassId;

            return ControlledClassId == Guid.Empty ? kNotImplemented : kResultOk;
        }

        public virtual TResult SetIoMode(IoModes mode)
        {
            Trace.WriteLine($"IComponent.SetIoMode({mode})");

            return kNotImplemented;
        }

        // retval NOT a TResult
        public virtual int GetBusCount(MediaTypes type, BusDirections dir)
        {
            Trace.WriteLine($"IComponent.GetBusCount({type}, {dir})");

            var busses = GetBusCollection(type, dir);

            return busses != null ? busses.Count : 0;
        }

        public virtual TResult GetBusInfo(MediaTypes type, BusDirections dir, int index, out BusInfo bus)
        {
            Trace.WriteLine($"IComponent.GetBusInfo({type}, {dir}, {index})");

            var busses = GetBusCollection(type, dir);

            bus = default;
            if (busses != null)
            {
                if (index < 0 || index >= busses.Count) return kInvalidArgument;

                busses[index].GetInfo(ref bus);

                return kResultOk;
            }

            return kNotInitialized;
        }

        public virtual TResult GetRoutingInfo(ref RoutingInfo inInfo, out RoutingInfo outInfo)
        {
            Trace.WriteLine("IComponent.GetRoutingInfo");

            outInfo = default;
            return kNotImplemented;
        }

        public virtual TResult ActivateBus(MediaTypes type, BusDirections dir, int index, bool state)
        {
            Trace.WriteLine($"IComponent.ActivateBus({type}, {dir}, {index}, {state})");

            var busses = GetBusCollection(type, dir);

            if (busses != null)
            {
                if (index < 0 || index >= busses.Count) return kInvalidArgument;

                busses[index].IsActive = state;

                return kResultOk;
            }

            return kNotInitialized;
        }

        public virtual TResult SetActive(bool state)
        {
            Trace.WriteLine($"IComponent.SetActive({state})");

            IsActive = state;

            return kResultOk;
        }

        public virtual TResult SetState(IBStream state)
        {
            Trace.WriteLine("IComponent.SetState");

            return kNotImplemented;
        }

        public virtual TResult GetState(IBStream state)
        {
            Trace.WriteLine("IComponent.GetState");

            return kNotImplemented;
        }

        #endregion
    }
}
