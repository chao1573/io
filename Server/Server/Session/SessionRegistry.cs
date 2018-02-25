using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using IO.Net.Tcp;

namespace IO.Session
{
    public class SessionRegistry
    {
        ConcurrentDictionary<string, ISession> m_sessions = new ConcurrentDictionary<string, ISession>();

        public SessionRegistry()
        {
        }

        public void AddTcp(string userId, TcpClient client, ProcessRequest processor)
        {
            var session = new SessionTcp(userId, client, Remove);
            m_sessions[session.ID()] = session;

            session.Consume(processor);
        }

        void Remove(ISession session)
        {
            ISession s;
            m_sessions.TryRemove(session.ID(), out s);
        }
    }
}
