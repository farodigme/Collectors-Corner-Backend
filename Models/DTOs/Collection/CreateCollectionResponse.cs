namespace Collectors_Corner_Backend.Models.DTOs.Collection
{
	public class CreateCollectionResponse : BaseResponse
	{
		public int CollectionId { get; set; }
		public string NativeImageUrl { get; set; }
		public string ThumbnailImageUrl { get; set; }
	}
}
