namespace Collectors_Corner_Backend.Models.DTOs.Collection
{
	public class CreateCollectionResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		public int CollectionId { get; set; }
		public string NativeImageUrl { get; set; }
		public string ThumbnailImageUrl { get; set; }
	}
}
