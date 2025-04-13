using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Card;
using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class CardService
	{
		private readonly ApplicationContext _context;
		private readonly ImageService _imageService;

		public CardService(ApplicationContext context, ImageService imageService)
		{
			_context = context;
			_imageService = imageService;
		}

		private static T Fail<T>(string error) where T : BaseResponse, new() => new() { Success = false, Error = error };
		private static T Success<T>(Action<T>? configure = null) where T : BaseResponse, new()
		{
			var response = new T { Success = true };
			configure?.Invoke(response);
			return response;
		}

		public async Task<CreateCardResponse> CreateCardAsync(ICurrentUserService currentUser, CreateCardRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<CreateCardResponse>("Invalid user");

			var collection = await _context.Collections
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.Id == request.CollectionId && c.User.Username == currentUser.Username);
			if (collection == null)
				return Fail<CreateCardResponse>("Invalid collection");

			var category = await _context.CardCategories.FirstOrDefaultAsync(c => c.Title == request.Category);
			if (category == null)
				return Fail<CreateCardResponse>("Invalid category");

			var imageUploadResponse = await _imageService.UploadImageAsync(request.Image);
			if (!imageUploadResponse.Success)
				return Fail<CreateCardResponse>(imageUploadResponse.Error ?? "Image upload failed");

			var card = new Card
			{
				Title = request.Title.Trim(),
				Description = request.Description?.Trim(),
				Category = category,
				ImageUrl = imageUploadResponse.NativeImageUrl,
				Collection = collection,
				IsPublic = request.IsPublic
			};

			await _context.Cards.AddAsync(card);
			await _context.SaveChangesAsync();

			return Success<CreateCardResponse>(r =>
			{
				r.CardId = card.Id;
				r.NativeImageUrl = imageUploadResponse.NativeImageUrl;
				r.ThumbnailImageUrl = imageUploadResponse.ThumbnailImageUrl;
			});
		}

		public async Task<UpdateCardResponse> UpdateCardAsync(ICurrentUserService currentUser, UpdateCardRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<UpdateCardResponse>("Invalid user");

			var card = await _context.Cards
				.Include(c => c.Category)
				.Include(c => c.Collection)
				.ThenInclude(c => c.User)
				.FirstOrDefaultAsync(c => c.Id == request.cardId && c.Collection.User.Username == currentUser.Username);

			if (card == null)
				return Fail<UpdateCardResponse>("Card not found");

			var category = await _context.CardCategories
				.FirstOrDefaultAsync(c => c.Title == request.Category);

			if (category == null)
				return Fail<UpdateCardResponse>("Invalid category");

			card.Title = request.Title.Trim();
			card.Description = request.Description?.Trim();
			card.Category = category;
			card.IsPublic = request.IsPublic;

			string? nativeUrl = card.ImageUrl;
			string? thumbUrl = nativeUrl is not null ? nativeUrl + "_thumb" : null;

			if (request.Image != null)
			{
				var imageUploadResponse = await _imageService.UploadImageAsync(request.Image);
				if (!imageUploadResponse.Success)
					return Fail<UpdateCardResponse>(imageUploadResponse.Error ?? "Image upload failed");

				card.ImageUrl = imageUploadResponse.NativeImageUrl;
				nativeUrl = imageUploadResponse.NativeImageUrl;
				thumbUrl = imageUploadResponse.ThumbnailImageUrl;
			}

			await _context.SaveChangesAsync();

			return Success<UpdateCardResponse>(r =>
			{
				r.cardId = card.Id;
				r.Title = card.Title;
				r.Description = card.Description;
				r.Category = card.Category.Title;
				r.IsPublic = card.IsPublic;
				r.NativeImageUrl = nativeUrl;
				r.ThumbnailImageUrl = thumbUrl;
			});
		}

		public async Task<BaseResponse> DeleteCardAsync(ICurrentUserService currentUser, int cardId)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<BaseResponse>("Invalid user");

			var card = await _context.Cards
				.Include(c => c.Collection)
				.ThenInclude(c => c.User)
				.FirstOrDefaultAsync(c => c.Id == cardId && c.Collection.User.Username == currentUser.Username);

			if (card == null)
				return Fail<BaseResponse>("Invalid card");

			_context.Cards.Remove(card);
			await _context.SaveChangesAsync();

			return Success<BaseResponse>();
		}
	}
}
