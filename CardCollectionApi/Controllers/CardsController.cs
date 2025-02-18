using CardCollectionApi.DataBase;
using CardCollectionApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardCollectionApi.Controllers
{
	public class CardsController : Controller
	{
		private ApplicationContext _context;
		public CardsController(ApplicationContext context)
		{
			_context = context;
		}

		[HttpGet("Get")]
		public async Task<IActionResult> Get(int id)
		{
			if (id == 0) return BadRequest();

			var card = await _context.Cards.FindAsync(id);
			if (card == null) return NotFound();

			return Ok(card);
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create([FromBody] Cards inputCard)
		{
			if (inputCard == null) return BadRequest();

			var newCard = await _context.AddAsync(inputCard);
			await _context.SaveChangesAsync();

			var response = new
			{
				Status = "Success",
				Id = newCard.Entity.Id
			};

			return Ok(response);
		}

		[HttpPost("Edit")]
		public async Task<IActionResult> Edit([FromBody] Cards inputCard)
		{
			if (inputCard == null) return BadRequest();

			var card = await _context.Cards.FindAsync(inputCard.Id);
			if (card == null) return NotFound();

			_context.Cards.Update(card).CurrentValues.SetValues(inputCard);
			await _context.SaveChangesAsync();

			return Ok(card);
		}

		[HttpDelete("Delete")]
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0) return BadRequest();

			var card = await _context.Cards.FindAsync(id);
			if (card == null) return NotFound();

			_context.Cards.Remove(card);
			await _context.SaveChangesAsync();

			return Ok();
		}
	}
}
