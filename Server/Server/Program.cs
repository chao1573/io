using System;
using IO.Session;
using IO.Net.Tcp;
using System.Threading;

namespace IO
{
    class MainClass
    {
        static AutoResetEvent autoEvent = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MatchService matchService = new MatchService();
            MessageDispatcher dispatcher = new MessageDispatcher(matchService);
            SessionRegistry sessionRegistry = new SessionRegistry();

            SessionService sessionService = new SessionService(sessionRegistry, dispatcher);
            sessionService.Start();
            Console.WriteLine("Server start.");

            autoEvent.WaitOne();
            Console.WriteLine("Server exit.");
        }
    }
}
