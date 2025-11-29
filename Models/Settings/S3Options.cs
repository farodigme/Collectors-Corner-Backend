namespace Collectors_Corner_Backend.Models.Settings
{
	public class S3Options
	{
		public string AWS_BUCKET_NAME { get; set; } = null!;
		public string AWS_ACCESS_KEY {  get; set; } = null!;
		public string AWS_SECRET_KEY { get;set; } = null!;
		public string AWS_SERVICE_URL { get; set; } = null!;
	}
}
