using MonitoringAndCommunicationMicroservice.DataTransferObject;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace MonitoringAndCommunicationMicroservice.Utilities
{
	public class WebSocketConnectionManager
	{
		private readonly ConcurrentDictionary<int, WebSocketPipeline> _connections = new();

		public bool AddConnection(int userId, WebSocketPipeline socket)
		{

			return _connections.TryAdd(userId, socket);
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
				await socket.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
				socket.TaskCompletionSource.TrySetResult("");
			}
		}
	}
}
