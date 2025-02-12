using CardCollectionApi.DataBase;
using CardCollectionApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardCollectionApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CollectionsControlles : ControllerBase
	{
		private ApplicationContext _context;
		public CollectionsControlles(ApplicationContext context)
		{
			_context = context;
		}

		[HttpGet("GetCollection")]
		public async Task<IActionResult> GetCollection(int id)
		{
			if (id == 0) return BadRequest();
			var collection = await _context.Collections.FindAsync(id);
			if (collection == null) return NotFound();
			return Ok(collection);
		}

		[HttpPost("Create")]
		public async Task<ActionResult> Create([FromBody] Collections collection)
		{
			if (collection == null) return BadRequest();

			var newCollection = await _context.AddAsync(collection);
			await _context.SaveChangesAsync();

			var response = new
			{
				Status = "Success",
				Id = newCollection.Entity.Id
			};

			return Ok(response);
		}

	}
}
