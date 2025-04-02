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

		public AccountService(ApplicationContext context)
		{
			_context = context;
		}

		private static BaseResponse Success() => new() { Success = true };

		private static GetUserResponse InvalidUser(string message) => new()
		{
			Success = false,
			Error = message
		};

		private static BaseResponse Fail(string error) => new()
		{
			Success = false,
			Error = error
		};

		public async Task<GetUserResponse> GetUserAsync(ICurrentUserService currentUser, GetUserRequest request)
		{
			if (string.IsNullOrWhiteSpace(currentUser.Username) || currentUser.Username != request.Username.Trim())
				return InvalidUser("Invalid user");

			var user = await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Username == currentUser.Username);

			if (user is null)
				return InvalidUser("No such user");

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
				return Fail("Invalid user");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
				return Fail("User not found");

			user.Nickname = request.Nickname.Trim();
			await _context.SaveChangesAsync();

			return Success();
		}
	}
}
