namespace Collectors_Corner_Backend.Models.DTOs.Card
{
	public class CreateCardResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		public int CardId { get; set; }
		public string NativeImageUrl { get; set; }
		public string ThumbnailImageUrl { get; set; }
	}
}
