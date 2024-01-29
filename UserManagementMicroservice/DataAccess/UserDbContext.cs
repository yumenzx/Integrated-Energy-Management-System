using Microsoft.EntityFrameworkCore;
using UserManagementMicroservice.DataTransferObject;
using UserManagementMicroservice.Model;

namespace UserManagementMicroservice.DataAccess
{
	public class UserDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

		/*
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseMySql("ds",null);
		}
		*/

		public User? findUser(int id)
		{
			var user = Users.FirstOrDefault(u => u.Id == id);
			return user;
		}

		public User? findUser(string username,string password)
		{
			var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);
			return user;
		}

		public bool InsertUser(User user)
		{
			var u = Users.FirstOrDefault(u => u.Username == user.Username);

			if (u != null)
				return false;

			Users.Add(user);
			SaveChanges();

			return true;
		}

		public bool UpdateUser(UserUpdateCredentials newCredentials)
		{
			var u = Users.FirstOrDefault(u => u.Id == newCredentials.UserId);

			if (u == null)
				return false;

			u.Username = newCredentials.NewUsername;
			u.Password = newCredentials.NewPassword;
			u.Role = newCredentials.NewRole;
			SaveChanges();

			return true;
		}

		public bool DeleteUser(int userId)
		{
			var u = Users.FirstOrDefault(u=> u.Id == userId);

			if (u == null)
				return false;

			Users.Remove(u);
			SaveChanges();

			return true;
		}
	}
}
