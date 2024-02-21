using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PairExpensesAPI.Data;
using PairExpensesAPI.Entities; // Import EF Core namespace

namespace PairXpensesAPI.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public PaymentService(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public void CreatePayment(PaymentReq paymentReq)
		{
			var payment = _mapper.Map<Payment>(paymentReq);
			try
			{
				_context.Payments.Add(payment);
				_context.SaveChanges();
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
				_context.Payments.Remove(payment);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al eliminar el pago: {ex.Message}");
			}
		}

		public List<Payment> GetAllPaymentsByUserId(int userId)
		{
			return _context.Payments.Where(p => p.UserId == userId).ToList();
		}

		public Payment? GetPaymentById(int id)
		{
			return _context.Payments.FirstOrDefault(p => p.Id == id);
		}

		public long GetTotalPaymentValueByUserId(int userId)
		{
			return _context.Payments.Where(p => p.UserId == userId).Sum(p => p.Value);
		}

		public Payment? UpdatePaymentById(Payment paymentToUpdate, PaymentReq updatePayment)
		{
			try
			{
				// Map properties from PaymentReq to Payment
				_mapper.Map(updatePayment, paymentToUpdate);

				// Update additional properties if needed
				paymentToUpdate.UpdateDate = DateTime.Now;

				_context.Entry(paymentToUpdate).State = EntityState.Modified;
				_context.SaveChanges();

				return paymentToUpdate;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al actualizar el pago: {ex.Message}");
				return null;
			}
		}

		public void DeleteAllPayments()
		{
			try
			{
				var payments = _context.Payments.ToList();

				
				_context.Payments.RemoveRange(payments);

				
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
	}
}
