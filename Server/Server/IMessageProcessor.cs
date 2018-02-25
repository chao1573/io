using IO.Session;
using Common.Api;

namespace IO
{
    public interface IMessageProcessor
    {
        void Process(ISession session, Envelope envelope);
    }
}
