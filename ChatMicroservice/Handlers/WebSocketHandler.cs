using ChatMicroservice.Data;
using ChatMicroservice.DataTransferObject;
using ChatMicroservice.Model;
using ChatMicroservice.Services;
using ChatMicroservice.Utilities;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace ChatMicroservice.Handlers
{
	public class WebSocketHandler
	{
		private WebSocketConnectionManager WebSocketConnectionManager { get; set; }
		private readonly TokenValidationService _validationService;
		private MessagesContext _messagesContext;

		public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager, TokenValidationService validationService, MessagesContext messagesContext)
		{
			WebSocketConnectionManager = webSocketConnectionManager;
			_validationService = validationService;
			_messagesContext = messagesContext;
		}


		public void OnConnected(WebSocketPipeline socket, int userId, UserRoleType userRole)
		{
			var isAdmin = userRole == UserRoleType.Admin ? true : false;

			bool success = WebSocketConnectionManager.AddConnection(userId, socket, isAdmin);

			if (!success)
			{
				Console.WriteLine("Adaugarea socketului a esuat");
			}

			ListenForMessages(userId);
		}

		public void Disconect(int userId)
		{
			if (!WebSocketConnectionManager.isAdmin(userId)) // if is a client
			{
				var messages = _messagesContext.GetMessages(userId);

				if (messages.Count > 0)
				{
					var m = messages.FirstOrDefault(m => m.From == userId);
					int adminId = m.To;

					if (WebSocketConnectionManager.IsConnected(adminId))
					{
						var response = new MessageObject();
						response.Type = "CLIENT_DECONECTAT";
						response.From = "ChatMicroservice";
						response.To = userId;
						response.Message = $"Clientul cu id:{userId} s-a deconectat";
						SendMessageAsync(adminId, response).Wait();
					}
				}
				
			}

			WebSocketConnectionManager.RemoveConnectionAsync(userId);
			_messagesContext.DeleteMessages(userId);
		}

		public async Task SendMessageAsync(int userId, MessageObject message)
		{
			var socket = WebSocketConnectionManager.GetConnection(userId);

			if (socket == null)
			{
				Console.WriteLine("Socketul este null");
				return;
			}

			if (socket.State != WebSocketState.Open)
			{
				Console.WriteLine("Socketul nu este open aici");
				return;
			}

			var jsonMessage = JsonConvert.SerializeObject(message);
			var buffer = Encoding.UTF8.GetBytes(jsonMessage);
			var segment = new ArraySegment<byte>(buffer);

			try
			{
				if (socket.State == WebSocketState.Open)
				{
					await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
					Console.WriteLine("Mesaj trimis cu succes!");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exceptie la trimiterea mesajului: {ex.Message}");
			}
		}

		private async Task ListenForMessages(int userId)
		{
			try
			{
				var socket = WebSocketConnectionManager.GetConnection(userId);
				var buffer = new ArraySegment<byte>(new byte[4096]);

				while (socket.State == WebSocketState.Open)
				{
					var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

					if (result.MessageType == WebSocketMessageType.Text)
					{
						var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

						// Deserializare JSON in obiect
						var messageObject = JsonConvert.DeserializeObject<MessageObject>(message);

						if(messageObject == null)
						{
							Console.WriteLine("ChatMicroservice:: deserializarea a esuat:" + message);
							continue;
						}

						//Console.WriteLine(messageObject.ToString());

						ProcessMessage(userId, messageObject);

					}
					else if (result.MessageType == WebSocketMessageType.Close)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exceptia la listening: " + ex.Message);
			}
			Console.WriteLine($"ChatMicroservice:: Conexiunea userId:{userId} nu mai asculta pentru mesaje");
		}

		private void ProcessMessage(int userId, MessageObject msg)
		{
			var response = new MessageObject();

			var values = _validationService.ValidateToken(msg.From);

			bool success = _validationService.ParseTokenValues(values, out int userIdFrom, out UserRoleType userRole);
			if (!success)
			{
				response.Type = "MESAJ_TRIMIS_FAIL";
				response.From = "ChatMicroservice";
				response.To = userId;
				response.Message = "Eroare vezi in Microserviciul de Chat";
				SendMessageAsync(userId, response);
				return;
			}

			var singleMessage = new SingleMessage();
			response.To = userId;
			response.Message = msg.Message;
			switch (userRole)
			{
				case UserRoleType.Client:
					int firstAdminId = WebSocketConnectionManager.GetFirstConnectedAdminId();
					if(firstAdminId == -1) // nu este niciun admin disponibil/logat
					{
						response.Type = "MESAJ_TRIMIS_FAIL";
						response.Message = "In momentul de fata nu este niciun administrator disponibil";
						SendMessageAsync(userId, response).Wait();
						return;
					}

					// if first message, initiate the conversation
					var m = _messagesContext.GetMessages(userId);
					bool firstMessage = m.Count == 0 ? true : false;
					if (firstMessage)
						response.Type = "MESAJ_PRIMIT_SUCCESS_CONV_NOU";
					else
					{
						firstAdminId = m.First().To;
						response.Type = "MESAJ_PRIMIT_SUCCESS";
					}

					response.From = userId.ToString();
					response.To = firstAdminId;
					response.Message = msg.Message;
					SendMessageAsync(firstAdminId, response).Wait();

					singleMessage.From = userId;
					singleMessage.To = firstAdminId;
					singleMessage.Message = msg.Message;
					_messagesContext.InsertMessage(singleMessage);

					break;
				case UserRoleType.Admin:
					response.Type = "MESAJ_PRIMIT_SUCCESS";
					response.From = userId.ToString();
					response.To = msg.To;
					response.Message = msg.Message;
					SendMessageAsync(msg.To, response).Wait();

					singleMessage.From = userId;
					singleMessage.To = msg.To;
					singleMessage.Message = msg.Message;
					_messagesContext.InsertMessage(singleMessage);

					break;
				case UserRoleType.None:
				default:
					Console.WriteLine("ChatMicroservice:: UserRoleType is not valid");
					break;
			}

			// reply to the sender with a success message
			var responseSuccess = new MessageObject();
			responseSuccess.Type = "MESAJ_TRIMIS_SUCCESS";
			responseSuccess.From = "ChatMicroservice";
			responseSuccess.To = userId;
			responseSuccess.Message = msg.Message;
			SendMessageAsync(userId, responseSuccess);
		}
	}
}
