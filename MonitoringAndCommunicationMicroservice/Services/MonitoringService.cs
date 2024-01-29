using MonitoringAndCommunicationMicroservice.DataTransferObject;
using MonitoringAndCommunicationMicroservice.Handlers;
using MonitoringAndCommunicationMicroservice.Model;
using MonitoringAndCommunicationMicroservice.Repositories;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace MonitoringAndCommunicationMicroservice.Services
{
	public class MonitoringService
	{
		private readonly MonitoringRepository _repository;
		private WebSocketHandler _websocketHandler {  get; set; }

		public MonitoringService(MonitoringRepository repository, WebSocketHandler webSocketHandler)
		{
			_repository = repository;
			_websocketHandler = webSocketHandler;
		}

		public bool InsertMeasurement(DeviceMeasurement measurement)
		{
			var latestMeasurement = _repository.GetLeatestMeasurement(measurement.Device_Id);

			bool success = _repository.InsertMeasurement(measurement);

			if (!success)
			{
				Console.WriteLine("MonitoringService:: Inserarea a esuat");
				return false;
			}

			ComputeHourlyConsumption(latestMeasurement, measurement);

			return true;
		}

		public bool DeleteMeasurement(int deviceId)
		{
			return _repository.DeleteMeasurements(deviceId);
		}

		private async Task ComputeHourlyConsumption(DeviceMeasurement? latest, DeviceMeasurement current)
		{
			//get device max consumption and verify.
			int maxHourlyConsumption = 100;
			double computedConsumption = default(double);

			if(latest != null)
			{
				var currentTicks = current.Timestampt.Ticks;
				var latestTicks = latest.Timestampt.Ticks;

				double currentSeconds = (double)currentTicks / TimeSpan.TicksPerSecond;
				double latestSeconds = (double)latestTicks / TimeSpan.TicksPerSecond;

				var consumption = current.Measurement_Value - latest.Measurement_Value;
				var differenceSeconds = currentSeconds - latestSeconds;
				const double seccondsInAHour = 3600;

				computedConsumption = seccondsInAHour * consumption / differenceSeconds;

			}
			else
			{
				computedConsumption = current.Measurement_Value;
			}

			var diff = computedConsumption - maxHourlyConsumption;
			if (diff > 0)
			{
				string message = $"consumul pentru deviceul cu id {current.Device_Id} s-a depășit cu: {diff:0.00}";
				Console.WriteLine("MonitoringService:: consumul s-a depasit cu: " + diff);

				var resp = GetDeviceOwner(current.Device_Id);
				resp.Wait();

				if (resp.IsCompletedSuccessfully)
				{
					var ownerId = resp.Result;
					await _websocketHandler.SendMessageAsync(ownerId, message);
				}
				else
				{
					var ex = resp.Exception;
					Console.WriteLine("MonitoringMicrosrvice:: exceptia la returnarea rezultatului owner id: " + ex.Message);
				}

			}

			return;
		}

		public static async Task<int> GetDeviceOwner(int deviceId)
		{
			string apiUrl = "http://api_gateway:80/api/Device/getDeviceOwner/" + deviceId;

			var httpClient = new HttpClient();

			//httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


			HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();

				// Deserializare JSON in obiect
				DeviceOwnerResponse result = JsonConvert.DeserializeObject<DeviceOwnerResponse>(responseBody);

				// accesarea valorilor din obiect
				if (result.Response == "success")
				{
					int ownerId = result.OwnerId;
					return ownerId;
				}
			}
			else
			{
				Console.WriteLine("Cererea a esuat cu codul " + response.StatusCode);
				return -1;
			}

			return -1;
		}
	}
}
