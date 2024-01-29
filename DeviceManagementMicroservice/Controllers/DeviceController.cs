using DeviceManagementMicroservice.DataTransferObject;
using DeviceManagementMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementMicroservice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DeviceController : ControllerBase
	{
		private readonly DeviceService _deviceService;

		public DeviceController(DeviceService deviceService)
		{
			_deviceService = deviceService;
		}

		[Authorize]
		[HttpPost]
		[Route("getUserDevices")]
		public IActionResult GetUserDevices([FromBody] UserId id)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var d = _deviceService.GetUserDevices(token);

			return Ok(new { response = "success", devices = d });
		}

		[Authorize]
		[HttpGet]
		[Route("getAllDevices")]
		public IActionResult GetAllDevices()
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var d = _deviceService.GetAllDevices(token);

			return Ok(new { response = "success", devices = d });
		}

		[Authorize]
		[HttpPost]
		[Route("insertDevice")]
		public IActionResult InsertDevice([FromBody] DeviceRegister device)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");


			var response = _deviceService.RegisterDevice(token, device);

			return Ok(response);
		}

		[Authorize]
		[HttpPut]
		[Route("updateDevice")]
		public IActionResult UpdateDevice([FromBody] DeviceUpdateDatas updateDatas)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var response = _deviceService.UpdateDevice(token, updateDatas);

			return Ok(response);
		}

		[Authorize]
		[HttpDelete]
		[Route("deleteDevice/{id}")]
		public IActionResult DeleteDevice(string id)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var response = _deviceService.DeleteDevice(token, id);
			return Ok(response);
		}

		[Authorize]
		[HttpPut]
		[Route("mapDevice")]
		public IActionResult MapDevice([FromBody] DeviceMapData mapData)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var response = _deviceService.MapDevice(token, mapData);
			return Ok(response);
		}

		[Authorize]
		[HttpPut]
		[Route("unMapDevice")]
		public IActionResult UnMapDevice([FromBody] DeviceUnMapData deviceId)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var response = _deviceService.UnMapDevice(token, deviceId.DeviceId);
			return Ok(response);
		}

		[Authorize]
		[HttpPut]
		[Route("unMapUserDevices")]
		public IActionResult UnMapUserDevices([FromBody] DeviceUnMapData userId)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var response = _deviceService.UnMapUserDevices(token, userId.DeviceId);
			return Ok(response);
		}

		private string? GetTokenFromRequestHeader()
		{
			string? token = default(string); //null
			try
			{
				token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Eroare la extragerea tokenului: " + ex.Message);
			}

			return token;
		}

		[HttpGet]
		[Route("getDeviceOwner/{deviceId}")]
		public IActionResult GetDeviceOwner(string deviceId)
		{
			/*var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");
			*/
			var owner = _deviceService.GetDeviceOwner(deviceId);

			if(owner != -1)
				return Ok(new { response = "success", ownerId = owner });
			return Ok(new { response = "fail", ownerId = -1 });
		}

		[HttpGet]
		[Route("getUserDevices/{userId}")]
		public Task<GetDevicesForUserResponse> getDevicesForUser(int userId)
		{
			/*var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");
			*/

			var d = _deviceService.GetDevicesForUser(userId);

			var resp = new GetDevicesForUserResponse();
			resp.Response = "success";
			resp.Devices = d.ToArray();

			return Task.FromResult(resp);
		}
	}
}
