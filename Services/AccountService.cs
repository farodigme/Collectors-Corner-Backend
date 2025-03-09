using Collectors_Corner_Backend.Models.DataBase;
using Collectors_Corner_Backend.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class AccountService
	{
		private readonly TokenService _tokenService;
		private ApplicationContext _context;
		private PasswordHasher<string> _passwordHasher;
		public AccountService(ApplicationContext context, TokenService jwtService)
		{
			_context = context;
			_tokenService = jwtService;
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

			var user = await _context.Users.AsNoTracking().FirstAsync(u => u.Username == model.Username);
			if (user == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "No such user"
				};
			}

			var isPasswordEqual = _passwordHasher.VerifyHashedPassword(user.Username, user.Password, model.Password);
			
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
				AccessToken = _tokenService.GenerateJwtToken(user.Username, user.Email, out DateTime expires),
				RefreshToken = _tokenService.GenerateRefreshToken(),
				AccessTokenExpires = expires
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

			if (await _context.Users.AsNoTracking().AnyAsync(u => u.Username == model.Username))
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Username alredy taken"
				};
			}

			if (await _context.Users.AsNoTracking().AnyAsync(u => u.Email == model.Email))
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
				Password = _passwordHasher.HashPassword(model.Username, model.Password),
				AccessToken = _tokenService.GenerateJwtToken(model.Username, model.Email, out DateTime expires),
				RefreshToken = _tokenService.GenerateRefreshToken(),
				AccessTokenExpires = expires
			};
			
			await _context.Users.AddAsync(newUser);
			await _context.SaveChangesAsync();

			return new AuthResponse()
			{
				Success = true,
				AccessToken = newUser.AccessToken,
				RefreshToken = newUser.RefreshToken,
				AccessTokenExpires = newUser.AccessTokenExpires
			};
		}
	}
}
