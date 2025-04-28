using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.Entities
{
	public class User
	{
		public int Id { get; set; }

		[MaxLength(16)]
		public string? Nickname { get; set; }

		[Required]
		[MaxLength(16)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		[MaxLength(32)]
		public string Email { get; set; }

		[Required]
		[MaxLength(512)]
		public string PasswordHash { get; set; }
		public string? AvatarUrl { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		
		[Required]
		[MaxLength(64)]
		public RefreshToken RefreshToken { get; set; }

		[MaxLength(64)]
		public ResetToken? ResetToken { get; set; }
	}
}
