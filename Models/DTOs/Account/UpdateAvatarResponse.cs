namespace Collectors_Corner_Backend.Models.DTOs.Account
{
	public class UpdateAvatarResponse : BaseResponse
	{
		public string NativeImageUrl { get; set; }
		public string ThumbnailImageUrl { get; set; }
	}
}
