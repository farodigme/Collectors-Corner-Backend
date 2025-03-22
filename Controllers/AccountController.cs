using Collectors_Corner_Backend.Models.DTOs.Account;
using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : Controller
	{
		private AccountService _accountService;
		public AccountController(AccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet("getuser")]
		public async Task<IActionResult> GetUser([FromQuery] GetUserRequest request)
		{
			var username = User.Identity?.Name;
			var result = await _accountService.GetUserAsync(username, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update-nickname")]
		public async Task<IActionResult> UpdateNickname([FromBody] UpdateNicknameRequest request)
		{
			var username = User.Identity?.Name;
			var result = await _accountService.UpdateNicknameAsync(username, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
