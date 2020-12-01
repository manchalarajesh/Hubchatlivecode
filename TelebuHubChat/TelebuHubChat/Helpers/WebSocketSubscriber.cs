using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Text;

namespace TelebuHubChat.Helpers
{
    public static class WebSocketSubscriber
    {
        public static WebSocketSharp.WebSocket ws = null;
        public static void pushSocket(string socketUrl, JObject socketObj)
        {
            if (ws == null)
            {
                ws = new WebSocketSharp.WebSocket(socketUrl);
            }
            if (ws.IsAlive == false)
            {
                LogClasses.LogProperties.info("WebSocket Is Alive : " + ws.IsAlive.ToString());
                ws.Connect();
            }

            if (ws.IsAlive == true)
            {
                LogClasses.LogProperties.info("socket data send: " + socketObj.ToString());
                ws.Send(socketObj.ToString());



            }
            ws.OnMessage += (sender, e) =>
            {
                LogClasses.LogProperties.info("WebSocket OnMessage : " + e.Data.ToString());
            };
            ws.OnClose += (sender, e) =>
            {
                LogClasses.LogProperties.error("WebSocket OnClose : CODE IS " + e.Code.ToString() + " reason is " + e.Reason.ToString());
                ws.Connect();
            };
            ws.OnError += (sender, e) =>
            {
                LogClasses.LogProperties.error("WebSocket OnError exception : " + e.Exception.ToString() + " message is : " + e.Message.ToString());
            };

        }
        //public async Task postsAsync(string socketurl ,JObject message )
        //{
        //    using (ClientWebSocket ws = new ClientWebSocket())
        //    {
        //        Uri serverUri = new Uri(socketurl);
        //        await ws.ConnectAsync(serverUri, CancellationToken.None);
        //       // Logger.Info("Connected To WebSocket", LogTarget.PUBLISHER);


        //            ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message.ToString()));
        //            await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
        //        //}

        //    }
        //}
        //public static WebSocket ws = null;
        //public static void pushSocket(JObject socketObj, string socketUrl)
        //{
        //    if (ws == null)
        //    {
        //        ws = new WebSocket(socketUrl);
        //    }
        //    if (ws.IsAlive == false)
        //    {
        //       // LogClasses.LogProperties.info("WebSocket Is Alive : " + ws.IsAlive.ToString());
        //        ws.Connect();
        //    }

        //    if (ws.IsAlive == true)
        //    {
        //        ws.Send(socketObj.ToString());
        //    }
        //    ws.OnMessage += (sender, e) =>
        //    {
        //       // LogClasses.LogProperties.info("WebSocket OnMessage : " + e.Data.ToString());
        //    };
        //    ws.OnClose += (sender, e) =>
        //    {
        //      //  LogClasses.LogProperties.info("WebSocket OnClose : CODE IS " + e.Code.ToString() + " reason is " + e.Reason.ToString());
        //        ws.Connect();
        //    };
        //    ws.OnError += (sender, e) =>
        //    {
        //      //  LogClasses.LogProperties.info("WebSocket OnError exception : " + e.Exception.ToString() + " message is : " + e.Message.ToString());
        //    };

        //}
    }
}
