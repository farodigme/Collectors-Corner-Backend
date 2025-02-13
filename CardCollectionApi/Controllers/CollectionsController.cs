using CardCollectionApi.DataBase;
using CardCollectionApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardCollectionApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CollectionsController : ControllerBase
	{
		private ApplicationContext _context;
		public CollectionsController(ApplicationContext context)
		{
			_context = context;
		}

		[HttpGet("Get")]
		public async Task<IActionResult> Get(int id)
		{
			if (id == 0) return BadRequest();

			var collection = await _context.Collections.FindAsync(id);
			if (collection == null) return NotFound();

			return Ok(collection);
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create([FromBody] Collections inputCollection)
		{
			if (inputCollection == null) return BadRequest();

			var newCollection = await _context.AddAsync(inputCollection);
			await _context.SaveChangesAsync();

			var response = new
			{
				Status = "Success",
				Id = newCollection.Entity.Id
			};

			return Ok(response);
		}

		[HttpPost("Edit")]
		public async Task<IActionResult> Edit([FromBody] Collections inputCollection)
		{
			if (inputCollection == null) return BadRequest();

			var collection = await _context.Collections.FindAsync(inputCollection.Id);
			if (collection == null) return NotFound();

			_context.Collections.Update(collection).CurrentValues.SetValues(inputCollection);
			await _context.SaveChangesAsync();

			return Ok(collection);
		}

		[HttpDelete("Delete")]
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0) return BadRequest();

			var collection = await _context.Collections.FindAsync(id);
			if (collection == null) return NotFound();

			_context.Collections.Remove(collection);
			await _context.SaveChangesAsync();

			return Ok();
		}
	}
}
