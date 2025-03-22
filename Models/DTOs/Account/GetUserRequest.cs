using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Account
{
	public class GetUserRequest
	{
		[Required]
		[Length(8,16)]
		public string Username {  get; set; }
	}
}
