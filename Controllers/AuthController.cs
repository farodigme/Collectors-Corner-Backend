using Collectors_Corner_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Collectors_Corner_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> _logger;
		private readonly JwtSettings _jwtSettings;

		public AuthController(ILogger<AuthController> logger, IOptions<JwtSettings> jwtSettings)
		{
			_logger = logger;
			_jwtSettings = jwtSettings.Value;
		}

		public string GenerateJwtToken(string username)
		{
			var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
			var jwt = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
				signingCredentials: new SigningCredentials(_jwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		[HttpGet("login/{username}")]
		public IActionResult Login(string username)
		{
			if (string.IsNullOrEmpty(username)) return BadRequest("Empty username");

			return Ok(GenerateJwtToken(username));
		}
	}
}
