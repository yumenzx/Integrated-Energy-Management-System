using Microsoft.AspNetCore.Mvc;
using MonitoringAndCommunicationMicroservice.DataTransferObject;
using MonitoringAndCommunicationMicroservice.Handlers;
using MonitoringAndCommunicationMicroservice.Services;
using System.Net.WebSockets;

namespace MonitoringAndCommunicationMicroservice.Controllers
{
	public class WebSocketController : ControllerBase
	{
		private WebSocketHandler _webSocketHandler {  get; set; }
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
		
				if (values == null) {
					Console.WriteLine("Validarea a esuat");
					return;
				}
				if (!values.Role.Equals("client"))
				{
					Console.WriteLine("rolul userului nu este client; este: " + values.Role);
					return;
				}
				var userId = -1;
				bool success = int.TryParse(values.UserId, out userId);
				if (!success)
				{
					Console.WriteLine("parsarea a esuat");
					return;
				}
	
				_webSocketHandler.OnConnected(webSocketPipeline, userId);
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
			if (!values.Role.Equals("client"))
				return Ok("");
			var userId = -1;
			bool success = int.TryParse(values.UserId, out userId);
			if (!success)
				return Ok("");

			_webSocketHandler.Disconect(userId);
			Console.WriteLine("MonitoringService:: userul cu id-ul: " + userId + " s-a deconectat");
			return Ok("Conexiunea websocket stearsa");
		}
	}
}
