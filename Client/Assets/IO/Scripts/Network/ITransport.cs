using System;

namespace IO.Net
{
    public interface ITransport
    {
        event EventHandler<SocketCloseEventArgs> SocketClosedEvent;
        event EventHandler<SocketErrorEventArgs> SocketErrorEvent;
        event EventHandler<SocketMessageEventArgs> SocketMessageEvent;
        event EventHandler SocketOpenEvent;

        void ConnectAsync(string uri, Action<bool> callback);
        void Close();
        void SendAsync(byte[] data, Action<bool> completed);
    }

    public class SocketMessageEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }

        internal SocketMessageEventArgs(byte[] data)
        {
            Data = data;
        }
    }

    public class SocketCloseEventArgs : EventArgs
    {
        public int Code { get; private set; }
        public string Reason { get; private set; }

        internal SocketCloseEventArgs(int code, string reason)
        {
            Code = code;
            Reason = reason;
        }
    }

    public class SocketErrorEventArgs : EventArgs
    {
        public Exception Error { get; private set; }

        internal SocketErrorEventArgs(Exception error)
        {
            Error = error;
        }
    }
}