using MonitoringAndCommunicationMicroservice.DataTransferObject;
using MonitoringAndCommunicationMicroservice.Utilities;
using System.Net.WebSockets;
using System.Text;

namespace MonitoringAndCommunicationMicroservice.Handlers
{
	public class WebSocketHandler
	{
		private WebSocketConnectionManager WebSocketConnectionManager { get; set; }


		public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
		{
			WebSocketConnectionManager = webSocketConnectionManager;
		}


		public void OnConnected(WebSocketPipeline socket, int userId)
		{
			bool success = WebSocketConnectionManager.AddConnection(userId, socket);
			
			if (!success)
			{
				Console.WriteLine("Adaugarea socketului a esuat");
			}
			//ListenForMessages(userId);
		}

		public void Disconect(int userId)
		{
			WebSocketConnectionManager.RemoveConnectionAsync(userId).Wait();
		}

		public async Task SendMessageAsync(int userId, string message)
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

			var buffer = Encoding.UTF8.GetBytes(message);
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
					Console.WriteLine("este open acumaa");
					var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
					Console.WriteLine("s-a primit cevaa" + result);
					if (result.MessageType == WebSocketMessageType.Text)
					{
						var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
						Console.WriteLine($"Mesaj primit: {message}");
						// Aici poți să faci ceva cu mesajul primit
					}
					else if (result.MessageType == WebSocketMessageType.Close)
					{
						break;
					}
				}
			}catch(Exception ex)
			{
				Console.WriteLine("Exceptia la listening: " + ex.Message);
			}
			Console.WriteLine("s-a inchis conexiunea aparent???");
		}


		/*public void OnDisconnected(WebSocket socket)
		{
			// Eliminați socket-ul din managerul de conexiuni WebSocket
			var user = WebSocketConnectionManager.GetSocketUser(socket);
			WebSocketConnectionManager.RemoveSocket(socket);

			// Aici puteți implementa orice logică specifică când un client se deconectează.
		}

		public async Task SendMessageAsync(WebSocket socket, string message)
		{
			if (socket.State != WebSocketState.Open)
				return;

			var buffer = Encoding.UTF8.GetBytes(message);
			var segment = new ArraySegment<byte>(buffer);

			await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
		}

		public async Task SendMessageAsync(ClaimsPrincipal user, string message)
		{
			var socket = WebSocketConnectionManager.GetSocketByUser(user);
			await SendMessageAsync(socket, message);
		}*/

		//public Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
	}
}
