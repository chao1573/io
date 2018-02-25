using System;
using Common.Api;
using IO.Session;

namespace IO
{
    public class MatchService : IMessageProcessor
    {
        public MatchService()
        {
        }

        public void Process(ISession session, Envelope envelope)
        {
            Console.WriteLine(session.UserID() +  envelope.ToString());
            //throw new NotImplementedException();
        }
    }
}
