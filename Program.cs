using Collectors_Corner_Backend.Models.DataBase;
using Collectors_Corner_Backend.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;

namespace Collectors_Corner_Backend
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var configuration = builder.Configuration;

			builder.Services.Configure<JwtSettings>(configuration.GetRequiredSection(configuration.GetConnectionString("DefaultConnection")));
			builder.Services.AddDbContext<ApplicationContext>(options => options.UseMySql("", new MySqlServerVersion(new Version(8, 0, 32))));
			builder.Services.AddControllers();
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
