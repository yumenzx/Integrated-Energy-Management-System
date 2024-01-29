using ChatMicroservice.DataTransferObject;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatMicroservice.Services
{
	public class TokenValidationService
	{
		private IConfiguration _config;

		public TokenValidationService(IConfiguration config)
		{
			_config = config;
		}

		public TokenContent? ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

			ClaimsPrincipal? claimsPrincipal = null;

			try
			{
				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = _config["Jwt:Issuer"],
					ValidAudience = _config["Jwt:Issuer"],
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};

				SecurityToken validatedToken;
				claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Validarea tokenului a esuat: " + ex.ToString());
				return null;
			}

			if (claimsPrincipal == null)
			{
				Console.WriteLine("ClaimsPrincipal este null");
				return null;
			}
			return GetClaimValues(claimsPrincipal);
		}

		private TokenContent GetClaimValues(ClaimsPrincipal claimsPrincipal)
		{
			TokenContent tokenContent = new TokenContent();


			var userId = claimsPrincipal.FindFirst("userId");
			if (userId != null)
				tokenContent.UserId = userId.Value;
			else
				Console.WriteLine("Claimul userId a esuat");

			var userRole = claimsPrincipal.FindFirst("userRole");
			if (userRole != null)
				tokenContent.Role = userRole.Value;
			else
				Console.WriteLine("Claimul userRole a esuat");

			var expirationDateTime = claimsPrincipal.FindFirst("exp");
			if (expirationDateTime != null)
				tokenContent.ExpirationDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationDateTime.Value)).DateTime;
			else
				Console.WriteLine("Claimul cu data expirarii a esuat");

			return tokenContent;
		}


		public bool ParseTokenValues(TokenContent? values, out int userId, out UserRoleType userRoleType)
		{
			userId = -1; // pt a putea folosi out
			userRoleType = UserRoleType.None; // altfel as putea folosi si ref

			if (values == null)
			{
				Console.WriteLine("Validarea a esuat");
				return false;
			}

			bool success = int.TryParse(values.UserId, out userId);
			if (!success)
			{
				Console.WriteLine("parsarea a esuat");
				return false;
			}

			var userRole = values.Role;
			switch (userRole)
			{
				case "client":
					userRoleType = UserRoleType.Client;
					break;
				case "administrator":
					userRoleType = UserRoleType.Admin;
					break;
				default:
					Console.WriteLine("ChatMicroservice:: user role invalid value: " + userRole);
					return false;
			}

			return true;
		}
	}
}
