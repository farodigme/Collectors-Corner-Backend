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
	public class AccountController : ControllerBase
	{
		private AccountService _userService;
		public AccountController(AccountService userService)
		{
			_userService = userService;
		}

		[HttpGet("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest model)
		{
			var result = await _userService.Login(model);
			if (!result.Success) return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
		{
			var result = await _userService.Register(model);
			if (!result.Success) return BadRequest(result);
			return Ok(result);
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest model)
		{
			var result = await _userService.RefreshToken(model);
			if (!result.Success) return BadRequest(result);
			return Ok(result);
		}
	}
}
