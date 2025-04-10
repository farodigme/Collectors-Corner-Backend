using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs.Card;
using Collectors_Corner_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class CardService
	{
		private ApplicationContext _context;
		private ImageService _imageService;
		public CardService(ApplicationContext context, ImageService imageService)
		{
			_context = context;
			_imageService = imageService;
		}

		public async Task<CreateCardResponse> CreateCardAsync(ICurrentUserService currentUser, CreateCardRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return new CreateCardResponse() { Success = false, Error = "Invalid user" };

			var collection = await _context.Collections.Include(u => u.User.Username == currentUser.Username).FirstOrDefaultAsync(c => c.Id == request.CollectionId);
			if (collection == null)
				return new CreateCardResponse() { Success = false, Error = "Invalid collection" };

			var category = await _context.CardCategories.FirstOrDefaultAsync(c => c.Title == request.Category);
			if (category == null)
				return new CreateCardResponse() { Success = false, Error = "Invalid category" };

			var imageUploadResponse = await _imageService.UploadImageAsync(request.Image);
			if (!imageUploadResponse.Success)
				return new CreateCardResponse() { Success = false, Error = "Image upload error" };

			var card = new Card()
			{
				Title = request.Title,
				Description = request.Description,
				Category = category,
				ImageUrl = imageUploadResponse.NativeImageUrl
			};

			await _context.Collections.AddAsync(collection);
			await _context.SaveChangesAsync();

			return new CreateCardResponse()
			{
				Success = true,
				CardId = card.Id,
				NativeImageUrl = imageUploadResponse.NativeImageUrl,
				ThumbnailImageUrl = imageUploadResponse.ThumbnailImageUrl,
			};
		}
	}
}
