using Collectors_Corner_Backend.Interfaces;
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
		private readonly ICurrentUserService _currentUser;
		private CollectionService _collectionService;
		public CollectionController(CollectionService collectionService, ICurrentUserService currentUser)
		{
			_collectionService = collectionService;
			_currentUser = currentUser;
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateCollection([FromForm] CreateCollectionRequest request)
		{
			var result = await _collectionService.CreateCollectionAsync(_currentUser, request);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
