using Collectors_Corner_Backend.Models.Entities;

namespace Collectors_Corner_Backend.Models.DTOs.Collection
{
	public class GetCollectionsResponse
	{
		public bool Success { get; set; }
		public string? Error { get; set; }
		
		public List<CollectionDto>? Collections { get; set; }
	}
}
