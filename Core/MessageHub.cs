using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Core
{
    public class MessageHub : Controller
    {
        public async void Index(){
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }           
        }
    }
}
