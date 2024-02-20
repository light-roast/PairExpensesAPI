using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PairExpensesAPI.Data;
using PairExpensesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Debt
namespace PairXpensesAPI.Services
{
	public class DebtService : IDebtService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public DebtService(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public void CreateDebt(Debt debt)
		{
			try
			{
				_context.Debts.Add(debt);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al crear la deuda: {ex.Message}");
			}
		}

		public void DeleteDebt(Debt debt)
		{
			_context.Debts.Remove(debt);
			_context.SaveChanges();
		}

		public List<Debt> GetAllDebtsByUserId(int userId)
		{
			return _context.Debts.Where(d => d.UserId == userId).ToList();
		}

		public Debt? GetDebtById(int id)
		{
			return _context.Debts.FirstOrDefault(d => d.Id == id);
		}

		public long GetTotalDebtValueByUserId(int userId)
		{
			return _context.Debts.Where(d => d.UserId == userId).Sum(d => d.Value);
		}

		public Debt? UpdateDebtById(Debt debtToUpdate, DebtReq updateDebt)
		{
			try
			{
				// Map properties from DebtReq to Debt
				_mapper.Map(updateDebt, debtToUpdate);

				// Update additional properties if needed
				debtToUpdate.UpdateDate = DateTime.Now;

				_context.Entry(debtToUpdate).State = EntityState.Modified;
				_context.SaveChanges();

				return debtToUpdate;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al actualizar la deuda: {ex.Message}");
				return null;
			}
		}

		public void DeleteAllDebts()
		{
			try
			{
				var Debts = _context.Debts.ToList();

				
				_context.Debts.RemoveRange(Debts);

				
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}
	
	}
}