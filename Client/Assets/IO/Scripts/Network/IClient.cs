using System;
using Common.Api;

namespace IO.Net
{
    public interface IClient
    {
        event Action DisconnectEvent;
        void Connect(Action<bool> callback);
        void Cleanup();
        void SendCollation(Envelope message, Action<Envelope> onSuccess, Action<Error> onError);
        void SendUncollation(Envelope message, Action<bool> onSuccess, Action<Error> onError);
        void RegisterNotification(Envelope.PayloadOneofCase messageType, Action<Envelope> callback);
        void UnregisterNotification(Envelope.PayloadOneofCase messageType, Action<Envelope> callback);
    }
}
