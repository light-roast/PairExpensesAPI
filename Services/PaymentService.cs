using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PairXpensesAPI.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly List<Payment> Payments = new List<Payment>
		{
			new Payment
			{
				Id = 1,
				Name = "Servicio de agua",
				Value = 110000,
				CreateDate = DateTime.Now,
				UpdateDate = DateTime.Now,
				User = new User { Id = 1, Name = "Daniel" }
			},
			new Payment
			{
				Id = 2,
				Name = "Pizza",
				Value = 600000,
				CreateDate = DateTime.Now,
				UpdateDate = DateTime.Now,
				User = new User { Id = 2, Name = "Maritza" }
			}
		};

		public void CreatePayment(Payment payment)
		{
			try
			{
				Payments.Add(payment);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al crear el pago: {ex.Message}");	
			}
		}

		public void DeletePayment(Payment payment)
		{
			try
			{
				Payments.Remove(payment);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al eliminar el pago: {ex.Message}");
			
			}
		}

		public List<Payment> GetAllPaymentsByUserId(int userId)
		{
			List<Payment> paymentsByUser = Payments.Where(p => p.User.Id == userId).ToList();
			return paymentsByUser;
		}

		public Payment? GetPaymentById(int id)
		{
			var payment = Payments.FirstOrDefault(p => p.Id == id);
			return payment;
		}

		public long GetTotalPaymentValueByUserId(int userId)
		{
			long totalPaymentValue = Payments.Where(p => p.User.Id == userId).Sum(p => p.Value);
			return totalPaymentValue;
		}

		public Payment? UpdatePaymentById(Payment paymentToUpdate, Payment updatePayment)
		{
			

				paymentToUpdate.Name = updatePayment.Name;
				paymentToUpdate.Value = updatePayment.Value;
				paymentToUpdate.UpdateDate = DateTime.Now;
				return paymentToUpdate;
		}
	}
}
