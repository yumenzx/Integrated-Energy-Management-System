namespace UserManagementMicroservice.DataTransferObject
{
	public class UserRegister
	{
		public string Username { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string Role { get; set; } = string.Empty;
	}
}
