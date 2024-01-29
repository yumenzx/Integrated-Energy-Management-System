using DeviceManagementMicroservice.DataTransferObject;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DeviceManagementMicroservice.Services
{
	public class RabbitMQProducerService
	{
		private const string queueName = "device_changes_topic";

		public static void SendMessage(DeviceChangeTopic topic)
		{
			var factory = new ConnectionFactory() { HostName = "rabbitmq_device_changes", Port = 5672, UserName = "guest", Password = "guest" };

			var connection = factory.CreateConnection();

			using var channel = connection.CreateModel();
			channel.QueueDeclare(queue: queueName,
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);


			var json = JsonConvert.SerializeObject(topic);
			var body = Encoding.UTF8.GetBytes(json);

			channel.BasicPublish(exchange: "",
								 routingKey: queueName,
								 basicProperties: null,
								 body: body);

			Console.WriteLine($"DeviceMicroservice:: Sent through RabbitMQ: {topic}");
		}
	}
}
