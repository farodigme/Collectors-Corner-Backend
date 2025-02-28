using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> _logger;

		public AuthController(ILogger<AuthController> logger)
		{
			_logger = logger;
		}

		[HttpGet("HealthCheck")]
		public IActionResult HealthCheck()
		{
			return Ok("Work");
		}

		[Authorize]
		[HttpGet("Test")]
		public IActionResult Test()
		{
			return Ok("Test");
		}
	}
}
