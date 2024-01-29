using ChatMicroservice.DataTransferObject;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace ChatMicroservice.Utilities
{
	public class WebSocketConnectionManager
	{
		private readonly ConcurrentDictionary<int, WebSocketPipeline> _connections = new();

		private readonly HashSet<int> _admins = new();


		public bool AddConnection(int userId, WebSocketPipeline socket, bool isAdmin)
		{
			bool success = _connections.TryAdd(userId, socket);
			if (!success)
				return false;

			if(isAdmin)
				_admins.Add(userId);

			return true;
		}

		public bool isAdmin(int userId)
		{
			return _admins.Contains(userId);
		}

		public bool IsConnected(int adminId) 
		{
			return _admins.Contains(adminId);
		}

		public int GetFirstConnectedAdminId()
		{
			if(_admins.Count == 0) 
				return -1;
			
			return _admins.First();
		}

		public WebSocket? GetConnection(int userId)
		{
			_connections.TryGetValue(userId, out var socket);
			return socket.WebSocket;
		}

		public async Task RemoveConnectionAsync(int userId)
		{
			if (_connections.TryRemove(userId, out var socket))
			{
				_admins.Remove(userId);
				await socket.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
				socket.TaskCompletionSource.TrySetResult("");
			}
		}
	}
}
