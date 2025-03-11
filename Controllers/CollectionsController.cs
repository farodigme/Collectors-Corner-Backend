using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CollectionsController : Controller
	{
		[HttpPost("Test"), Authorize]
		public IActionResult Index()
		{
			return Ok();
		}
	}
}
