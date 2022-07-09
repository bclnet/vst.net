using Jacobi.Vst3.Core;

namespace Jacobi.Vst3.Host
{
    public class ConnectionProxy : IConnectionPoint
    {
        //protected ThreadChecker threadChecker = ThreadChecker.create();
        protected IConnectionPoint srcConnection;
        protected IConnectionPoint dstConnection;

        public ConnectionProxy(IConnectionPoint srcConnection)
            => this.srcConnection = srcConnection;

        //--- from IConnectionPoint
        public int Connect(IConnectionPoint other)
        {
            if (other == null) return TResult.E_InvalidArg;
            if (dstConnection != null) return TResult.S_False;

            dstConnection = other; // share it
            var res = srcConnection.Connect(this);
            if (res != TResult.S_True) dstConnection = null;
            return res;
        }

        public int Disconnect(IConnectionPoint other)
        {
            if (other == null) return TResult.E_InvalidArg;

            if (other == dstConnection)
            {
                srcConnection?.Disconnect(this);
                dstConnection = null;
                return TResult.S_True;
            }
            return TResult.E_InvalidArg;
        }

        public int Notify(IMessage message)
        {
            if (dstConnection != null)
            {
                // We discard the message if we are not in the UI main thread
                //if (threadChecker?.Test()) return dstConnection.Notify(message);
            }
            return TResult.S_False;
        }

        public bool Disconnect()
            => Disconnect(dstConnection) == TResult.S_True;

    }
}
