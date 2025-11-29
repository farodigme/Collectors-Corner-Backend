using Collectors_Corner_Backend.Models.Settings;

namespace Collectors_Corner_Backend.Services
{
	public class S3Service
	{
		private S3Options _options;
		private IAmazonS3 _s3;
		public S3Service(S3Options s3Options, IAmazonS3 s3)
		{
			_options = s3Options;
			_s3 = s3;
		}
	}
}
