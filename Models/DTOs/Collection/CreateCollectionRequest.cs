using Collectors_Corner_Backend.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Collectors_Corner_Backend.Models.DTOs.Collection
{
	public class CreateCollectionRequest
	{
		[Required]
		[MaxLength(50)]
		public string Title { get; set; }
		
		[MaxLength(200)]		
		public string? Description { get; set; }

		[Required]
		[MaxLength(30)]
		public string Category { get; set; }

		[Required]
		public IFormFile Image { get; set; }
	}
}
