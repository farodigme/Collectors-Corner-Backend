using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Models.Entities;
using ImageHosting.Extensions.Mappers;
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
		private static CreateCollectionResponse FailCreateCollection(string error) => new()
		{
			Success = false,
			Error = error
		};
		private static GetCollectionsResponse FailCollections(string error) => new()
		{
			Success = false,
			Error = error
		};

		public async Task<CreateCollectionResponse> CreateCollectionAsync(ICurrentUserService currentUser, CreateCollectionRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return FailCreateCollection("Invalid user");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
				return FailCreateCollection("User not found");

			var category = await _context.CollectionCategories.FirstOrDefaultAsync(c => c.Title == request.Category);
			if (category == null)
				return FailCreateCollection("Invalid category");

			var imageUploadResponse = await _imageService.UploadImageAsync(request.Image);
			if (!imageUploadResponse.Success)
				return FailCreateCollection(imageUploadResponse.Error ?? "Image upload failed");

			var newCollection = new Collection
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

			return new CreateCollectionResponse
			{
				Success = true,
				CollectionId = newCollection.Id,
				NativeImageUrl = imageUploadResponse.NativeImageUrl,
				ThumbnailImageUrl = imageUploadResponse.ThumbnailImageUrl
			};
		}

		public async Task<GetCollectionsResponse> GetCollectionsByUserAsync(ICurrentUserService currentUser)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return FailCollections("Invalid user");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
				return FailCollections("User not found");

			var collections = await _context.Collections
				.AsNoTracking()
				.Include(c => c.Category)
				.Where(c => c.UserId == user.Id)
				.ToListAsync();

			var collectionsDto = CollectionMapper.ToDtoList(collections);

			return new GetCollectionsResponse()
			{
				Success = true,
				Collections = collectionsDto
			};
		}

		public async Task<GetCollectionsResponse> UpdateUserCollectionAsync(ICurrentUserService currentUser, UpdateCollectionRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return FailCollections("Invalid user");

			var collection = await _context.Collections
				.Include(c => c.User.Username == currentUser.Username)
				.FirstOrDefaultAsync(u => u.Id == request.collectionId);

			if (collection == null)
				return FailCollections("Collection not found");

			collection.Title = request.Title;
			collection.Description = request.Description;
			collection.Category.Title = request.Category;
			collection.IsPublic = request.IsPublic;
			collection.ImageUrl = null;

			throw new NotImplementedException();
		}
	}
}