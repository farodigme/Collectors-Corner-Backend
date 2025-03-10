using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string? Nickname { get; set; }

		[Required]
		[MaxLength(50)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		[MaxLength(100)]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public RefreshToken? RefreshToken { get; set; }

	}
}
