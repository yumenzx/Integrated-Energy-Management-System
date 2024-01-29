namespace ChatMicroservice.DataTransferObject
{
	public class TokenContent
	{
		public string UserId { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public DateTime ExpirationDateTime { get; set; }
	}
}
