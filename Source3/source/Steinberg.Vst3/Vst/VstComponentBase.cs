using System.Text;
using static Steinberg.Vst3.TResult;

namespace Steinberg.Vst3
{
    public abstract class ComponentBase : ObservableObject, IPluginBase, IConnectionPoint
    {
        protected object hostContext;
        protected IConnectionPoint peerConnection;

        /// <summary>
        /// Returns the hostContext (set by the host during initialize call).
        /// </summary>
        public object HostContext => hostContext;

        /// <summary>
        /// Returns the peer for the messaging communication (you can only use IConnectionPoint::notify for communicate between peers, do not try to cast peerConnection.
        /// </summary>
        public IConnectionPoint Peer => peerConnection;

        #region IPluginBase

        public virtual TResult Initialize(object context)
        {
            // check if already initialized
            if (hostContext != null)
                return kResultFalse;

            hostContext = context;

            return kResultOk;
        }

        public virtual TResult Terminate()
        {
            // release host interfaces
            hostContext = null;

            // in case host did not disconnect us,
            // release peer now
            peerConnection?.Disconnect(this);
            peerConnection = null;

            return kResultOk;
        }

        #endregion

        #region IConnectionPoint

        public virtual TResult Connect(IConnectionPoint other)
        {
            if (other == null)
                return kInvalidArgument;

            // check if already connected
            if (peerConnection != null)
                return kResultFalse;

            peerConnection = other;
            return kResultOk;
        }

        public virtual TResult Disconnect(IConnectionPoint other)
        {
            if (peerConnection != null && other == peerConnection)
            {
                peerConnection = null;
                return kResultOk;
            }
            return kResultFalse;
        }

        public virtual TResult Notify(IMessage message)
        {
            if (message == null)
                return kInvalidArgument;

            if (message.GetMessageID() == "TextMessage")
            {
                var str = new StringBuilder();
                if (message.GetAttributes().GetString("Text", str, 255) == kResultOk)
                    return ReceiveText(str.ToString());
            }
            return kResultFalse;
        }

        #endregion

        public IMessage AllocateMessage()
        {
            var hostApp = (IHostApplication)hostContext;
            if (hostApp != null)
                return hostApp.AllocateMessage();
            return null;
        }

        public TResult SendMessage(IMessage message)
        {
            if (message != null && Peer != null)
                return Peer.Notify(message);
            return kResultFalse;
        }

        public TResult SendTextMessage(string text)
        {
            var msg = AllocateMessage();
            if (msg != null)
            {
                msg.SetMessageID("TextMessage");
                if (text.Length >= 256)
                    text = text.Remove(255);
                msg.GetAttributes().SetString("Text", text);
                return SendMessage(msg);
            }
            return kResultFalse;
        }

        public TResult SendMessageID(string messageID)
        {
            var msg = AllocateMessage();
            if (msg != null)
            {
                msg.SetMessageID(messageID);
                return SendMessage(msg);
            }
            return kResultFalse;
        }

        public TResult ReceiveText(string text)
            => kResultOk;
    }
}
