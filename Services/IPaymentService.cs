using PairExpensesAPI.Entities;

namespace PairXpensesAPI.Services
{
	public interface IPaymentService
	{
		void CreatePayment(PaymentReq payment);
		void DeletePayment(Payment payment);
		List<Payment> GetAllPaymentsByUserId(int userId);
		Payment? GetPaymentById(int id);

		Payment? UpdatePaymentById(Payment paymentToUpdate, PaymentReq updatePayment);
		long GetTotalPaymentValueByUserId(int userId);
		void DeleteAllPayments();
	}
}
