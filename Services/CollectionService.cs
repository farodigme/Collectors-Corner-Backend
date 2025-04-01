using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class CollectionService
	{
		private ApplicationContext _context;
		private ImageService _imageService;
		public CollectionService(ApplicationContext context, ImageService imageService)
		{
			_context = context;
			_imageService = imageService;
		}

		public async Task<CreateCollectionResponse> CreateCollectionAsync(ICurrentUserService currentUser, CreateCollectionRequest request)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
			{
				return new CreateCollectionResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			var category = await _context.CollectionCategories.FirstOrDefaultAsync(c => c.Title == request.Category);

			if (category == null)
			{
				return new CreateCollectionResponse()
				{
					Success = false,
					Error = "Invalid category"
				};
			}

			var imageUploadResponse = await _imageService.UploadImageAsync(request.Image);
			if (!imageUploadResponse.Success)
			{
				return new CreateCollectionResponse()
				{
					Success = false,
					Error = imageUploadResponse.Error
				};
			}

			var newCollection = new Collection()
			{
				User = user,
				Title = request.Title,
				Description = request.Description,
				Category = category,
				ImageUrl = imageUploadResponse.NativeImageUrl,
				IsPublic = request.IsPublic
			};

			await _context.Collections.AddAsync(newCollection);
			await _context.SaveChangesAsync();

			return new CreateCollectionResponse()
			{
				Success = true,
				CollectionId = newCollection.Id,
				NativeImageUrl = imageUploadResponse.NativeImageUrl,
				ThumbnailImageUrl = imageUploadResponse.ThumbnailImageUrl
			};
		}

		//public async Task<GetCollectionsResponse> GetCollectionsByUserAsync(ICurrentUserService currentUser)
		//{
		//	var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
		//	if (user == null)
		//	{
		//		return new GetCollectionsResponse()
		//		{
		//			Success = false,
		//			Error = "Invalid user"
		//		};
		//	}

		//	var collections = await _context.Collections.AsNoTracking().Include(u => u.User).Where(u => u.User == user).ToListAsync();
		//	if (collections == null || collections.Count <= 0)
		//	{
		//		return new GetCollectionsResponse()
		//		{
		//			Success = false,
		//			Error = "User do not have collections"
		//		};
		//	}

		//	return new GetCollectionsResponse()
		//	{
		//		Success = true,
		//		Collections = collections
		//	};
		//}
	}
}