using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Collectors_Corner_Backend.Models.Settings;

namespace Collectors_Corner_Backend.Services
{
	public class AuthService
	{
		private readonly TokenService _tokenService;
		private ApplicationContext _context;
		private PasswordHasher<string> _passwordHasher;
		private EmailService _emailService;
		private FrontendSettings _frontendSettings;

		public AuthService(
			ApplicationContext context,
			TokenService jwtService,
			EmailService emailService,
			FrontendSettings frontendSettings)
		{
			_context = context;
			_tokenService = jwtService;
			_emailService = emailService;
			_frontendSettings = frontendSettings;
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

		public async Task<AuthResponse> Register(RegistrationRequest request)
		{
			if (await _context.Users.AsNoTracking().AnyAsync(u => u.Username == request.Username))
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Username alredy taken"
				};
			}

			if (await _context.Users.AsNoTracking().AnyAsync(u => u.Email == request.Email))
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
				Username = request.Username,
				Email = request.Email,
				PasswordHash = _passwordHasher.HashPassword(request.Username, request.Password),
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

			var user = await _context.Users.Include(t => t.RefreshToken).FirstOrDefaultAsync(u => u.Username == username);

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

			if (user.RefreshToken.IsExpired)
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

		public async Task<AuthResponse> ForgotPassword(ForgotPasswordRequest request)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
			if (user == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Invalid Email"
				};
			}

			user.ResetToken = _tokenService.GenerateResetToken();
			await _context.SaveChangesAsync();

			await _emailService.SendAsync(user.Email, "Reset password", $"For reset password follow link: {_frontendSettings.BaseUrl}/reset-password/{user.ResetToken.Token}");

			return new AuthResponse
			{
				Success = true
			};
		}

		public async Task<AuthResponse> ResetPassword(ResetPasswordRequest request)
		{
			var user = await _context.Users.Include(r => r.ResetToken).FirstOrDefaultAsync(u => u.ResetToken.Token == request.ResetToken);
			if (user == null)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Invalid reset token"
				};
			}

			if (request.NewPassword != request.ConfirmPassword)
			{
				return new AuthResponse()
				{
					Success = false,
					Error = "Passwords are not equal"
				};
			}

			user.PasswordHash = _passwordHasher.HashPassword(user.Username, request.NewPassword);
			await _context.SaveChangesAsync();

			return new AuthResponse()
			{
				Success = true
			};
		}
	}
}
