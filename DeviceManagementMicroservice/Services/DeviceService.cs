using DeviceManagementMicroservice.DataTransferObject;
using DeviceManagementMicroservice.Model;
using DeviceManagementMicroservice.Repositories;
using DeviceManagementMicroservice.Utilities;
using System.Net.Http.Headers;

namespace DeviceManagementMicroservice.Services
{
	public class DeviceService
	{
		private readonly DeviceRepository _deviceRepository;
		private readonly JwtTokenService _jwtTokenService;

		public DeviceService(DeviceRepository deviceRepository, JwtTokenService jwtTokenService)
		{
			_deviceRepository = deviceRepository;
			_jwtTokenService = jwtTokenService;
		}


		public IEnumerable<Device> GetAllDevices(string token)
		{
			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return null;
			}

			if (!Verify(tokenContent, "administrator"))
				return null;

			return _deviceRepository.GetAllDevices();
		}


		public IEnumerable<Device> GetUserDevices(string token)
		{
			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return null;
			}

			if (!Verify(tokenContent, "client"))
				return null;

			int userId;
			bool success = int.TryParse(tokenContent.UserId.ToString(), out userId);

			if (!success)
				return null;

			return _deviceRepository.GetDevices(userId);
		}

		public string RegisterDevice(string token, DeviceRegister deviceRegister)
		{
			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}

			if (!Verify(tokenContent, "administrator"))
				return "fail";

			var device = new Device();
			device.Address = deviceRegister.Address;
			device.Description = deviceRegister.Description;
			device.MaxHourlyConsumption = deviceRegister.MaxHourlyConsumption;

			bool success = _deviceRepository.InsertDevice(device);

			return success == true ? "success" : "fail";
		}

		public string UpdateDevice(string token, DeviceUpdateDatas newDatas)
		{
			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}

			if (!Verify(tokenContent, "administrator"))
				return "fail";

			bool success = _deviceRepository.UpdateDevice(newDatas);

			return success == true ? "success" : "fail";
			
		}

		public string DeleteDevice(string token, string id)
		{
			int deviceId = -1;
			if (!int.TryParse(id, out deviceId))
			{
				Console.WriteLine("parsarea stringului deviceId la intreg a esuat: " + id);
				return "fail";
			}

			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}

			if (!Verify(tokenContent, "administrator"))
				return "fail";

			bool success = _deviceRepository.DeleteDevice(deviceId);

			if (!success)
				Console.WriteLine("DeviceMicroservice:: stergerea deviceului a esuat");
			else {
				var topic = new DeviceChangeTopic("delete", deviceId);
				RabbitMQProducerService.SendMessage(topic);
			}


			return success == true ? "success" : "fail";
		}

		public string MapDevice(string token, DeviceMapData mapData)
		{
			int deviceId = -1;
			if (!int.TryParse(mapData.DeviceId, out deviceId))
			{
				Console.WriteLine("parsarea stringului deviceId la intreg a esuat: " + mapData.DeviceId);
				return "fail";
			}

			int ownerId = -1;
			if (!int.TryParse(mapData.OwnerId, out ownerId))
			{
				Console.WriteLine("parsarea stringului ownerId la intreg a esuat: " + mapData.OwnerId);
				return "fail";
			}

			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}

			if (!Verify(tokenContent, "administrator"))
				return "fail";

			var userExists = VerifyUserExistency(token, ownerId.ToString());
			userExists.Wait();
			if (userExists.Result == false)
				return "fail";

			bool success = _deviceRepository.MapDevice(deviceId, ownerId);

			return success == true ? "success" : "fail";
		}

		public string UnMapDevice(string token, string deviceId)
		{
			int deviceIdInt = -1;
			if (!int.TryParse(deviceId, out deviceIdInt))
			{
				Console.WriteLine("parsarea stringului deviceId la intreg a esuat: " + deviceId);
				return "fail";
			}

			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}

			if (!Verify(tokenContent, "administrator"))
				return "fail";

			bool success = _deviceRepository.UnMapDevice(deviceIdInt);

			if (!success)
				Console.WriteLine("DeviceMicroservice:: unmaparea deviceului a esuat");
			else
			{
				var topic = new DeviceChangeTopic("unmap", deviceIdInt);
				RabbitMQProducerService.SendMessage(topic);
			}

			return success == true ? "success" : "fail";
		}

		public string UnMapUserDevices(string token, string userId)
		{
			int userIdInt = -1;
			if (!int.TryParse(userId, out userIdInt))
			{
				Console.WriteLine("parsarea stringului deviceId la intreg a esuat: " + userId);
				return "fail";
			}

			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}

			if (!Verify(tokenContent, "administrator"))
				return "fail";

			var devices = _deviceRepository.GetDevices(userIdInt);
			if (devices != null)
			{
				foreach (var device in devices)
				{
					var topic = new DeviceChangeTopic("unmap", device.Id);
					RabbitMQProducerService.SendMessage(topic);
				}
			}

			bool success = _deviceRepository.UnMapUserDevices(userIdInt);

			return success == true ? "success" : "fail";
		}

		private bool Verify(TokenContent tokenContent,string expectedRole)
		{
			var currentTime = DateTime.UtcNow;

			if (currentTime > tokenContent.ExpirationDateTime)
			{
				Console.WriteLine("acest token a expirat");
				return false;
			}

			if (!tokenContent.Role.Equals(expectedRole))
			{
				Console.WriteLine("userul nu are admin " + tokenContent.Role);
				return false;
			}

			return true;
		}

		public int GetDeviceOwner(string deviceId)
		{
			int deviceIdInt = -1;
			var success = int.TryParse(deviceId, out deviceIdInt);
			if (success == false)
			{
				Console.WriteLine("DeviceMicroservice:: GetDeviceOwner from DeviceService: parsarea deviceId a esuat");
				return -1;
			}

			return _deviceRepository.GetDeviceOwner(deviceIdInt);
		}

		public IEnumerable<int> GetDevicesForUser(int userId)
		{
			var devices = _deviceRepository.GetDevicesForUser(userId);

			if(devices == null)
			{
				Console.WriteLine("DeviceMicroservice:: lista deviceurilor este null");
				return Enumerable.Empty<int>();
			}

			return devices;
		}

		private async Task<bool> VerifyUserExistency(string token,string userId)
		{
			string apiUrl = "http://api_gateway:80/api/UserAuthentication/verifyUserExistency/" + userId;

			var httpClient = new HttpClient();

			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();
				if (responseBody.Equals("success"))
					return true;
			}
			else
			{
				Console.WriteLine("Cererea a esuat cu codul " + response.StatusCode);
				return false;
			}

			return false;
		}
	}
}
