using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Models;
using DB.Repository;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;

namespace Core
{
    public class EngineService : IEngineService
    {
        public readonly ICubeRepository<Cube> _cubeRepository;
	    private readonly ILogger<EngineService> _logger;

	    private IMqttClient _mqttClient;

        public EngineService(ICubeRepository<Cube> cubeRepository, ILogger<EngineService> logger)
        {
            _cubeRepository = cubeRepository;
	        _logger = logger;
	        SetUp();
        }

	    public void SetUp()
	    {
		    _logger.LogInformation("", "Getting item {ID}", 1);
		    ConnectToBrokerAsync();
		    SubscribeCloudTopic();
		    _mqttClient.ApplicationMessageReceived += ProccessMessage;
	    }
	    
	    public async Task SendMessage(string message, string destinationDeviceAddress)
	    {
		    var messageMqtt = new MqttApplicationMessageBuilder()
			    .WithTopic(destinationDeviceAddress)
			    .WithPayload(message)
			    .WithExactlyOnceQoS()
			    .WithRetainFlag()
			    .Build();

		    await _mqttClient.PublishAsync(messageMqtt);
	    }	    

	    private async Task ConnectToBrokerAsync()
	    {
		    var mqttFacotry = new MqttFactory();
		    _mqttClient = mqttFacotry.CreateMqttClient();
		    var options = new MqttClientOptionsBuilder()
			    .WithTcpServer("127.0.0.1", 1883)
			    .Build();
		    await _mqttClient.ConnectAsync(options);
	    }

	    private void SubscribeCloudTopic()
	    {
		    _mqttClient.Connected += async (s, e) =>
		    {
			    Console.WriteLine("### CONNECTED WITH SERVER ###");

			    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("CubeCloud").Build());

			    Console.WriteLine("### SUBSCRIBED ###");
		    };
	    }

	    private async void ProccessMessage(object s, MqttApplicationMessageReceivedEventArgs e)
        {
	        Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
	        Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
	        Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
	        Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
	        Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
	        Console.WriteLine();
	        
/*            var decodedMessage = DecodeMessage(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
            var address = decodedMessage[0];

            var engineCube = await CreateDeliveryCube(address);
            engineCube.ProcessMessage(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));*/

        }

	    private List<string> DecodeMessage(string message)
        {
            String[] messageItems = message.Split(new char[] { '|' });
            return messageItems.ToList();
        }

        private async Task<Cube> CreateDeliveryCube(string address)
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
