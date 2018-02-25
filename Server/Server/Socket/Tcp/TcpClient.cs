using System;
using System.Net.Sockets;

namespace IO.Net.Tcp
{

    enum ReadState
    {
        ReadHead = 0,
        ReadBody = 1,
    }

    public delegate void CloseClient(TcpClient client);
    public class TcpClient
    {
        
        const int HeaderLength = 2;

        string m_id;
        public string Id 
        {
            set
            {
                m_id = value;
            }
            get 
            {
                return m_id;
            }
        }

        Socket m_socket;
        public Socket Socket 
        {
            set 
            {
                m_socket = value;
                m_networkStream = new NetworkStream(m_socket);
            }
        }

        NetworkStream m_networkStream;
        byte[] m_headBuffer = new Byte[HeaderLength];
        CloseClient m_clientCloseEvent;
        public TcpClient(CloseClient closeClient)
        {
            m_clientCloseEvent = closeClient;
        }

        public void SendAsync(byte[] data)
        {
            m_networkStream.BeginWrite(data, 0, data.Length,ar => {
                try
                {
                    NetworkStream networkStream = (NetworkStream)ar.AsyncState;
                    networkStream.EndWrite(ar);
                }
                catch(Exception)
                {
                    Close();
                }
            }, m_networkStream);
        }

        public byte[] Read()
        {
          int len =  m_networkStream.Read(m_headBuffer, 0, HeaderLength);
            if(len == 0)
            {
                return null;
            }
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(m_headBuffer);
            }
           int  bodyLen = (int)BitConverter.ToUInt16(m_headBuffer, 0);

            byte[] data = new byte[bodyLen];
            len = m_networkStream.Read(data, 0, bodyLen);
            if(len == 0)
            {
                return null;
            }
            return data;
        }

        public void Close()
        {
            if(m_clientCloseEvent != null)
            {
                m_clientCloseEvent(this);
            }
        }

        public void Cleanup()
        {
            try
            {
                m_socket.Shutdown(SocketShutdown.Both);
                m_networkStream.Close();
            }
            catch (Exception)
            {

            }
            m_socket.Close();
        }
    }
}
