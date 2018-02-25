using System;
using Common.Api;
using IO.Net.Tcp;
using Google.Protobuf;

namespace IO.Session
{
    public class SessionTcp:ISession
    {
        const int HeaderLength = 2;
        string m_id;
        string m_userId;
        Unregister m_unregisterEvent;
        TcpClient m_client;

        public SessionTcp(string userId, TcpClient client, Unregister unregister)
        {
            m_id = Guid.NewGuid().ToString();
            m_userId = userId;
            m_client = client;
            m_unregisterEvent = unregister;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Consume(ProcessRequest processor)
        {
            try
            {
                while (true)
                {
                    var data = m_client.Read();
                    if(data == null)
                    {
                        Console.WriteLine("Read empty data");
                        CleanupClosedConnection();
                        break;
                    }

                    var envelope = Envelope.Parser.ParseFrom(data);
                    if (envelope.PayloadCase == Envelope.PayloadOneofCase.Heartbeat)
                    {
                        Console.WriteLine("Heartbeat");
                        Send(envelope);
                    }
                    else
                    {
                        processor(this, envelope);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Read error "+e.Message);
                CleanupClosedConnection();
            }
        }

        void CleanupClosedConnection()
        {
            if (m_unregisterEvent != null)
                m_unregisterEvent(this);
            m_client.Close();
        }

        public string ID()
        {
            return m_id;
        }

        public string UserID()
        {
            return m_userId;
        }

        public void Send(Envelope envelope)
        {
            byte[] data = envelope.ToByteArray();
            int bodyLength = data.Length;
            byte[] buf = new byte[HeaderLength + bodyLength];
            byte[] lengthBytes = BitConverter.GetBytes((ushort)bodyLength);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBytes, 0, lengthBytes.Length);
            }
            Buffer.BlockCopy(lengthBytes, 0, buf, 0, lengthBytes.Length);
            Buffer.BlockCopy(data, 0, buf, HeaderLength, data.Length);

            m_client.SendAsync(buf);
        }

        public void Unregister()
        {
            if(m_unregisterEvent!=null)
            {
                m_unregisterEvent(this);
            }
        }
    }
}
