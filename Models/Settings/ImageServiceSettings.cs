namespace Collectors_Corner_Backend.Models.Settings
{
	public class ImageServiceSettings
	{
		public string BaseUrl { get; set; }
		public string UploadEndpoint { get; set; }
		public string GetEndpoint { get; set; }

		public string GetImageUploadEndpoint() => string.Concat(BaseUrl, UploadEndpoint);
		public string GetImageGetEndpoint() => string.Concat(BaseUrl, GetEndpoint);

	}
}
