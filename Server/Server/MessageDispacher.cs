using System;
using Common.Api;
using IO.Session;
using System.Collections.Generic;

namespace IO
{
    public class MessageDispacher
    {
        Dictionary<Envelope.PayloadOneofCase, IMessageProcessor> m_processors = new Dictionary<Envelope.PayloadOneofCase, IMessageProcessor>();

        public MessageDispacher(MatchService matchService)
        {
            m_processors[Envelope.PayloadOneofCase.Groups] = matchService;
        }

        public void Dispatch(ISession session, Envelope envelope)
        {
            IMessageProcessor processor;
            if(m_processors.TryGetValue(envelope.PayloadCase, out processor))
            {
                processor.Process(session, envelope);
            }
            else
            {
                Console.WriteLine("No processor for "+envelope.PayloadCase);
            }
        }
    }
}
