using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Collectors_Corner_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private AuthService _userService;
		public AuthController(AuthService userService)
		{
			_userService = userService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var result = await _userService.Login(request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
		{
			var result = await _userService.Register(request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
		{
			var result = await _userService.RefreshToken(request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("reset")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
