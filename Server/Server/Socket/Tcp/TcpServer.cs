using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace IO.Net.Tcp  
{
    public delegate void TcpClientConnect(TcpClient client);
    public class TcpServer
    {
        TcpClientConnect m_clientConnectEvent;
        Socket m_listenSocket;
        int m_numConnectedSockets;
        ConcurrentDictionary<string, TcpClient> m_clients;

        public TcpServer(TcpClientConnect onConnect)
        {
            m_clients = new ConcurrentDictionary<string, TcpClient>();
            m_clientConnectEvent = onConnect;
        }

        public void Listen(int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            m_listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_listenSocket.Bind(localEndPoint);
            m_listenSocket.Listen(100);

            StartAccept();
        }

        void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request.  
            Socket socket = (Socket) ar.AsyncState;  
            Socket handler = socket.EndAccept(ar);  

            Interlocked.Increment(ref m_numConnectedSockets);
            Console.WriteLine("Client connection accepted. There are {0} clients connected to the server",
            m_numConnectedSockets);

            TcpClient client = new TcpClient(CloseClient);
            client.Socket = handler;
            client.Id = handler.RemoteEndPoint.ToString();
            m_clients[client.Id] = client;

            if (m_clientConnectEvent != null)
            {
                Task.Factory.StartNew(() => {
                    m_clientConnectEvent(client);
                });
            }

            StartAccept();
        }

        public void Stop()
        {
            foreach(var item in m_clients)
            {
                item.Value.Close();
            }
            m_clients.Clear();

            m_listenSocket.Shutdown(SocketShutdown.Both);
            m_listenSocket.Close();
        }


        void StartAccept()
        {
            m_listenSocket.BeginAccept(   
                    new AsyncCallback(AcceptCallback),  
                                           m_listenSocket ); 
        }

        void CloseClient(TcpClient client)
        {
            TcpClient c;
            if(m_clients.TryRemove(client.Id, out c))
            {
                Interlocked.Decrement(ref m_numConnectedSockets);
                Console.WriteLine("A client has been disconnected from the server. There are {0} clients connected to the server", m_numConnectedSockets);
                client.Cleanup();
            }
        }
    }
}
