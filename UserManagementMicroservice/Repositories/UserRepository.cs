using System.Reflection.Metadata.Ecma335;
using UserManagementMicroservice.DataAccess;
using UserManagementMicroservice.DataTransferObject;
using UserManagementMicroservice.Model;

namespace UserManagementMicroservice.Repositories
{
	public class UserRepository
	{
		private readonly UserDbContext _context;

		public UserRepository(UserDbContext context) 
		{
			_context = context;
		}

		public IEnumerable<User> GetAll()
		{
			return _context.Users.ToList();
		}

		public User FindById(int id)
		{
			var user = _context.findUser(id);

			if (user == null)
			{
				user = new User();
				user.Id = -1;
			}

			return user;
		}

		public User UserLogin(UserLogin user)
		{

			User? u = _context.findUser(user.Username, user.Password);
		
			if (u == null)
			{
				var userNotFound = new User();
				userNotFound.Id = -1;
				return userNotFound;
			}

			return u;
		}

		public bool CreateUser(UserRegister user)
		{
			User newUser = new User();
			newUser.Username = user.Username;
			newUser.Password = user.Password;
			newUser.Role = user.Role;

			return _context.InsertUser(newUser);
		}

		public bool UpdateUser(UserUpdateCredentials credentials)
		{
			return _context.UpdateUser(credentials);
		}

		public bool DeleteUser(int id)
		{
			return _context.DeleteUser(id);
		}
	}
}
