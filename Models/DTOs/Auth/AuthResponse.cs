﻿namespace Collectors_Corner_Backend.Models.DTOs.Auth
{
	public class AuthResponse : BaseResponse
	{
		public string? AccessToken { get; set; }
		public DateTime? AccessTokenExpires { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpires { get; set; }
	}
}
