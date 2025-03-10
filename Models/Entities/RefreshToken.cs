using Collectors_Corner_Backend.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectors_Corner_Backend.Models.Entities
{
	public class RefreshToken
	{
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User? User { get; set; }

		[Required]
		public string? Token { get; set; }

		[Required]
		public DateTime ExpiresAt { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
	}
}
