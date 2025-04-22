using Collectors_Corner_Backend.Extensions.Mappers;
using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Account;
using Collectors_Corner_Backend.Models.DTOs.Auth;
using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Models.Entities;
using ImageHosting.Extensions.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Collectors_Corner_Backend.Services
{
	public class AccountService
	{
		private ApplicationContext _context;
		private ImageService _imageService;
		public AccountService(ApplicationContext context, ImageService imageService)
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

		public async Task<GetUserResponse> GetUserAsync(ICurrentUserService currentUser)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<GetUserResponse>("Invalid user");

			var user = await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Username == currentUser.Username);

			if (user is null)
				return Fail<GetUserResponse>("No such user");

			var userCollections = await _context.Collections
				.AsNoTracking()
				.Include(c => c.User)
				.Include(c => c.Category)
				.Where(c => c.UserId == user.Id)
				.ToListAsync();

			return UserMapper.ToGetUserResponse(user, userCollections);
		}

		public async Task<BaseResponse> UpdateNicknameAsync(ICurrentUserService currentUser, UpdateNicknameRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<BaseResponse>("Invalid user");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
				return Fail<BaseResponse>("User not found");

			user.Nickname = request.Nickname.Trim();
			await _context.SaveChangesAsync();

			return Success<BaseResponse>();
		}

		public async Task<BaseResponse> UpdateEmailAsync(ICurrentUserService currentUser, UpdateEmailRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<BaseResponse>("Invalid user");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
				return Fail<BaseResponse>("User not found");

			user.Email = request.Email.Trim();
			await _context.SaveChangesAsync();

			return Success<BaseResponse>();
		}

		public async Task<UpdateAvatarResponse> UpdateAvatarAsync(ICurrentUserService currentUser, UpdateAvatarRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username))
				return Fail<UpdateAvatarResponse>("Invalid user");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
				return Fail<UpdateAvatarResponse>("User not found");

			var imageUploadResponse = await _imageService.UploadImageAsync(request.Image);
			if (!imageUploadResponse.Success)
				return Fail<UpdateAvatarResponse>(imageUploadResponse.Error ?? "Image upload failed");

			user.AvatarUrl = imageUploadResponse.NativeImageUrl;
			await _context.SaveChangesAsync();

			return Success<UpdateAvatarResponse>(r =>
			{
				r.NativeImageUrl = imageUploadResponse.NativeImageUrl;
				r.ThumbnailImageUrl = imageUploadResponse.ThumbnailImageUrl;
			});
		}
	}
}
