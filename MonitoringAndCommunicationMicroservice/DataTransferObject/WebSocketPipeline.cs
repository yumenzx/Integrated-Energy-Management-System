using System.Net.WebSockets;

namespace MonitoringAndCommunicationMicroservice.DataTransferObject
{
	public class WebSocketPipeline
	{
		public WebSocket WebSocket { get; set; }
		public TaskCompletionSource<object> TaskCompletionSource { get; set; }
	}
}
