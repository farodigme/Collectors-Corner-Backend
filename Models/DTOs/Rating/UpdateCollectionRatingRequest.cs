using System.ComponentModel.DataAnnotations;

namespace Collectors_Corner_Backend.Models.DTOs.Rating
{
	public class UpdateCollectionRatingRequest
	{
		[Required]
		public int CollectionId { get; set; }

		[Required]
		[Range(minimum: 0.0, maximum: 5.0)]
		public decimal RatingValue { get; set; }
	}
}
