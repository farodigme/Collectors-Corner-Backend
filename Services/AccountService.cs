using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Account;
using Collectors_Corner_Backend.Models.DTOs.Auth;
using Collectors_Corner_Backend.Models.Entities;
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

		public async Task<GetUserResponse> GetUserAsync(ICurrentUserService currentUser, GetUserRequest request)
		{
			if (string.IsNullOrEmpty(currentUser.Username))
			{
				return new GetUserResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			if (currentUser.Username != request.Username.Trim())
			{
				return new GetUserResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);

			if (user == null)
			{
				return new GetUserResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			return new GetUserResponse()
			{
				Success = true,
				Username = currentUser.Username,
				Nickname = user.Nickname,
				Email = user.Email,
				Created = user.CreatedAt
			};
		}

		public async Task<BaseResponse> UpdateNicknameAsync(ICurrentUserService currentUser, UpdateNicknameRequest request)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser.Username);
			if (user == null)
			{
				return new BaseResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			user.Nickname = request.Nickname;
			await _context.SaveChangesAsync();

			return new BaseResponse()
			{
				Success = true
			};
		}
	}
}
