using Collectors_Corner_Backend.Interfaces;
using Collectors_Corner_Backend.Models.DTOs;
using Collectors_Corner_Backend.Models.DTOs.Account;
using Collectors_Corner_Backend.Models.DTOs.Auth;
using Collectors_Corner_Backend.Models.DTOs.Collection;
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

			var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == currentUser.Username);

			if (user == null)
			{
				return new GetUserResponse()
				{
					Success = false,
					Error = "Invalid user"
				};
			}

			var userCollections = await _context.Collections.AsNoTracking().Include(u => u.User).Include(c => c.Category).Where(u => u.User == user).ToListAsync();
			if (userCollections.Count <= 0)
			{
				return new GetUserResponse()
				{
					Success = true,
					Username = currentUser.Username,
					Nickname = user.Nickname,
					Email = user.Email,
					Created = user.CreatedAt
				};
			}
			
			var collectionsDto = new List<CollectionDto>();
			foreach (var collection in userCollections)
			{
				collectionsDto.Add(new CollectionDto()
				{
					Id = collection.Id,
					Title = collection.Title,
					Description = collection.Description,
					Category = collection.Category.Title,
					ImageUrl = collection.ImageUrl,
					IsPublic = collection.IsPublic
				});
			}

			return new GetUserResponse()
			{
				Success = true,
				Username = currentUser.Username,
				Nickname = user.Nickname,
				Email = user.Email,
				Collections = collectionsDto,
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
