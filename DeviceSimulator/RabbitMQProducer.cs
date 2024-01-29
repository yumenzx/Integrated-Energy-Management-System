using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator
{
	public class RabbitMQProducer
	{
		private const string queueName = "device_measurements";

		public void SendMessage(DeviceMeasurement measurement)
		{
			var factory = new ConnectionFactory() { HostName = "localhost", Port = 5674, UserName = "guest", Password = "guest" };

			var connection = factory.CreateConnection();

			using var channel = connection.CreateModel();
			channel.QueueDeclare(queue: queueName,
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);


			var json = JsonConvert.SerializeObject(measurement);
			var body = Encoding.UTF8.GetBytes(json);

			channel.BasicPublish(exchange: "",
								 routingKey: queueName,
								 basicProperties: null,
								 body: body);

			//MessageBox.Show($" [x] Sent {measurement}");
		}
	}
}
