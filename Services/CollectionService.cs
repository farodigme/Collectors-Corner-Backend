using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Models.DTOs.ImageService;
using Collectors_Corner_Backend.Models.Entities;
using ImageHosting.Extensions.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class CollectionService
	{
		private readonly ApplicationContext _context;
		private readonly ImageService _imageService;

		public CollectionService(ApplicationContext context, ImageService imageService)
		{
			_context = context;
			_imageService = imageService;
		}

		private T Fail<T>(string error) where T : BaseResponse, new() => new() { Success = false, Error = error };
		private T Success<T>(Action<T>? configure = null) where T : BaseResponse, new()
		{
			var response = new T { Success = true };
			configure?.Invoke(response);
			return response;
		}

		public async Task<CreateCollectionResponse> CreateCollectionAsync(ICurrentUserService currentUser, CreateCollectionRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<CreateCollectionResponse>("Invalid user");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
				return Fail<CreateCollectionResponse>("User not found");

			var category = await _context.CollectionCategories.FirstOrDefaultAsync(c => c.Title == request.Category);
			if (category == null)
				return Fail<CreateCollectionResponse>("Invalid category");

			var imageUploadResponse = await _imageService.UploadImageAsync(request.Image);
			if (!imageUploadResponse.Success)
				return Fail<CreateCollectionResponse>(imageUploadResponse.Error ?? "Image upload failed");

			var newCollection = new Collection
			{
				User = user,
				Title = request.Title.Trim(),
				Description = request.Description?.Trim(),
				Category = category,
				ImageUrl = imageUploadResponse.NativeImageUrl,
				IsPublic = request.IsPublic
			};

			await _context.Collections.AddAsync(newCollection);
			await _context.SaveChangesAsync();

			return Success<CreateCollectionResponse>(r =>
			{
				r.CollectionId = newCollection.Id;
				r.NativeImageUrl = imageUploadResponse.NativeImageUrl;
				r.ThumbnailImageUrl = imageUploadResponse.ThumbnailImageUrl;
			});
		}

		public async Task<GetCollectionsResponse> GetCollectionsByUserAsync(ICurrentUserService currentUser)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<GetCollectionsResponse>("Invalid user");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
				return Fail<GetCollectionsResponse>("User not found");

			var collections = await _context.Collections
				.AsNoTracking()
				.Include(c => c.Category)
				.Where(c => c.UserId == user.Id)
				.ToListAsync();

			var collectionsDto = CollectionMapper.ToDtoList(collections);

			return Success<GetCollectionsResponse>(r => r.Collections = collectionsDto);
		}

		public async Task<GetCollectionsResponse> GetPopularCollectionsAsync()
		{
			var collections = await _context.Collections
				.AsNoTracking()
				.Include(c => c.Category)
				.Where(c => c.IsPublic)
				.OrderByDescending(c => c.Views)
				.ToListAsync();

			var collectionsDto = CollectionMapper.ToDtoList(collections);

			return Success<GetCollectionsResponse>(r => r.Collections = collectionsDto);
		}

		public async Task<UpdateCollectionResponse> UpdateUserCollectionAsync(ICurrentUserService currentUser, UpdateCollectionRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<UpdateCollectionResponse>("Invalid user");

			var collection = await _context.Collections
				.Include(c => c.Category)
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.Id == request.collectionId && c.User.Username == currentUser.Username);

			if (collection == null)
				return Fail<UpdateCollectionResponse>("Collection not found");

			var category = await _context.CollectionCategories
				.FirstOrDefaultAsync(c => c.Title == request.Category);

			if (category == null)
				return Fail<UpdateCollectionResponse>("Invalid category");

			collection.Title = request.Title.Trim();
			collection.Description = request.Description?.Trim();
			collection.Category = category;
			collection.IsPublic = request.IsPublic;

			string? nativeUrl = collection.ImageUrl;
			string? thumbUrl = nativeUrl is not null ? nativeUrl + "_thumb" : null;

			if (request.Image != null)
			{
				var imageUploadResponse = await _imageService.UploadImageAsync(request.Image);
				if (!imageUploadResponse.Success)
					return Fail<UpdateCollectionResponse>(imageUploadResponse.Error ?? "Image upload failed");

				collection.ImageUrl = imageUploadResponse.NativeImageUrl;

				nativeUrl = imageUploadResponse.NativeImageUrl;
				thumbUrl = imageUploadResponse.ThumbnailImageUrl;
			}

			await _context.SaveChangesAsync();

			return Success<UpdateCollectionResponse>(r =>
			{
				r.collectionId = collection.Id;
				r.Title = collection.Title;
				r.Description = collection.Description;
				r.Category = collection.Category.Title;
				r.IsPublic = collection.IsPublic;
				r.NativeImageUrl = nativeUrl;
				r.ThumbnailImageUrl = thumbUrl;
			});
		}


		public async Task<BaseResponse> DeleteCollectionAsync(ICurrentUserService currentUser, int collectionId)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<BaseResponse>("Invalid user");

			var collection = await _context.Collections
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.Id == collectionId && c.User.Username == currentUser.Username);

			if (collection == null)
				return Fail<BaseResponse>("Invalid collection");

			_context.Collections.Remove(collection);
			await _context.SaveChangesAsync();

			return Success<BaseResponse>();
		}
	}
}
