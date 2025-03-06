using Collectors_Corner_Backend.Models;
using Collectors_Corner_Backend.Models.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class UserService
	{
		private readonly JwtService _jwtService;
		private ApplicationContext _context;
		private PasswordHasher<string> _passwordHasher;
		public UserService(ApplicationContext context, JwtService jwtService) 
		{
			_context = context;
			_jwtService = jwtService;
			_passwordHasher = new PasswordHasher<string>();
		}

		public async Task<bool> Login([FromBody] LoginModel model)
		{
			if (model == null) return false;

			var loginUser = new User()
			{
				Email = model.Email,
				Password = model.Password
			};

			var user = await _context.Users.FirstAsync(u => u.Email == model.Email);
			if (user == null) return false;

			var isPasswordEqual = _passwordHasher.VerifyHashedPassword(user.Email, user.Password, model.Password);

			if (isPasswordEqual == PasswordVerificationResult.Success)
			{
				return true;
			}

			return false;
		}
	}
}
