using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PairXpensesAPI.Services
{
	public class UserService : IUserService
	{
		public List<User> Users = new List<User>
		{
			new User {
				Id = 1,
				Name = "Daniel",
				Debts = new List<Debt> {},
				Payments =  new List<Payment> {}
			},
			new User {
				Id = 2,
				Name = "Maritza",
				Debts = new List<Debt> {},
				Payments =  new List<Payment> {}
			}
		};

		public void CreateUser(User user)
		{
			try
			{
				Users.Add(user);

				
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al crear el usuario: {ex.Message}");

			}
		}

		public void DeleteUser(User user)
		{
			try
			{
				Users.Remove(user);

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
			}
		}

		public List<User> GetAllUsers()
		{
			return Users;
		}


		public User? UpdateUserById(User userToUpdate, User updateUser)
		{
				userToUpdate.Name = updateUser.Name;
	
				return userToUpdate;
		}
	}
}
