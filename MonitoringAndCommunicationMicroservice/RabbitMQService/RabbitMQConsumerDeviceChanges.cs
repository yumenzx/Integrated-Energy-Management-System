using MonitoringAndCommunicationMicroservice.DataTransferObject;
using MonitoringAndCommunicationMicroservice.Model;
using MonitoringAndCommunicationMicroservice.Services;

namespace MonitoringAndCommunicationMicroservice.RabbitMQService
{
	public class RabbitMQConsumerDeviceChanges : RabbitMQConsumer<DeviceChangeTopicDeserialized>
	{
		public RabbitMQConsumerDeviceChanges(IServiceScopeFactory scopeFactory)
			: base(scopeFactory, queueName: "device_changes_topic", hostname: "rabbitmq_device_changes")
		{
		}

		public RabbitMQConsumerDeviceChanges()
			: base()
		{
		}


		protected override bool ProcessMessage(string? message)
		{
			if (!base.ProcessMessage(message))
				return false;

			Console.WriteLine($"MonitoringMicroservice:: Device Change type: {deserializedObj.ChangeType}");
			

			bool success = false;
			var scope = _scopeFactory.CreateScope();
			try
			{
				var monitoringService = scope.ServiceProvider.GetRequiredService<MonitoringService>();
				success = monitoringService.DeleteMeasurement(deserializedObj.DeviceId);
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR RabitMQ: exceptie la scope(blocul try catch): " + ex.Message);
			}

			if (!success)
			{
				Console.WriteLine($"MonitoringMicroservice:: Stergerea masuratorilor pt deviceid: {deserializedObj.DeviceId} a esuat");
				return false;
			}
			return true;
		}
	}
}
