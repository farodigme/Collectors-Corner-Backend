using Collectors_Corner_Backend.Models.Entities;
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
					Error = "Null data"
				};
			}

			if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Empty data"
				};
			}

			var user = await _context.Users.Include(x => x.RefreshToken).FirstAsync(u => u.Username == model.Username);
			if (user == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "No such user"
				};
			}

			var isPasswordEqual = _passwordHasher.VerifyHashedPassword(user.Username, user.PasswordHash, model.Password);

			if (isPasswordEqual != PasswordVerificationResult.Success)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Invalid username or password"
				};
			}

			user.RefreshToken = _tokenService.GenerateRefreshToken();
			await _context.SaveChangesAsync();

			return new AuthResponse()
			{
				Success = true,
				AccessToken = _tokenService.GenerateJwtToken(user.Username, user.Email, out DateTime jwTokenExpires),
				AccessTokenExpires = jwTokenExpires,
				RefreshToken = user.RefreshToken.Token,
				RefreshTokenExpires = user.RefreshToken.ExpiresAt
			};
		}

		public async Task<AuthResponse> Register([FromBody] RegistrationRequest model)
		{
			if (model == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Null data"
				};
			}

			if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
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
				PasswordHash = _passwordHasher.HashPassword(model.Username, model.Password),
				RefreshToken = _tokenService.GenerateRefreshToken()
			};
			
			await _context.Users.AddAsync(newUser);
			await _context.SaveChangesAsync();

			return new AuthResponse()
			{
				Success = true,
				AccessToken = _tokenService.GenerateJwtToken(newUser.Username, newUser.Email, out DateTime jwTokenExpires),
				AccessTokenExpires = jwTokenExpires,
				RefreshToken = newUser.RefreshToken.Token,
				RefreshTokenExpires = newUser.RefreshToken.ExpiresAt
			};
		}

		public async Task<AuthResponse> RefreshToken(string requestRefreshToken)
		{
			var refreshToken = await _context.RefreshTokens.AsNoTracking().Include(u => u.User).FirstAsync(r => r.Token == requestRefreshToken);

			if (refreshToken == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Refresh token is invalid"
				};
			}

			if (refreshToken.IsExpired)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Refresh token is expired"
				};
			}

			return new AuthResponse()
			{
				Success = true
			};
		}
	}
}
