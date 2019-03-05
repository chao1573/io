using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace IO.Net
{
    enum TransportState
    {
        ReadHead = 0,
        ReadBody = 1,
    }

    class StateObject
    {
        public const int BufferSize = 1024;
        public Socket WorkSocket = null;
        public byte[] Buffer = new byte[BufferSize];
    }

    class SocketCallbackObject
    {
        public Socket WorkSocket;
        public Action<bool> Callback;
    }

    public class TransportTcp : ITransport
    {
        const int HeaderLength = 2;
        public event EventHandler<SocketCloseEventArgs> SocketClosedEvent;
        public event EventHandler<SocketErrorEventArgs> SocketErrorEvent;
        public event EventHandler<SocketMessageEventArgs> SocketMessageEvent;
        public event EventHandler SocketOpenEvent;

        Socket m_socket;
        StateObject m_stateObject = new StateObject();
        TransportState m_transportState = TransportState.ReadHead;

        int m_bufferOffset = 0;
        byte[] m_headBuffer = new Byte[HeaderLength];
        byte[] m_bodyBuffer;
        int m_bodyLen;

        void CreateSocket(string uri)
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Close()
        {
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
            m_socket = null;

            if (SocketClosedEvent != null)
            {
                SocketClosedEvent(this, new SocketCloseEventArgs(0, "disconnected"));
            }
        }

        public void ConnectAsync(string uri, Action<bool> callback)
        {
            if (m_socket == null)
            {
                CreateSocket(uri);
            }

            string[] splits = uri.Split(':');

            IPHostEntry ipHostInfo = Dns.GetHostEntry(splits[0]);
            IPAddress ipAddress = null;
            foreach (var address in ipHostInfo.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = address;
                    break;
                }
            }

            IPEndPoint remoteEP = new IPEndPoint(ipAddress, int.Parse(splits[1]));
            var state = new SocketCallbackObject { WorkSocket = m_socket, Callback = callback };
            m_socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), state);
        }

        void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                var callbackObj = (SocketCallbackObject)ar.AsyncState;
                callbackObj.WorkSocket.EndConnect(ar);
                if (callbackObj.Callback != null)
                    callbackObj.Callback(true);

                if (SocketOpenEvent != null)
                {
                    SocketOpenEvent(this, EventArgs.Empty);
                }

                Receive();
            }
            catch (Exception e)
            {
                var callbackObj = (SocketCallbackObject)ar.AsyncState;
                if (callbackObj.Callback != null)
                    callbackObj.Callback(false);
            }
        }

        public void SendAsync(byte[] data, Action<bool> completed)
        {
            if (m_socket != null)
            {
                int bodyLength = data.Length;
                byte[] buf = new byte[HeaderLength + bodyLength];
                byte[] lengthBytes = BitConverter.GetBytes((ushort)bodyLength);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(lengthBytes, 0, lengthBytes.Length);
                }
                Buffer.BlockCopy(lengthBytes, 0, buf, 0, lengthBytes.Length);
                Buffer.BlockCopy(data, 0, buf, HeaderLength, data.Length);
                var state = new SocketCallbackObject { WorkSocket = m_socket, Callback = completed };
                m_socket.BeginSend(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(SendCallback), state);
            }
            else
            {
                if (completed != null)
                {
                    completed(false);
                }
            }
        }

        void SendCallback(IAsyncResult ar)
        {
            try
            {
                var callbackObj = (SocketCallbackObject)ar.AsyncState;
                callbackObj.WorkSocket.EndSend(ar);
                if (callbackObj.Callback != null)
                {
                    callbackObj.Callback(true);
                }
            }
            catch (Exception)
            {
                var callbackObj = (SocketCallbackObject)ar.AsyncState;
                callbackObj.WorkSocket.EndSend(ar);
                if (callbackObj.Callback != null)
                {
                    callbackObj.Callback(true);
                }
            }
        }

        void Receive()
        {
            m_stateObject.WorkSocket = m_socket;
            m_socket.BeginReceive(m_stateObject.Buffer, 0, m_stateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), m_stateObject);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket socket = state.WorkSocket;
                int bytesRead = socket.EndReceive(ar);
                if (bytesRead > 0)
                {
                    ProcessBytes(state.Buffer, 0, bytesRead);

                    Receive();
                }
            }
            catch (Exception e)
            {
                if (SocketErrorEvent != null)
                {
                    SocketErrorEvent(this, new SocketErrorEventArgs(e));
                }
            }
        }

        void ProcessBytes(byte[] bytes, int offset, int size)
        {
            if (m_transportState == TransportState.ReadHead)
            {
                ReadHead(bytes, offset, size);
            }
            else if (m_transportState == TransportState.ReadBody)
            {
                ReadBody(bytes, offset, size);
            }
        }

        private void ReadHead(byte[] bytes, int offset, int limit)
        {
            int bytesLen = limit - offset;
            int headLen = HeaderLength - m_bufferOffset;

            if (bytesLen >= headLen)
            {
                Buffer.BlockCopy(bytes, offset, m_headBuffer, m_bufferOffset, headLen);
                offset += headLen;
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(m_headBuffer);
                }
                m_bodyLen = (int)BitConverter.ToUInt16(m_headBuffer, 0);

                m_bodyBuffer = new byte[m_bodyLen];
                // Buffer.BlockCopy(_headBuffer, 0, _bodyBuffer, 0, HeaderLength);

                m_bufferOffset = 0;//HeaderLength;
                m_transportState = TransportState.ReadBody;

                if (offset < limit) ProcessBytes(bytes, offset, limit);
            }
            else
            {
                Buffer.BlockCopy(bytes, offset, m_headBuffer, m_bufferOffset, bytesLen);
                m_bufferOffset += bytesLen;
            }
        }

        private void ReadBody(byte[] bytes, int offset, int limit)
        {
            int bytesLen = limit - offset;
            int bodyLen = m_bodyLen - m_bufferOffset;//_bodyLen + HeaderLength - _bufferOffset;
            // if ((offset + bodyLen) <= limit)
            if (bytesLen >= bodyLen)
            {
                Buffer.BlockCopy(bytes, offset, m_bodyBuffer, m_bufferOffset, bodyLen);
                offset += bodyLen;

                if (SocketMessageEvent != null)
                {
                    SocketMessageEvent(this, new SocketMessageEventArgs(m_bodyBuffer));
                }
                m_bufferOffset = 0;
                m_bodyLen = 0;

                m_transportState = TransportState.ReadHead;

                if (offset < limit)
                {
                    ProcessBytes(bytes, offset, limit);
                }
            }
            else
            {
                Buffer.BlockCopy(bytes, offset, m_bodyBuffer, m_bufferOffset, bytesLen);
                m_bufferOffset += bytesLen;
            }
        }
    }
}