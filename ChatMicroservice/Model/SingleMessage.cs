namespace ChatMicroservice.Model
{
	public class SingleMessage
	{
		public int From { get; set; }
		public int To { get; set; }
		public string Message { get; set; } = string.Empty;
	}
}
