using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Account;
using Collectors_Corner_Backend.Models.DTOs.Rating;
using Collectors_Corner_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class RatingService
	{
		private ApplicationContext _context;
		public RatingService(ApplicationContext context)
		{
			_context = context;
		}

		private static T Fail<T>(string error) where T : BaseResponse, new() => new() { Success = false, Error = error };
		private static T Success<T>(Action<T>? configure = null) where T : BaseResponse, new()
		{
			var response = new T { Success = true };
			configure?.Invoke(response);
			return response;
		}

		public async Task<BaseResponse> UpdateCollectionRating(ICurrentUserService currentUser, UpdateCollectionRatingRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<BaseResponse>("Invalid user");

			var user = await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Username == currentUser.Username);

			if (user is null)
				return Fail<BaseResponse>("No such user");

			var currentRating = await _context.Ratings.Include(u => u.UserId == user.Id).FirstOrDefaultAsync(r => r.CollectionId == request.CollectionId);

			if (currentRating is null)
			{
				var newRating = new Rating()
				{
					CollectionId = request.CollectionId,
					UserId = user.Id,
					RatingValue = request.RatingValue
				};
			}
			else
			{
				currentRating.RatingValue = request.RatingValue;
			}

			await _context.SaveChangesAsync();
			return Success<BaseResponse>();
		}
	}
}
