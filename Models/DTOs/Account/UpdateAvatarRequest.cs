using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Account
{
	public class UpdateAvatarRequest
	{
		[Required]
		public IFormFile Image {  get; set; }
	}
}
