using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CollectionsController : Controller
	{
		private CollectionService _collectionService;
		public CollectionsController(CollectionService collectionService)
		{
			_collectionService = collectionService;
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateCollection(CreateCollectionRequest request)
		{
			var result = await _collectionService.CreateCollectionAsync(request);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
