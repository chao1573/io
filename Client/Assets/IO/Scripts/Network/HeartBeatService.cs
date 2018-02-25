using System.Timers;
using System;
using Common.Api;

namespace IO.Net
{
    public class HeartBeatService
    {
        int m_interval;
        // int m_elapsedTime;
        DateTime m_lastTime;
        IClient m_client;
        Timer m_timer;
        Envelope m_heartbeatData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval">heartbeat interval in milliseconds</param>
        /// <param name="client">client to send heartbeat</param>
        public HeartBeatService(int interval, IClient client)
        {
            m_interval = interval;
            m_client = client;
            m_heartbeatData = new Envelope { Heartbeat = new Heartbeat() };
        }

        public void ResetTime()
        {
            m_lastTime = DateTime.Now;
        }

        public void Start()
        {
            m_timer = new Timer();
            m_timer.Interval = m_interval;
            m_timer.Elapsed += new ElapsedEventHandler(SendHeartBeat);
            m_timer.Enabled = true;

            m_lastTime = DateTime.Now;
        }

        public void Stop()
        {
            m_timer.Enabled = false;
            m_timer.Dispose();
        }

        void SendHeartBeat(object source, ElapsedEventArgs e)
        {
            TimeSpan span = DateTime.Now - m_lastTime;
            float elapsedTime = (int)span.TotalMilliseconds;
            if (elapsedTime > m_interval * 2)
            {
                m_client.Cleanup();
                return;
            }

            m_client.SendUncollation(m_heartbeatData, null, null);
        }
    }
}