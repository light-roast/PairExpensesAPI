namespace PairXpensesAPI.Services
{
	public interface IPaymentService
	{
		void CreatePayment(Payment payment);
		void DeletePayment(Payment payment);
		List<Payment> GetAllPaymentsByUserId(int userId);
		Payment? GetPaymentById(int id);

		Payment? UpdatePaymentById(Payment paymentToUpdate, Payment updatePayment);
		long GetTotalPaymentValueByUserId(int userId);
	}
}
