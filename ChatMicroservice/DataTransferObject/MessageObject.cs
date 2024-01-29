namespace ChatMicroservice.DataTransferObject
{
	public class MessageObject
	{
		public string Type { get; set; } = string.Empty;
		public string From { get; set; } = string.Empty;
		public int To { get; set; } = -1;
		public string Message { get; set; } = string.Empty;

		public override string ToString()
		{
			return $"Message type:{Type} from:{From} to:{To} message:{Message}";
		}
	}
}
