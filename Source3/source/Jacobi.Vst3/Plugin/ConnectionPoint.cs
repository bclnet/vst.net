using Jacobi.Vst3;
using System;
using System.Diagnostics;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.Plugin
{
    public abstract class ConnectionPoint : ObservableObject, IPluginBase, IConnectionPoint, IServiceContainerSite
    {
        IConnectionPoint _peer;

        protected ConnectionPoint() { }

        #region IServiceContainerSite Members

        public ServiceContainer ServiceContainer { get; protected set; } = new ServiceContainer();

        #endregion

        #region IPluginBase Members

        public virtual TResult Initialize(object context)
        {
            Trace.WriteLine("IPluginBase.Initialize");

            ServiceContainer.Unknown = context;

            return kResultOk;
        }

        public virtual TResult Terminate()
        {
            Trace.WriteLine("IPluginBase.Terminate");

            _peer = null;

            ServiceContainer.Dispose();

            return kResultOk;
        }

        #endregion

        #region IConnectionPoint Members

        public virtual TResult Connect(IConnectionPoint other)
        {
            Trace.WriteLine("IConnectionPoint.Connect");

            if (other == null) return kInvalidArgument;
            if (_peer != null) return kResultFalse;

            _peer = other;

            return kResultOk;
        }

        public virtual TResult Disconnect(IConnectionPoint other)
        {
            Trace.WriteLine("IConnectionPoint.Disconnect");

            if (_peer != null && _peer == other)
            {
                _peer = null;

                return kResultOk;
            }

            return kResultFalse;
        }

        public virtual TResult Notify(IMessage message)
        {
            if (message == null) return kInvalidArgument;

            Trace.WriteLine($"IConnectionPoint.Notify {message.GetMessageID()}");

            return OnMessageReceived(new MessageEventArgs(message)) ? kResultOk : kResultFalse;
        }

        #endregion

        // use funcBeforeSend to populate message.
        protected virtual bool SendMessage(Action<IMessage> funcBeforeSend)
        {
            Guard.ThrowIfNull(nameof(funcBeforeSend), funcBeforeSend);

            if (_peer == null) return false;

            var host = ServiceContainer.GetService<IHostApplication>();
            if (host == null) return false;
            var msg = host.AllocateMessage();

            if (msg != null)
            {
                funcBeforeSend(msg);

                var result = _peer.Notify(msg);

                return result.Succeeded();
            }

            return false;
        }

        protected virtual bool OnMessageReceived(MessageEventArgs messageEventArgs)
        {
            var handler = MessageReceived;

            if (handler != null)
            {
                handler(this, messageEventArgs);

                return true;
            }

            return false;
        }

        public event EventHandler<MessageEventArgs> MessageReceived;
    }
}
