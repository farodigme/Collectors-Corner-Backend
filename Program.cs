using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models;
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

			var frontendSettings = builder.Configuration.GetSection("FrontendSettings").Get<FrontendSettings>();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowFrontend", policy =>
				{
					policy.WithOrigins(frontendSettings.BaseUrl)
						  .AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials();
				});
			});

			builder.Services.Configure<JwtSettings>(configuration.GetRequiredSection("JwtSettings"));
			var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

			builder.Services.Configure<RefreshTokenSettings>(configuration.GetRequiredSection("RefreshTokenSettings"));
			builder.Services.Configure<ResetTokenSettings>(configuration.GetRequiredSection("ResetTokenSettings"));
			builder.Services.Configure<FrontendSettings>(configuration.GetRequiredSection("FrontendSettings"));
			builder.Services.Configure<EmailSettings>(configuration.GetRequiredSection("EmailSettings"));
			builder.Services.Configure<ImageServiceSettings>(configuration.GetRequiredSection("ImageServiceSettings"));

			builder.Services.AddHttpClient();
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<AuthService>();
			builder.Services.AddScoped<AccountService>();
			builder.Services.AddScoped<CollectionService>();
			builder.Services.AddScoped<CardService>();
			builder.Services.AddSingleton<EmailService>();
			builder.Services.AddScoped<ImageService>();
			builder.Services.AddSingleton<TokenService>();
			builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();	

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			  {
				  options.TokenValidationParameters = jwtSettings.GetTokenValidationParameters();
			  });

			builder.Services.AddAuthorization();

			var app = builder.Build();

			app.UseCors("AllowFrontend");
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
