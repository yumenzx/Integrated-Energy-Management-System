using MonitoringAndCommunicationMicroservice.Services;
using MonitoringAndCommunicationMicroservice.Model;
using MonitoringAndCommunicationMicroservice.DataTransferObject;

namespace MonitoringAndCommunicationMicroservice.RabbitMQService
{
	public class RabbitMQConsumerDeviceMeasurements : RabbitMQConsumer<DeviceMeasurementDeserialized>
	{
		public RabbitMQConsumerDeviceMeasurements(IServiceScopeFactory scopeFactory)
			: base(scopeFactory, queueName: "device_measurements", hostname: "rabbitmq_device_measurements")
		{	
		}

		public RabbitMQConsumerDeviceMeasurements()
			: base()
		{ 
		}



		protected override bool ProcessMessage(string? message)
		{
			if (!base.ProcessMessage(message))
				return false;

			var resp = MonitoringService.GetDeviceOwner(deserializedObj.Device_Id);
			resp.Wait();
			int ownerId = -1;
			if (resp.IsCompletedSuccessfully)
			{
				ownerId = resp.Result;
			}
			else
			{
				var ex = resp.Exception;
				Console.WriteLine("MonitoringMicrosrvice:: exceptia la returnarea rezultatului owner id: " + ex.Message);
				return false;
			}

			if(ownerId == -1)
			{
				Console.WriteLine("MonitoringService:: deviceul cu id: " +  deserializedObj.Device_Id + " nu a fost gasit sau nu este mapat");
				return false;
			}

			var measurement = new DeviceMeasurement();
			measurement.Timestampt = deserializedObj.Timestampt;
			measurement.Device_Id = deserializedObj.Device_Id;
			measurement.Measurement_Value = deserializedObj.Measurement_Value;
			
			bool success = false;
			var scope = _scopeFactory.CreateScope();
			try
			{
				var monitoringService = scope.ServiceProvider.GetRequiredService<MonitoringService>();
				success = monitoringService.InsertMeasurement(measurement);
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR RabitMQ: exceptie la scope(blocul try catch): " + ex.Message);
			}

			if (!success)
			{
				Console.WriteLine("Inserarea masurarii a esuat");
				return false;
			}
			return true;
		}
	}
}
