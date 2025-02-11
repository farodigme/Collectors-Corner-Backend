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
