using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementMicroservice.DataTransferObject;
using UserManagementMicroservice.Services;

namespace UserManagementMicroservice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserAuthenticationController : ControllerBase
	{
		private readonly UserService _userService;

		public UserAuthenticationController(UserService userSevice) {
			_userService = userSevice;
		}

		[HttpPost]
		[Route("login")]
		public IActionResult Login([FromBody] UserLogin user)
		{
			var response = _userService.Login(user);

			return Ok(response);
		}


		[HttpPost]
		[Route("register")]
		public IActionResult Register([FromBody] UserRegister user)
		{
			var response = _userService.Register(user);

			return Ok(response);
		}

		[Authorize]
		[HttpPost]
		[Route("registerByAdmin")]
		public IActionResult RegisterByAdmin([FromBody] UserRegister user)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");


			var response = _userService.RegisterByAdmin(token, user);

			return Ok(response);
		}

		[Authorize]
		[HttpGet]
		[Route("getAllUsers")]
		public IActionResult GetAllUsers()
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var u = _userService.GetUsers(token);

			return Ok(new { response = "success", users = u });
		}

		[Authorize]
		[HttpPut]
		[Route("updateCredentials")]
		public IActionResult UpdateCredentials([FromBody] UserUpdateCredentials credentials)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");


			var response = _userService.UpdateCredentials(token,credentials);

			return Ok(response);
		}


		[Authorize]
		[HttpDelete]
		[Route("deleteUser/{id}")]
		public IActionResult Delete(string id)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var response = _userService.Delete(token, id);
			return Ok(response);
		}


		[Authorize]
		[HttpGet]
		[Route("verifyUserExistency/{id}")]
		public IActionResult VerifyUserExistency(string id)
		{
			var token = GetTokenFromRequestHeader();

			if (token == null)
				return Ok("Tokenul este null");

			var response = _userService.VerifyUserExistency(token, id);

			return Ok(response);
		}

		private string? GetTokenFromRequestHeader()
		{
			string? token = default(string); //null
			try
			{
				token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Eroare la extragerea tokenului: " + ex.Message);
			}

			return token;
		}
	}
}
