using Collectors_Corner_Backend.Models.DataBase;
using Collectors_Corner_Backend.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

		public async Task<AuthResponse> Login([FromBody] LoginRequest model)
		{
			if (model == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Empty data"
				};
			}

			var loginUser = new User()
			{
				Username = model.Username,
				Password = model.Password
			};

			var user = await _context.Users.FirstAsync(u => u.Username == model.Username);
			if (user == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "No such user"
				};
			}

			var isPasswordEqual = _passwordHasher.VerifyHashedPassword(user.Email, user.Password, model.Password);

			if (isPasswordEqual != PasswordVerificationResult.Success)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Invalid username or password"
				};
			}

			return new AuthResponse()
			{
				Success = true,
				JWToken = _jwtService.GenerateJwtToken(user),
				RefreshToken = ""
			};
		}

		public async Task<AuthResponse> Register([FromBody] RegistrationRequest model)
		{
			if (model == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Empty data"
				};
			}

			if (await _context.Users.AnyAsync(u => u.Username == model.Username))
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Username alredy taken"
				};
			}

			if (await _context.Users.AnyAsync(u => u.Email == model.Email))
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Email alredy used"
				};
			}

			var newUser = new User()
			{
				Username = model.Username,
				Email = model.Email,
				Password = _passwordHasher.HashPassword(model.Email, model.Password),
			};

			await _context.Users.AddAsync(newUser);
			await _context.SaveChangesAsync();

			return new AuthResponse()
			{
				Success = true,
				JWToken = _jwtService.GenerateJwtToken(newUser),
				RefreshToken = ""
			};
		}
	}
}
