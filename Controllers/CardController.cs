using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs.Card;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CardController : Controller
	{
		private readonly ICurrentUserService _currentUser;
		private CardService _cardService;
		public CardController(CardService cardService, ICurrentUserService currentUser)
		{
			_cardService = cardService;
			_currentUser = currentUser;
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateCard([FromForm] CreateCardRequest request)
		{
			var result = await _cardService.CreateCardAsync(_currentUser, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
