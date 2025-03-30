namespace Collectors_Corner_Backend.Models.DTOs.ImageService
{
	public class ImageUploadResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		public string ImageNativeUrl { get; set; }
		public string ImageThumbnailUrl { get; set; }
	}
}
