using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;

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

		public async Task<AuthResponse> Login(LoginRequest model)
		{
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

			var newAccessToken = _tokenService.GenerateJwtToken(user);
			var newRefreshToken = _tokenService.GenerateRefreshToken();

			user.RefreshToken = newRefreshToken;
			await _context.SaveChangesAsync();

			return new AuthResponse()
			{
				Success = true,
				AccessToken = newAccessToken.Token,
				AccessTokenExpires = newAccessToken.ExpiresAt,
				RefreshToken = user.RefreshToken.Token,
				RefreshTokenExpires = user.RefreshToken.ExpiresAt
			};
		}

		public async Task<AuthResponse> Register(RegistrationRequest model)
		{
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

			var newRefreshToken = _tokenService.GenerateRefreshToken();

			var newUser = new User()
			{
				Username = model.Username,
				Email = model.Email,
				PasswordHash = _passwordHasher.HashPassword(model.Username, model.Password),
				RefreshToken = newRefreshToken
			};

			await _context.Users.AddAsync(newUser);
			await _context.SaveChangesAsync();

			var newAccessToken = _tokenService.GenerateJwtToken(newUser);

			return new AuthResponse()
			{
				Success = true,
				AccessToken = newAccessToken.Token,
				AccessTokenExpires = newAccessToken.ExpiresAt,
				RefreshToken = newRefreshToken.Token,
				RefreshTokenExpires = newRefreshToken.ExpiresAt
			};
		}

		public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
		{
			string accessToken = request.AccessToken;
			string refreshToken = request.RefreshToken;

			var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
			var username = principal.Identity.Name;

			var user = _context.Users.Include(t => t.RefreshToken).FirstOrDefault(u => u.Username == username);

			if (user == null)
			{
				return new AuthResponse
				{
					Success = false,
					Error = "Invalid access token"
				};
			}

			if (user.RefreshToken.Token != refreshToken)
			{
				return new AuthResponse
				{
					Success = false,
					Error = "Invalid refresh token"
				};
			}

			if (user.RefreshToken.ExpiresAt <= DateTime.UtcNow)
			{
				return new AuthResponse
				{
					Success = false,
					Error = "Refresh token expired"
				};
			}

			var newAccessToken = _tokenService.GenerateJwtToken(user);

			return new AuthResponse
			{
				Success = true,
				AccessToken = newAccessToken.Token,
				AccessTokenExpires = newAccessToken.ExpiresAt,
				RefreshToken = user.RefreshToken.Token,
				RefreshTokenExpires = user.RefreshToken.ExpiresAt
			};
		}
	}
}
