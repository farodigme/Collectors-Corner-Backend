using Collectors_Corner_Backend.Interfaces;
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
		private readonly ICurrentUserService _currentUser;
		private AccountService _accountService;
		public AccountController(AccountService accountService, ICurrentUserService currentUser)
		{
			_accountService = accountService;
			_currentUser = currentUser;
		}

		[HttpGet("getuser")]
		public async Task<IActionResult> GetUser([FromQuery] GetUserRequest request)
		{
			var result = await _accountService.GetUserAsync(_currentUser, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update-nickname")]
		public async Task<IActionResult> UpdateNickname([FromBody] UpdateNicknameRequest request)
		{
			var result = await _accountService.UpdateNicknameAsync(_currentUser, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
