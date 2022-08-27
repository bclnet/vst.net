using Jacobi.Vst3;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.Hosting
{
    public class ConnectionProxy : IConnectionPoint
    {
        //protected ThreadChecker threadChecker = ThreadChecker.create();
        protected IConnectionPoint srcConnection;
        protected IConnectionPoint dstConnection;

        public ConnectionProxy(IConnectionPoint srcConnection)
            => this.srcConnection = srcConnection;

        //--- from IConnectionPoint
        public TResult Connect(IConnectionPoint other)
        {
            if (other == null)
                return kInvalidArgument;
            if (dstConnection != null)
                return kResultFalse;

            dstConnection = other; // share it
            var res = srcConnection.Connect(this);
            if (res != kResultTrue)
                dstConnection = null;
            return res;
        }

        public TResult Disconnect(IConnectionPoint other)
        {
            if (other == null)
                return kInvalidArgument;

            if (other == dstConnection)
            {
                srcConnection?.Disconnect(this);
                dstConnection = null;
                return kResultTrue;
            }
            return kInvalidArgument;
        }

        public TResult Notify(IMessage message)
        {
            if (dstConnection != null)
            {
                // We discard the message if we are not in the UI main thread
                //TODO: threadChecker
                //if (threadChecker?.Test()) return dstConnection.Notify(message);
            }
            return kResultFalse;
        }

        public bool Disconnect()
            => Disconnect(dstConnection) == kResultTrue;
    }
}
