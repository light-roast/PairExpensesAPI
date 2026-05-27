using System.Linq;
using PairExpensesAPI.Entities;

namespace PairXpensesAPI.Services
{
	public class ReportService : IReportService
	{
		private readonly IUserService _userService;
		private readonly IPaymentService _paymentService;
		private readonly IDebtService _debtService;

		public ReportService(IUserService userService, IPaymentService paymentService, IDebtService debtService)
		{
			_userService = userService;
			_paymentService = paymentService;
			_debtService = debtService;
		}

		public ReportRes? GenerateReport(string pairRole, int percentageA)
		{
			var users = _userService.GetAllUsers(pairRole).OrderBy(u => u.Id).ToList();
			if (users.Count < 2) return null;

			var a = users[0];
			var b = users[1];

			long totalPaymentA = _paymentService.GetTotalPaymentValueByUserId(a.Id);
			long totalPaymentB = _paymentService.GetTotalPaymentValueByUserId(b.Id);
			long totalDebtA = _debtService.GetTotalDebtValueByUserId(a.Id);
			long totalDebtB = _debtService.GetTotalDebtValueByUserId(b.Id);

			long totalExpense = totalPaymentA + totalPaymentB;
			long expenseForA = totalExpense * percentageA / 100;
			long expenseForB = totalExpense - expenseForA;

			var res = new ReportRes
			{
				HasData = !(totalPaymentA == 0 && totalPaymentB == 0 && totalDebtA == 0 && totalDebtB == 0),
				PercentageA = percentageA,
				TotalExpense = totalExpense,
				UserA = new ReportUserBreakdown
				{
					Id = a.Id,
					Name = a.Name,
					TotalPayment = totalPaymentA,
					TotalDebt = totalDebtA,
					Share = expenseForA
				},
				UserB = new ReportUserBreakdown
				{
					Id = b.Id,
					Name = b.Name,
					TotalPayment = totalPaymentB,
					TotalDebt = totalDebtB,
					Share = expenseForB
				},
				Settlement = null
			};

			if (!res.HasData) return res;

			long debtFromAtoB = 0;
			long debtFromBtoA = 0;

			if (totalPaymentA > expenseForA)
				debtFromBtoA = totalPaymentA - expenseForA;
			else if (totalPaymentB > expenseForB)
				debtFromAtoB = totalPaymentB - expenseForB;

			if (debtFromBtoA > 0)
			{
				long netDebt = (debtFromBtoA + totalDebtB) - totalDebtA;
				if (netDebt > 0)
					res.Settlement = new ReportSettlement { FromUserId = b.Id, ToUserId = a.Id, Amount = netDebt };
				else if (netDebt < 0)
					res.Settlement = new ReportSettlement { FromUserId = a.Id, ToUserId = b.Id, Amount = -netDebt };
			}
			else if (debtFromAtoB > 0)
			{
				long netDebt = (debtFromAtoB + totalDebtA) - totalDebtB;
				if (netDebt > 0)
					res.Settlement = new ReportSettlement { FromUserId = a.Id, ToUserId = b.Id, Amount = netDebt };
				else if (netDebt < 0)
					res.Settlement = new ReportSettlement { FromUserId = b.Id, ToUserId = a.Id, Amount = -netDebt };
			}
			else
			{
				if (totalDebtA > totalDebtB)
					res.Settlement = new ReportSettlement { FromUserId = a.Id, ToUserId = b.Id, Amount = totalDebtA - totalDebtB };
				else if (totalDebtB > totalDebtA)
					res.Settlement = new ReportSettlement { FromUserId = b.Id, ToUserId = a.Id, Amount = totalDebtB - totalDebtA };
			}

			return res;
		}
	}
}
