using System;
using IO.Net.Tcp;

namespace IO.Session
{
    public class SessionService
    {
        SessionRegistry m_registry;
        TcpServer m_tcpServer;

        public SessionService(SessionRegistry registry, MessageDispacher dispatcher)
        {
            m_registry = registry;

            m_tcpServer = new TcpServer(client => { 
                string userId =  Guid.NewGuid().ToString();
                m_registry.AddTcp(userId, client, (session, envelope) => {
                    dispatcher.Dispatch(session, envelope);
                });
            });
        }

        public void Start()
        {
            m_tcpServer.Listen(7366);
        }
    }
}
