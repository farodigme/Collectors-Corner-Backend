namespace Collectors_Corner_Backend.Models.DTOs.Card
{
	public class CreateCardResponse : BaseResponse
	{
		public int CardId { get; set; }
		public string NativeImageUrl { get; set; }
		public string ThumbnailImageUrl { get; set; }
	}
}
