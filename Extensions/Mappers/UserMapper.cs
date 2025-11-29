using Collectors_Corner_Backend.Models.DTOs.Account;
using Collectors_Corner_Backend.Models.Entities;
using ImageHosting.Extensions.Mappers;

namespace Collectors_Corner_Backend.Extensions.Mappers
{
	public static class UserMapper
	{
		public static GetUserResponse ToGetUserResponse(User user, List<Collection> collections)
		{
			return new GetUserResponse
			{
				Success = true,
				Username = user.Username,
				Nickname = user.Nickname,
				AvatarUrl = user.AvatarUrl,
				Email = user.Email,
				Created = user.CreatedAt,
				Collections = CollectionMapper.ToDtoList(collections)
			};
		}
	}

}
