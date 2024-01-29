using ChatMicroservice.Data;
using ChatMicroservice.DataTransferObject;
using ChatMicroservice.Handlers;
using ChatMicroservice.Model;
using ChatMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ChatMicroservice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessagesController : ControllerBase
	{
		private MessagesContext _messagesContext;
		private readonly TokenValidationService _tokenValidationService;
		private readonly WebSocketHandler _webSocketHandler;

		public MessagesController(MessagesContext messagesContext, TokenValidationService tokenValidationService, WebSocketHandler webSocketHandler)
		{
			_messagesContext = messagesContext;
			_tokenValidationService = tokenValidationService;
			_webSocketHandler = webSocketHandler;
		}

		[Authorize]
		[HttpGet]
		[Route("getMessages/{clientId}")]
		public IActionResult GetMessages(string clientId)
		{
			var token = GetTokenFromRequestHeader();
			if (token == null)
			{
				Console.WriteLine("ChatMicroservice:: Tokenul este null");
				return Ok(new { response = "TokenIsNull", messagess = 0 });
			}

			var values = _tokenValidationService.ValidateToken(token);

			bool success = _tokenValidationService.ParseTokenValues(values, out int userId, out UserRoleType userRole);
			if (!success)
			{
				Console.WriteLine("ChatMicroservice:: Validarea tokenului a esuat");
				return Ok(new { response = "ValidationFailed", messagess = 0 });
			}

			List<SingleMessage> messages;

			int c = -1;
			if(userRole == UserRoleType.Admin) 
			{
				if(!int.TryParse(clientId, out c))
				{
					Console.WriteLine($"ChatMicroservice:: Parsarea a esuat:{clientId}");
					return Ok(new { response = "Parse failed", messagess = 0 });
				}

				messages = _messagesContext.GetMessages(userId, c);
				return Ok(new { response = "success", messagess = messages });
			}

			messages = _messagesContext.GetMessages(userId);
			return Ok(new { response = "success", messagess = messages });
		}

		[Authorize]
		[HttpGet]
		[Route("notifyMessageReaded/{clientId}")]
		public IActionResult NotifyMessageReaded(string clientId)
		{
			var token = GetTokenFromRequestHeader();
			if (token == null)
			{
				Console.WriteLine("ChatMicroservice:: Tokenul este null");
				return Ok(new { response = "TokenIsNull", messagess = 0 });
			}

			var values = _tokenValidationService.ValidateToken(token);

			bool success = _tokenValidationService.ParseTokenValues(values, out int userId, out UserRoleType userRole);
			if (!success)
			{
				Console.WriteLine("ChatMicroservice:: Validarea tokenului a esuat");
				return Ok(new { response = "ValidationFailed", messagess = 0 });
			}

			var message = new MessageObject();
			message.Type = "NOTIFY_MESSAGE_READED";
			if (userRole == UserRoleType.Admin)
			{
				int c = -1;
				if (!int.TryParse(clientId, out c))
				{
					Console.WriteLine($"ChatMicroservice:: Parsarea a esuat:{clientId}");
					return Ok(new { response = "Parse failed"});
				}

				_webSocketHandler.SendMessageAsync(c, message).Wait();
				
				return Ok(new { response = "success"});
			}

			// is a client, notify the admin
			var adminId = _messagesContext.GetMessages(userId)
							.Where(m => m.From == userId)
							.First()
							.To;

			message.Message = $"Clientul cu id: {userId} v-a citit mesajul";
			_webSocketHandler.SendMessageAsync(adminId, message).Wait();

			return Ok(new { response = "success"});
		}

		[Authorize]
		[HttpGet]
		[Route("notifyTyping/{clientId}")]
		public IActionResult NotifyTyping(string clientId)
		{
			var token = GetTokenFromRequestHeader();
			if (token == null)
			{
				Console.WriteLine("ChatMicroservice:: Tokenul este null");
				return Ok(new { response = "TokenIsNull", messagess = 0 });
			}

			var values = _tokenValidationService.ValidateToken(token);

			bool success = _tokenValidationService.ParseTokenValues(values, out int userId, out UserRoleType userRole);
			if (!success)
			{
				Console.WriteLine("ChatMicroservice:: Validarea tokenului a esuat");
				return Ok(new { response = "ValidationFailed", messagess = 0 });
			}

			var message = new MessageObject();
			message.Type = "NOTIFY_TYPING";
			if (userRole == UserRoleType.Admin)
			{
				int c = -1;
				if (!int.TryParse(clientId, out c))
				{
					Console.WriteLine($"ChatMicroservice:: Parsarea a esuat:{clientId}");
					return Ok(new { response = "Parse failed" });
				}

				_webSocketHandler.SendMessageAsync(c, message).Wait();

				return Ok(new { response = "success" });
			}

			// is a client, notify the admin
			message.From = userId.ToString();
			var m = _messagesContext.GetMessages(userId);
            if (m.Count > 0)
            {
				var adminId = m
							.Where(m => m.From == userId)
							.First()
							.To;

				message.Message = $"Clientul cu id: {userId} scrie un mesaj";
				_webSocketHandler.SendMessageAsync(adminId, message).Wait();
			}
            

			return Ok(new { response = "success" });
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
	}
}
