namespace Collectors_Corner_Backend.Models.DTOs.Account
{
	public class GetUserResponse
	{
		public bool Success { get; set; }
		public string? Error { get; set; }
		public string Username { get; set; }
		public string? Nickname { get; set; }
		public string Email { get; set; }
		public DateTime? Created { get; set; }
	}
}
