using MonitoringAndCommunicationMicroservice.DataTransferObject;
using MonitoringAndCommunicationMicroservice.Model;
using MonitoringAndCommunicationMicroservice.Repositories;
using Newtonsoft.Json;

namespace MonitoringAndCommunicationMicroservice.Services
{
	public class ConsumptionService
	{
		private readonly ConsumptionRepository _repository;

		public ConsumptionService(ConsumptionRepository repository)
		{
			_repository = repository;
		}


		public double[] GetChartData(GetChartDataDTO request)
		{
			var resp = GetUserDevices(request.UserId);
			resp.Wait();

			IEnumerable<int> deviceIds = resp.Result;

			if (deviceIds == null)
			{
				Console.WriteLine("MonitoringService:: lista deviceurilor este null");
				return new double[24];
			}

			if (deviceIds.Count() == 0)
			{
				Console.WriteLine("MonitoringService:: lista deviceurilor este gol");
				return new double[24];
			}

			var d = request.Timestampt;
			var date = new DateTime(d.Year, d.Month, d.Day);

			var hourlyConsumptions = ComputeHourlyConsumptions(deviceIds, date);
			return hourlyConsumptions;
		}

		private double[] ComputeHourlyConsumptions(IEnumerable<int> deviceIds, DateTime date) 
		{
			double[] hourlyConsumptions = new double[24];
			Array.Clear(hourlyConsumptions, 0, 24);

			foreach (var deviceId in deviceIds)
			{
				List<DeviceMeasurement> measurements = _repository.GetDeviceMeasurements(deviceId, date);

				var consumPeOra = measurements
					.OrderBy(m => m.Timestampt)
					.GroupBy(m => new { m.Timestampt.Year, m.Timestampt.Month, m.Timestampt.Day, m.Timestampt.Hour })
					.Select(grp => new
					{
						Ora = grp.Key.Hour,
						ConsumEstimat = grp.Max(m => m.Measurement_Value) - grp.Min(m => m.Measurement_Value)
					});


				foreach (var consumOra in consumPeOra)
				{
					Console.WriteLine($"Ora {consumOra.Ora}: Consum estimat = {consumOra.ConsumEstimat}");
					hourlyConsumptions[consumOra.Ora] += consumOra.ConsumEstimat;
				}
			}

			for (int i = 0; i < hourlyConsumptions.Length; i++)
			{
				var consuptionRounded = Math.Round(hourlyConsumptions[i], 2);
				hourlyConsumptions[i] = consuptionRounded;
			}
			
			return hourlyConsumptions;
		}


		private async Task<IEnumerable<int>> GetUserDevices(int userId)
		{
			string apiUrl = "http://api_gateway:80/api/Device/getUserDevices/" + userId;

			var httpClient = new HttpClient();

			//httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			IEnumerable<int> deviceIdList = new List<int>();

			HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();

				// Deserializare JSON în obiect
				DeviceIdListResponse result = JsonConvert.DeserializeObject<DeviceIdListResponse>(responseBody);

				// Acum poți accesa valorile din obiect
				if (result.Response == "success")
				{
					var x = result.Devices;
					return x;
				}
				else
				{
					Console.WriteLine("MonitorindService:: responsul cererii este: " + result.Response);
					return deviceIdList;
				}
			}
			else
			{
				Console.WriteLine("Cererea a esuat cu codul " + response.StatusCode);
				return deviceIdList;
			}

		}
	}
}
