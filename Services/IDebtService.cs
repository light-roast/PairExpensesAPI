using PairExpensesAPI.Entities;

namespace PairXpensesAPI.Services
{
	public interface IDebtService
	{
		void CreateDebt(DebtReq debt);
		void DeleteDebt(Debt debt);
		List<Debt> GetAllDebtsByUserId(int userId);

		Debt? GetDebtById(int id);

		Debt? UpdateDebtById(Debt debtToUpdate, DebtReq updateDebt);
		long GetTotalDebtValueByUserId(int userId);

		void DeleteAllDebts();
	}
}

