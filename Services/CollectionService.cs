using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Models.Entities;

namespace Collectors_Corner_Backend.Services
{
	public class CollectionService
	{
		private ApplicationContext _context;
		public CollectionService(ApplicationContext context)
		{
			_context = context;
		}
	}
}
