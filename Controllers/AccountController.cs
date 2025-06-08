using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs.Account;
using Collectors_Corner_Backend.Models.DTOs.Collection;
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

		[HttpGet("get-user")]
		public async Task<IActionResult> GetUser()
		{
			var result = await _accountService.GetUserAsync(_currentUser);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update-nickname")]
		public async Task<IActionResult> UpdateNickname([FromBody] UpdateNicknameRequest request)
		{
			var result = await _accountService.UpdateNicknameAsync(_currentUser, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update-email")]
		public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailRequest request)
		{
			var result = await _accountService.UpdateEmailAsync(_currentUser, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update-avatar")]
		public async Task<IActionResult> UpdateAvatar([FromForm] UpdateAvatarRequest request)
		{
			var result = await _accountService.UpdateAvatarAsync(_currentUser, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpPost("add-favorite-collection")]
		public async Task<IActionResult> AddCollectionToFavorite([FromBody] int CollectionId)
		{
			var result = await _accountService.AddCollectionToFavorite(_currentUser, CollectionId);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("get-favorite-colelctions")]
		public async Task<IActionResult> GetFavoriteCollections()
		{
			var result = await _accountService.GetFavoriteCollections(_currentUser);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("delete-favorite-collection")]
		public async Task<IActionResult> DeleteFavoriteCollection([FromBody] int collectionId)
		{
			var result = await _accountService.DeleteFavoriteCollection(_currentUser, collectionId);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("delete-favorite-collections")]
		public async Task<IActionResult> DeleteFavoriteCollection([FromBody] IEnumerable<int> collectionIds)
		{
			var result = await _accountService.DeleteFavoriteCollections(_currentUser, collectionIds);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
