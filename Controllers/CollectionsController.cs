﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectors_Corner_Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CollectionsController : Controller
	{
		[HttpGet("Test")]
		[Authorize]
		public IActionResult Index()
		{
			return Ok("Work");
		}
	}
}
