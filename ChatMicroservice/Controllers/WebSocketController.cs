using ChatMicroservice.DataTransferObject;
using ChatMicroservice.Handlers;
using ChatMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net.WebSockets;

namespace ChatMicroservice.Controllers
{ 
	public class WebSocketController : ControllerBase
	{
		private WebSocketHandler _webSocketHandler { get; set; }
		private readonly TokenValidationService _validationService;

		public WebSocketController(WebSocketHandler webSocketHandler, TokenValidationService validationService)
		{
			_webSocketHandler = webSocketHandler;
			_validationService = validationService;
		}



		[HttpGet("/ws/{token}")]
		public async Task Get(string token)
		{
			if (HttpContext.WebSockets.IsWebSocketRequest)
			{
				var socketFinishedTcs = new TaskCompletionSource<object>();

				WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
				
				var webSocketPipeline = new WebSocketPipeline();
				webSocketPipeline.WebSocket = webSocket;
				webSocketPipeline.TaskCompletionSource = socketFinishedTcs;

				var values = _validationService.ValidateToken(token);

				bool success = _validationService.ParseTokenValues(values, out int userId, out UserRoleType userRole);
				if (!success)
					return;

				_webSocketHandler.OnConnected(webSocketPipeline, userId, userRole);
				Console.WriteLine("MonitoringService:: userul cu id-ul: " + userId + " s-a conectat");

				await socketFinishedTcs.Task;
			}
			else
			{
				HttpContext.Response.StatusCode = 400;
			}
		}

		[HttpDelete("/ws/{token}")]
		public IActionResult Disconnect(string token)
		{
			var values = _validationService.ValidateToken(token);

			bool success = _validationService.ParseTokenValues(values, out int userId, out UserRoleType userRole);
			if (!success)
				return Ok("validarea a esuat");

			_webSocketHandler.Disconect(userId);
			Console.WriteLine("MonitoringService:: userul cu id-ul: " + userId + " s-a deconectat");
			return Ok("Conexiunea websocket inchisa");
		}

		/*
		private bool VerifyTokenValues(TokenContent? values, out int userId, out UserRoleType userRoleType)
		{
			userId = -1; // pt a putea folosi out
			userRoleType = UserRoleType.None; // altfel as putea folosi si ref

			if (values == null)
			{
				Console.WriteLine("Validarea a esuat");
				return false;
			}

			bool success = int.TryParse(values.UserId, out userId);
			if (!success)
			{
				Console.WriteLine("parsarea a esuat");
				return false;
			}

			var userRole = values.Role;
			switch (userRole)
			{
				case "client":
					userRoleType = UserRoleType.Client;
					break;
				case "administrator":
					userRoleType = UserRoleType.Admin;
					break;
				default:
					Console.WriteLine("ChatMicroservice:: user role invalid value: " + userRole);
					return false;
			}

			return true;
		}*/
	}
}
