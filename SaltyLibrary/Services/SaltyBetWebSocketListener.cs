using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebSocketSharp;

namespace SaltyLibrary.Services
{
    public class SaltyBetWebSocketListener : IDisposable
    {
        public const string WS_URL = "wss://www.saltybet.com:2096/socket.io/?EIO=3&transport=websocket";
        public const int PING_INTERVAL = 25000;

        public event EventHandler<MessageEventArgs> MessageReceived;
        private WebSocket ws;
        private Thread pingThread;

        public bool Running { get; private set; } = false;

        public SaltyBetWebSocketListener(EventHandler<MessageEventArgs> messageReceivedAction)
        {
            ws = new WebSocket(WS_URL);
            MessageReceived += messageReceivedAction;
            ws.OnMessage += Ws_OnMessage;

            pingThread = new Thread(() => {
                while (Running)
                {
                    Thread.Sleep(PING_INTERVAL);
                    ws.Send("2");
                }
                Console.WriteLine("WebSocket thread successfully terminated!");
            });
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine(e.Reason);
        }

        public void Connect()
        {
            if (Running == false)
            {
                try
                {
                    ws.Connect();
                    pingThread.Start();
                    Running = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occurred while trying to connect the websocket!");
                }
            }
            else
            {
                Console.WriteLine("WebSocket is already running!");
            }
        }

        public void Dispose()
        {
            Running = false;
            Console.WriteLine("Shutting down WebSocket thread please wait...");
            ws.Close();
        }

        protected virtual void OnMessageReceived(object sender, MessageEventArgs e)
        {
            MessageReceived?.Invoke(sender, e);
        }
        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            OnMessageReceived(sender, e);
        }
    }
}
