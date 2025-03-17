using Collectors_Corner_Backend.Models;
using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Collectors_Corner_Backend.Services
{
	public class TokenService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly RefreshTokenSettings _refreshTokenSettings;
		private readonly ResetTokenSettings _resetTokenSettings;

		public TokenService(
			IOptions<JwtSettings> jwtSettings,
			IOptions<RefreshTokenSettings> refreshTokenSettings,
			IOptions<ResetTokenSettings> resetTokenSettings)
		{
			_jwtSettings = jwtSettings.Value;
			_refreshTokenSettings = refreshTokenSettings.Value;
			_resetTokenSettings = resetTokenSettings.Value;
		}

		public JWToken GenerateJwtToken(User user)
		{
			var tokenExpires = DateTime.Now.AddMinutes(_jwtSettings.TokenLifeTimeMin);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Email, user.Email)
			};

			var jwt = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			claims: claims,
			expires: tokenExpires,
			signingCredentials: new SigningCredentials(_jwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

			return new JWToken
			{
				Token = new JwtSecurityTokenHandler().WriteToken(jwt),
				ExpiresAt = tokenExpires
			};
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = _jwtSettings.GetTokenValidationParameters();

			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;

			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;

			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
				throw new SecurityTokenException("Invalid token");

			return principal;
		}

		private string GenerateRandomToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToHexString(randomNumber);
		}

		public RefreshToken GenerateRefreshToken()
		{
			return new RefreshToken
			{
				Token = GenerateRandomToken(),
				ExpiresAt = DateTime.Now.AddDays(_refreshTokenSettings.TokenLifeTimeDays),
			};
		}

		public ResetToken GenerateResetToken()
		{
			return new ResetToken
			{
				Token = GenerateRandomToken(),
				ExpiresAt = DateTime.Now.AddDays(_resetTokenSettings.TokenLifeTimeMin),
			};
		}
	}
}
