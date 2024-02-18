using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using PairExpensesAPI.Data;
using PairExpensesAPI.Entities;
using AutoMapper;

namespace PairXpensesAPI.Services
{
	public class UserService : IUserService
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public UserService(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public void CreateUser(User user)
		{
			try
			{
				_context.Users.Add(user);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al crear el usuario: {ex.Message}");
				throw; // Optionally, handle or log the exception
			}
		}

		public void DeleteUser(User user)
		{
			try
			{
				_context.Users.Remove(user);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
				throw; // Optionally, handle or log the exception
			}
		}

		public List<User> GetAllUsers()
		{
			return _context.Users.ToList();
		}


		public User? UpdateUserById(User userToUpdate, UserReq updateUser)
		{
			try
			{
				_mapper.Map(updateUser, userToUpdate);
				_context.SaveChanges();
				return userToUpdate;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
				return null; // Handle or log the exception as needed
			}
		}
	}
}
