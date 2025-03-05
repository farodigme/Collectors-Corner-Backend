using Collectors_Corner_Backend.Models;
using Collectors_Corner_Backend.Models.DataBase;
using Collectors_Corner_Backend.Models.Settings;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;

namespace Collectors_Corner_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly JwtService _jwtService;
		private ApplicationContext _context;
		private PasswordHasher<string> _passwordHasher;

		public AuthController(JwtService jwtService, ApplicationContext context)
		{
			_jwtService = jwtService;
			_context = context;
			_passwordHasher = new PasswordHasher<string>();
		}

		[HttpGet("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var loginUser = new User()
			{
				Email = model.Email,
				Password = model.Password
			};

			var user = await _context.Users.FirstAsync(u => u.Email == model.Email);
			if (user == null) return NotFound();

			var isPasswordEqual = _passwordHasher.VerifyHashedPassword(user.Email, user.Password, model.Password);

			if (isPasswordEqual == PasswordVerificationResult.Success)
			{
				return Ok(_jwtService.GenerateJwtToken(user));
			}

			return Unauthorized("Неверный логин или пароль");
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			if (await _context.Users.AnyAsync(u => u.Email == model.Email))
				return BadRequest("Пользователь уже существует");

			var newUser = new User()
			{
				Email = model.Email,
				Password = _passwordHasher.HashPassword(model.Email, model.Password),
			};

			await _context.Users.AddAsync(newUser);
			await _context.SaveChangesAsync();

			return Ok(_jwtService.GenerateJwtToken(newUser));
		}

		[HttpGet("logout")]
		public async Task<IActionResult> Logout()
		{
			await Response.HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
			return Ok();
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return Unauthorized();
		}
	}
}
