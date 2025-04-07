﻿using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Collection
{
	public class UpdateCollectionRequest
	{
		[Required]
		public int collectionId { get; set; }

		[Required]
		[Length(3, 50)]
		public string Title { get; set; }

		[Length(20, 200)]
		public string? Description { get; set; }

		[Required]
		[Length(5, 30)]
		public string Category { get; set; }

		[Required]
		public bool IsPublic { get; set; }

		[Required]
		public IFormFile Image { get; set; }
	}
}
