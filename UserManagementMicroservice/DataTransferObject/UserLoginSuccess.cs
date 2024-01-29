namespace UserManagementMicroservice.DataTransferObject
{
	public class UserLoginSuccess
	{
		public string Response { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;

		public UserLoginSuccess() { }

		public UserLoginSuccess(string response, string token) { 
			Response = response;
			Token = token;
		}
	}
}
