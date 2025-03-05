using Collectors_Corner_Backend.Models.DataBase;
using Collectors_Corner_Backend.Models.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Collectors_Corner_Backend.Services
{
	public class JwtService
	{
		private readonly JwtSettings _jwtSettings;
		public JwtService(JwtSettings jwtSettings)
		{
			_jwtSettings = jwtSettings;
		}
		public string GenerateJwtToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Email, user.Email)
			};
			var jwt = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
				signingCredentials: new SigningCredentials(_jwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}
	}
}
