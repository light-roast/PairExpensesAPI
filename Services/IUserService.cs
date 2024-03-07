using PairExpensesAPI.Entities;

namespace PairXpensesAPI.Services
{
	public interface IUserService
	{
		void CreateUser(User user);
		void DeleteUser(User user);
		List<User> GetAllUsers(string PairRole);

		User? UpdateUserById(User userToUpdate, UserReq updateUser);
		
	}
}

