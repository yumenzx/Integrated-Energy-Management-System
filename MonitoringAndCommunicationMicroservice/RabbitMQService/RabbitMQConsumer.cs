using MonitoringAndCommunicationMicroservice.DataTransferObject;
using MonitoringAndCommunicationMicroservice.Model;
using MonitoringAndCommunicationMicroservice.Services;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace MonitoringAndCommunicationMicroservice.RabbitMQService
{
	public class RabbitMQConsumer<T>
	{
		private readonly IModel _channel;
		private readonly string _queueName;

		protected readonly IServiceScopeFactory _scopeFactory;

		protected T deserializedObj;

		public RabbitMQConsumer(IServiceScopeFactory scopeFactory, string queueName, string hostname)
		{
			_scopeFactory = scopeFactory;
			_queueName = queueName;
			//HostName = localhost; pt a rula pe sistemul principal
			var factory = new ConnectionFactory() { HostName = hostname }; // adresa server rabitmq
			var connection = factory.CreateConnection();

			_channel = connection.CreateModel();
			_channel.QueueDeclare(queue: _queueName,
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);
		}

		public RabbitMQConsumer()
		{
		}

		~RabbitMQConsumer()
		{
			StopConsuming();
		}

		public void StartConsuming()
		{
			Console.WriteLine("OK RabbitMQ:: waiting for measurements datas");
			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, eventArgs) =>
			{
				var body = eventArgs.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);

				// procesare mesaj
				ProcessMessage(message);

			};
			_channel.BasicConsume(queue: _queueName,
								 autoAck: true,
								 consumer: consumer);
		}

		public void StopConsuming()
		{
			_channel.Close();
			_channel.Dispose();
		}

		protected virtual bool ProcessMessage(string? message)
		{
			Console.WriteLine("Received message: {0}", message);

			if (message == null)
			{
				Console.WriteLine("Mesajul primit este null");
				return false;
			}

			// deserializare mesaj
			deserializedObj = JsonConvert.DeserializeObject<T>(message);

			if (deserializedObj == null)
			{
				Console.WriteLine("Deserializarea mesajului a esuat");
				return false;
			}

			return true;
		}
	}
}
