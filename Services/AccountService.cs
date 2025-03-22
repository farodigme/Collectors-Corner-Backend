using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Account;
using Collectors_Corner_Backend.Models.DTOs.Auth;
using Collectors_Corner_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Services
{
	public class AccountService
	{
		private ApplicationContext _context;

		public AccountService(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<GetUserResponse> GetUserAsync(string identityUsername, GetUserRequest request)
		{
			if (string.IsNullOrEmpty(identityUsername))
			{
				return new GetUserResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			if (identityUsername != request.Username.Trim())
			{
				return new GetUserResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == identityUsername);

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
				Username = identityUsername,
				Nickname = user.Nickname,
				Email = user.Email,
				Created = user.CreatedAt
			};
		}

		public async Task<BaseResponse> UpdateNicknameAsync(string identityUsername, UpdateNicknameRequest request)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == identityUsername);
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
