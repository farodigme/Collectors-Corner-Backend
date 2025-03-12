using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Collectors_Corner_Backend.Models.Settings
{
	public class JwtSettings
	{
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public string Key { get; set; }
		public int TokenLifeTimeMin { get; set; }
		public SymmetricSecurityKey GetSymmetricSecurityKey() =>
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

		public TokenValidationParameters GetTokenValidationParameters()
		{
			return new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = Issuer,
				ValidateAudience = true,
				ValidAudience = Audience,
				ValidateLifetime = true,
				IssuerSigningKey = GetSymmetricSecurityKey(),
				ValidateIssuerSigningKey = true,
				ClockSkew = TimeSpan.Zero
			};
		}
	}
}
