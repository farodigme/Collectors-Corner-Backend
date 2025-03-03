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
		private ApplicationContext _context;

		public AuthController(ILogger<AuthController> logger, IOptions<JwtSettings> jwtSettings, ApplicationContext context)
		{
			_logger = logger;
			_jwtSettings = jwtSettings.Value;
			_context = context;
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

		[HttpPost("register")]
		public async IActionResult Register([FromBody] LoginModel model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var user = await _context.Users.FindAsync(model);
			if (user != null) return Ok();

			//register user
		}
	}
}
