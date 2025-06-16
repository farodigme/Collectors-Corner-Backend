using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs.Rating;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class RatingController : Controller
	{
		private readonly ICurrentUserService _currentUser;
		private RatingService _ratingService;
		public RatingController(ICurrentUserService currentUserService, RatingService ratingService)
		{
			_currentUser = currentUserService;
			_ratingService = ratingService;
		}

		[HttpPost("update-collection-rating")]
		public async Task<IActionResult> UpdateCollectionRating([FromBody] UpdateCollectionRatingRequest request) 
		{
			var result = await _ratingService.UpdateCollectionRating(_currentUser, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
