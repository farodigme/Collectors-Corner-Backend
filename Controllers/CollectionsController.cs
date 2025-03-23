using Collectors_Corner_Backend.Models.DTOs.Collection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CollectionsController : Controller
	{
		[HttpPost("create")]
		public IActionResult CreateCollection(CreateCollectionRequest request)
		{
			return Ok();
		}
	}
}
