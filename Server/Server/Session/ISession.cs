using System;
using Common.Api;
namespace IO.Session
{
    public delegate void ProcessRequest(ISession session, Envelope envelope);
    public delegate void Unregister(ISession session);
    public interface ISession
    {
        string ID();
        string UserID();
        void Consume(ProcessRequest processor);
        void Unregister();
        void Send(Envelope envelope);
        void Close();
    }
}
