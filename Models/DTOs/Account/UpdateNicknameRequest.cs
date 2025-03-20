using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Account
{
	public class UpdateNicknameRequest
	{
		[Required]
		[MaxLength(16)]
		public string Nickname { get; set; }
	}
}
