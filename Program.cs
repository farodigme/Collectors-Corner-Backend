using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Models.Settings;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Collectors_Corner_Backend
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var configuration = builder.Configuration;

			builder.Services.AddControllers();
			builder.Services.Configure<JWTokenSettings>(configuration.GetRequiredSection("JwtSettings"));
			builder.Services.Configure<RefreshTokenSettings>(configuration.GetRequiredSection("RefreshTokenSettings"));

			builder.Services.AddDbContext<ApplicationContext>(options =>
			{
				options.UseMySql(
					configuration.GetConnectionString("DefaultConnection"),
					new MySqlServerVersion(new Version(8, 0, 32))
					);
			});
			builder.Services.AddScoped<AccountService>();
			builder.Services.AddSingleton<TokenService>();
			builder.Services.AddAuthorization();
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JwtSettings:Issuer"],
						ValidateAudience = true,
						ValidAudience = configuration["JwtSecrets:Audience"],
						ValidateLifetime = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
						ValidateIssuerSigningKey = true
					};
				});

			var app = builder.Build();

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
