using Collectors_Corner_Backend.Models.DTOs;
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

		public async Task<BaseResponse> UpdateNicknameAsync(string username, string nickname)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
			if (user == null)
			{
				return new BaseResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			user.Nickname = nickname;
			await _context.SaveChangesAsync();

			return new BaseResponse()
			{
				Success = true
			};
		}
	}
}
