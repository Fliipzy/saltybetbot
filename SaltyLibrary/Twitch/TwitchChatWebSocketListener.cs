using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebSocketSharp;

namespace SaltyLibrary.Twitch
{
    public class TwitchChatWebSocketListener : IDisposable
    {
        private const int PING_INTERVAL = 25000;
        private const string PING_MSG = "PING";

        private WebSocket webSocket;
        private Thread wsThread;
        private bool t_running = false;

        public event EventHandler<MessageEventArgs> ModMessageReceived;

        public TwitchChatWebSocketListener(EventHandler<MessageEventArgs> messageReceivedAction)
        {
            webSocket = new WebSocket("wss://irc-ws.chat.twitch.tv/");
            webSocket.OnMessage += WebSocket_OnMessage;
            ModMessageReceived += messageReceivedAction;
        }

        public void Connect()
        {
            wsThread = new Thread(() => { 
                //Join message sequence
                webSocket.Connect();
                webSocket.Send("CAP REQ :twitch.tv/tags twitch.tv/commands");
                webSocket.Send($"PASS SCHMOOPIIE");
                webSocket.Send($"NICK justinfan1773");
                webSocket.Send($"USER justinfan1773 8 * :justinfan1773");
                Thread.Sleep(1000);
                webSocket.Send("JOIN #saltybet");

                t_running = true;

                //ping loop
                while (t_running)
                {
                    Thread.Sleep(PING_INTERVAL);
                    webSocket.Send(PING_MSG);
                }
            });

            wsThread.Start();
        }

        protected virtual void OnMessageReceived(object sender, MessageEventArgs e)
        {
            ModMessageReceived?.Invoke(sender, e);
        }

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Data.Length > 50 && e.Data.Substring(0, 50).Contains("moderator"))
            {
                OnMessageReceived(sender, e);
            }
        }

        public void Dispose()
        {
            t_running = false;
            webSocket.Close();
        }
    }
}
