﻿using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Collection
{
	public class UpdateCollectionResponse : BaseResponse
	{
		public int collectionId { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public string Category { get; set; }
		public bool IsPublic { get; set; }
		public string? NativeImageUrl { get; set; }
		public string? ThumbnailImageUrl { get; set; }
	}
}
