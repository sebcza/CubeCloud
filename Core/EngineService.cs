using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DB.Models;
using DB.Repository;
using Microsoft.AspNetCore.Http;

namespace Core
{
    public class EngineService
    {
        public readonly ICubeRepository<Cube> _cubeRepository;

        public WebSocket _messageSocket;

        public EngineService(ICubeRepository<Cube> cubeRepository)
        {
            _cubeRepository = cubeRepository;
        }

        public async void ProccessMessage(String rawMessage)
        {
            var decodedMessage = DecodeMessage(rawMessage);
            var address = decodedMessage[0];

            var engineCube = await CreateDeliveryCube(address);
            engineCube.ProcessMessage(rawMessage);

        }

        public List<string> DecodeMessage(string message)
        {
            String[] messageItems = message.Split(new char[] { '|' });
            return messageItems.ToList();
        }

        public async Task Echo(HttpContext context, WebSocket webSocket)
        {
            
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public void SendMessage(string message)
        {
            //Logger.LogInfo("Send message: " + message);
            byte[] msg = Encoding.ASCII.GetBytes(message);
            try
            {
             //   _connectedSocketEndpoint.Send(msg);
            }
            catch (NullReferenceException)
            {

              //  Logger.LogError("Any endpoint is not connected");
            }
        }

        public async Task<Cube> CreateDeliveryCube(string address)
        {
            Cube addressedCube = await _cubeRepository.GetCubeAsync(address);
            if (addressedCube == null)
            {
             //   Logger.LogError("Not found cube addressed: " + address);
            }
            var cubeType = GetType().Assembly.GetTypes()
                .FirstOrDefault(x => x.Name.Contains(addressedCube.Type));
            Object[] args = { addressedCube, _cubeRepository, this };
            Cube cube = (Cube)Activator.CreateInstance(cubeType, args);
            return cube;
        }
    }
}
