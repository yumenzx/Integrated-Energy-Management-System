using System.Net.WebSockets;

namespace ChatMicroservice.DataTransferObject
{
	public class WebSocketPipeline
	{
		public WebSocket WebSocket { get; set; }
		public TaskCompletionSource<object> TaskCompletionSource { get; set; }
	}
}
