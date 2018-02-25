using System;
using System.Collections.Generic;
using Google.Protobuf;
using Common.Api;

namespace IO.Net
{
    public class Client : IClient
    {
        public string Host { get; private set; }
        public int Port { get; private set; }

        public event Action DisconnectEvent;

        ITransport m_transport;
        int m_collationId;
        HeartBeatService m_heartbeatService;
        Dictionary<int, KeyValuePair<Action<Envelope>, Action<Error>>> m_collationIds = new Dictionary<int, KeyValuePair<Action<Envelope>, Action<Error>>>();
        Dictionary<Envelope.PayloadOneofCase, Action<Envelope>> m_notifications = new Dictionary<Envelope.PayloadOneofCase, Action<Envelope>>();

        public Client()
        {
            Host = "192.168.1.105";
            Port = 7366;
            m_transport = new TransportTcp();
            m_transport.SocketMessageEvent += (sender, args) =>
            {
                try
                {
                    m_heartbeatService.ResetTime();

                    var envelope = Envelope.Parser.ParseFrom(args.Data);
                    var collationId = envelope.CollationId;
                    KeyValuePair<Action<Envelope>, Action<Error>> pair;
                    if (m_collationIds.TryGetValue(collationId, out pair))
                    {
                        pair.Key(envelope);
                        m_collationIds.Remove(collationId);
                    }
                    else
                    {
                        Action<Envelope> action;
                        if (m_notifications.TryGetValue(envelope.PayloadCase, out action))
                        {
                            action(envelope);
                        }
                    }
                }
                catch (Exception)
                {
                    Cleanup();
                }
            };

            m_transport.SocketClosedEvent += (sender, args) =>
            {
                m_collationIds.Clear();
                if (DisconnectEvent != null)
                {
                    DisconnectEvent();
                }
            };
        }
        public void Connect(Action<bool> callback)
        {
            m_transport.ConnectAsync(string.Format("{0}:{1}", Host, Port), (success) =>
            {
                if (success)
                {
                    m_heartbeatService = new HeartBeatService(3000, this);
                    m_heartbeatService.Start();
                }

                if (callback != null)
                {
                    callback(success);
                }
            });
        }

        public void Cleanup()
        {
            m_transport.Close();
            m_heartbeatService.Stop();
        }

        public void SendCollation(Envelope message, Action<Envelope> onSuccess, Action<Error> onError)
        {
            message.CollationId = m_collationId;
            m_collationId++;

            var pair = new KeyValuePair<Action<Envelope>, Action<Error>>(data => onSuccess(data), onError);
            m_collationIds.Add(m_collationId, pair);

            m_transport.SendAsync(message.ToByteArray(), completed =>
            {
                if (!completed)
                {
                    m_collationIds.Remove(m_collationId);
                }
            });
        }

        public void SendUncollation(Envelope message, Action<bool> onSuccess, Action<Error> onError)
        {
            m_transport.SendAsync(message.ToByteArray(), completed =>
            {
                if (completed)
                {
                    onSuccess(true);
                }
                else
                {
                    onError(new Error("Message send error"));
                }
            });
        }

        public void RegisterNotification(Envelope.PayloadOneofCase messageType, Action<Envelope> callback)
        {
            Action<Envelope> action;
            if (m_notifications.TryGetValue(messageType, out action))
            {
                action += callback;
            }
            else
            {
                action += callback;
                m_notifications.Add(messageType, action);
            }
        }

        public void UnregisterNotification(Envelope.PayloadOneofCase messageType, Action<Envelope> callback)
        {
            Action<Envelope> action;
            if (m_notifications.TryGetValue(messageType, out action))
            {
                action -= callback;
            }
        }
    }
}