using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class CollectionService
	{
		private ApplicationContext _context;
		public CollectionService(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<BaseResponse> CreateCollectionAsync(CreateCollectionRequest request)
		{
			var category = await _context.CollectionCategories.FirstOrDefaultAsync(c => c.Title == request.Category);

			if (category == null)
			{
				return new BaseResponse()
				{
					Success = false,
					Error = "Invalid category"
				};
			}

			// Image upload

			var newCollection = new Collection()
			{
				Title = request.Title,
				Description = request.Description,
				Category = category
			};

			await _context.Collections.AddAsync(newCollection);
			await _context.SaveChangesAsync();

			return new BaseResponse()
			{
				Success = true
			};
		}
	}
}
