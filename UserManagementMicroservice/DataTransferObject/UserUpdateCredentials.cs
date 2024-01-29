namespace UserManagementMicroservice.DataTransferObject
{
	public class UserUpdateCredentials
	{
		public int AdminId { get; set; }

		public int UserId { get; set; }
		public string NewUsername { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
		public string NewRole { get; set; } = string.Empty;
	}
}