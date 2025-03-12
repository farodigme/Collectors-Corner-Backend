using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Models.Settings;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var configuration = builder.Configuration;

			builder.Services.AddControllers();
			builder.Services.AddDbContext<ApplicationContext>(options =>
			{
				options.UseMySql(
					configuration.GetConnectionString("DefaultConnection"),
					new MySqlServerVersion(new Version(8, 0, 32))
					);
			});

			builder.Services.Configure<JwtSettings>(configuration.GetRequiredSection("JwtSettings"));
			var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

			builder.Services.Configure<RefreshTokenSettings>(configuration.GetRequiredSection("RefreshTokenSettings"));
			
			builder.Services.AddScoped<AccountService>();
			builder.Services.AddSingleton<TokenService>();
			builder.Services.AddAuthorization();

			
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = jwtSettings.GetTokenValidationParameters();
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
