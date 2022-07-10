using Jacobi.Vst3.Core;
using System;
using System.Diagnostics;

namespace Jacobi.Vst3.Plugin
{
    public abstract class Component : ConnectionPoint, IComponent
    {
        protected abstract BusCollection GetBusCollection(MediaTypes mediaType, BusDirections busDir);

        public Guid ControlledClassId { get; protected set; }

        public bool IsActive { get; protected set; }

        #region IComponent Members

        public virtual int GetControllerClassId(out Guid controllerClassId)
        {
            Trace.WriteLine("IComponent.GetControllerClassId");

            controllerClassId = ControlledClassId;

            return ControlledClassId == Guid.Empty ? TResult.E_NotImplemented : TResult.S_OK;
        }

        public virtual int SetIoMode(IoModes mode)
        {
            Trace.WriteLine($"IComponent.SetIoMode({mode})");

            return TResult.E_NotImplemented;
        }

        // retval NOT a TResult
        public virtual int GetBusCount(MediaTypes type, BusDirections dir)
        {
            Trace.WriteLine($"IComponent.GetBusCount({type}, {dir})");

            var busses = GetBusCollection(type, dir);

            return busses != null ? busses.Count : 0;
        }

        public virtual int GetBusInfo(MediaTypes type, BusDirections dir, int index, out BusInfo bus)
        {
            Trace.WriteLine($"IComponent.GetBusInfo({type}, {dir}, {index})");

            var busses = GetBusCollection(type, dir);

            bus = default;
            if (busses != null)
            {
                if (index < 0 || index >= busses.Count) return TResult.E_InvalidArg;

                busses[index].GetInfo(ref bus);

                return TResult.S_OK;
            }

            return TResult.E_Unexpected;
        }

        public virtual int GetRoutingInfo(ref RoutingInfo inInfo, out RoutingInfo outInfo)
        {
            Trace.WriteLine("IComponent.GetRoutingInfo");

            outInfo = default;
            return TResult.E_NotImplemented;
        }

        public virtual int ActivateBus(MediaTypes type, BusDirections dir, int index, bool state)
        {
            Trace.WriteLine($"IComponent.ActivateBus({type}, {dir}, {index}, {state})");

            var busses = GetBusCollection(type, dir);

            if (busses != null)
            {
                if (index < 0 || index >= busses.Count) return TResult.E_InvalidArg;

                busses[index].IsActive = state;

                return TResult.S_OK;
            }

            return TResult.E_Unexpected;
        }

        public virtual int SetActive(bool state)
        {
            Trace.WriteLine($"IComponent.SetActive({state})");

            IsActive = state;

            return TResult.S_OK;
        }

        public virtual int SetState(IBStream state)
        {
            Trace.WriteLine("IComponent.SetState");

            return TResult.E_NotImplemented;
        }

        public virtual int GetState(IBStream state)
        {
            Trace.WriteLine("IComponent.GetState");

            return TResult.E_NotImplemented;
        }

        #endregion
    }
}
