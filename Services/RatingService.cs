using Collectors_Corner_Backend.Models.Entities;

namespace Collectors_Corner_Backend.Services
{
	public class RatingService
	{
		private ApplicationContext _context;
		public RatingService(ApplicationContext context)
		{
			_context = context;
		}
	}
}
