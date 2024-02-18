using PairExpensesAPI.Entities;

namespace PairXpensesAPI.Services
{
	public interface IUserService
	{
		void CreateUser(User user);
		void DeleteUser(User user);
		List<User> GetAllUsers();

		User? UpdateUserById(User userToUpdate, UserReq updateUser);
		
	}
}

