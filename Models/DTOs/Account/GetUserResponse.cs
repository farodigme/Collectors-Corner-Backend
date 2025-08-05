using Collectors_Corner_Backend.Models.DTOs.Collection;

namespace Collectors_Corner_Backend.Models.DTOs.Account
{
	public class GetUserResponse : BaseResponse
	{
		public string Username { get; set; }
		public string? Nickname { get; set; }
		public string Email { get; set; } 
		public string? AvatarUrl { get; set; } 
		public List<CollectionDto> Collections { get; set; }
		public DateTime? Created { get; set; }
	}
}
