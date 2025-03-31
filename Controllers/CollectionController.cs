using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CollectionController : Controller
	{
		private CollectionService _collectionService;
		public CollectionController(CollectionService collectionService)
		{
			_collectionService = collectionService;
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateCollection([FromForm] CreateCollectionRequest request)
		{
			var username = User.Identity?.Name;
			var result = await _collectionService.CreateCollectionAsync(username, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
