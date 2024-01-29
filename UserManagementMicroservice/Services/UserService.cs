using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UserManagementMicroservice.DataTransferObject;
using UserManagementMicroservice.Model;
using UserManagementMicroservice.Repositories;
using UserManagementMicroservice.Utilities;

namespace UserManagementMicroservice.Services
{
	public class UserService
	{
		private readonly UserRepository _userRepository;
		private readonly JwtTokenService _jwtTokenService;

		public UserService(UserRepository userRepository, JwtTokenService jwtTokenService)
		{
			_userRepository = userRepository;
			_jwtTokenService = jwtTokenService;
		}


		public UserLoginSuccess Login(UserLogin user)
		{
			User currentUser = _userRepository.UserLogin(user);

			int id = currentUser.Id;
			if (id == -1)
				return new UserLoginSuccess("UserNotFound", string.Empty);

			var token = _jwtTokenService.GenerateJwtToken(id, currentUser.Role);

			return new UserLoginSuccess("Success", token);
		}

		public string Register(UserRegister user)
		{
			user.Role = "client";
			bool success = _userRepository.CreateUser(user);

			return success == true ? "success" : "fail";
		}

		public string RegisterByAdmin(string token, UserRegister user)
		{
			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}

			int adminId = -1;
			if (!int.TryParse(tokenContent.UserId, out adminId))
			{
				Console.WriteLine("parsarea stringului la intreg a esuat: " + tokenContent.UserId);
				return "fail";
			}

			if (!Verify(adminId, tokenContent.ExpirationDateTime))
				return "fail";

			bool success = _userRepository.CreateUser(user);

			return success == true ? "success" : "fail";
		}

		public IEnumerable<User> GetUsers(string token)
		{
			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return null;
			}

			int adminId = -1;
			if (!int.TryParse(tokenContent.UserId, out adminId))
			{
				Console.WriteLine("parsarea stringului la intreg a esuat: " + tokenContent.UserId);
				return null;
			}

			if (!Verify(adminId, tokenContent.ExpirationDateTime))
				return null;

			var users = _userRepository.GetAll();

			return users;
		}

		public string UpdateCredentials(string token, UserUpdateCredentials credentials)
		{
			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}
		
			if (!Verify(credentials.AdminId, tokenContent.ExpirationDateTime))
				return "fail";


			bool success = _userRepository.UpdateUser(credentials);

			return success == true ? "success" : "fail";
		}

		public string Delete(string token, string id)
		{
			int userId = -1;
			if (!int.TryParse(id, out userId))
			{
				Console.WriteLine("parsarea stringului userId la intreg a esuat: " + id);
				return "fail";
			}

			var tokenContent = _jwtTokenService.ValidateToken(token);

			if(tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}
			
			int adminId = -1;
			if(!int.TryParse(tokenContent.UserId, out adminId))
			{
				Console.WriteLine("parsarea stringului la intreg a esuat: " +  tokenContent.UserId);
				return "fail";
			}

			if (!Verify(adminId, tokenContent.ExpirationDateTime))
				return "fail";

			var unMapDevicesSuccess = UnMapUserDevices(token,userId.ToString());
			unMapDevicesSuccess.Wait();
			if(unMapDevicesSuccess.Result == false)
			{
				Console.WriteLine($"UnMaparea deviceurilor pentru userul cu id-ul {userId} a esuat");
				return "fail";
			}

			bool success = _userRepository.DeleteUser(userId);

			return success == true ? "success" : "fail";
		}

		public string VerifyUserExistency(string token, string id)
		{
			int userId = -1;
			if (!int.TryParse(id, out userId))
			{
				Console.WriteLine("parsarea stringului userId la intreg a esuat: " + id);
				return "fail";
			}

			var tokenContent = _jwtTokenService.ValidateToken(token);

			if (tokenContent == null)
			{
				Console.WriteLine("Token content este null");
				return "fail";
			}

			int adminId = -1;
			if (!int.TryParse(tokenContent.UserId, out adminId))
			{
				Console.WriteLine("parsarea stringului la intreg a esuat: " + tokenContent.UserId);
				return "fail";
			}

			if (!Verify(adminId, tokenContent.ExpirationDateTime))
				return "fail";

			var user = _userRepository.FindById(userId);

			return user.Id == -1 ? "fail" : "success";
		}

		private bool Verify(int adminId, DateTime tokenExpirationTime)
		{
			var currentTime = DateTime.UtcNow;

			if (currentTime > tokenExpirationTime)
			{
				Console.WriteLine("acest token a expirat");
				return false;
			}

			User admin = _userRepository.FindById(adminId);
			if (!admin.Role.Equals("administrator"))
			{
				Console.WriteLine("userul nu are admin " + admin.Role);
				return false;
			}

			return true;
		}

		private async Task<bool> UnMapUserDevices(string token, string userId)
		{
			string apiUrl = "https://localhost:7158/api/Device/unMapUserDevices";

			var httpClient = new HttpClient();

			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var data = new
			{
				DeviceId = userId
			};

			var json = JsonSerializer.Serialize(data);

			var content = new StringContent(
				json,
				Encoding.UTF8,
				"application/json"
			);

			HttpResponseMessage response = await httpClient.PutAsync(apiUrl,content);

			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();
				if (responseBody.Equals("success"))
					return true;
			}
			else
			{
				Console.WriteLine("Cererea a esuat cu codul " + response.StatusCode);
				return false;
			}

			return false;
		}
	}
}
