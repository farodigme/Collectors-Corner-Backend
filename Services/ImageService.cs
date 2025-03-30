using Collectors_Corner_Backend.Models.DTOs.ImageService;
using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Models.Settings;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

namespace Collectors_Corner_Backend.Services
{
	public class ImageService
	{
		private ApplicationContext _context;
		private HttpClient _httpClient;
		private ImageServiceSettings _serviceSettigns;

		public ImageService(ApplicationContext context, HttpClient httpClient, IOptions<ImageServiceSettings> serviceSettigns)
		{
			_context = context;
			_httpClient = httpClient;
			_serviceSettigns = serviceSettigns.Value;
		}

		public async Task<ImageUploadResponse> UploadImageAsync(IFormFile image)
		{
			using (_httpClient)
			{
				using (var form = new MultipartFormDataContent())
				{
					await using (var stream = image.OpenReadStream())
					{
						var fileContent = new StreamContent(stream);
						form.Add(fileContent, "image", image.FileName);
						Debug.WriteLine("PATH: " + _serviceSettigns.GetImageUploadEndpoint());
						var response = await _httpClient.PostAsync(_serviceSettigns.GetImageUploadEndpoint(), form);
						var json = await response.Content.ReadAsStringAsync();

						var result = JsonSerializer.Deserialize<ImageUploadResponse>(json);
						if (result == null)
						{
							return new ImageUploadResponse()
							{
								Success = false,
								Error = "Invalid answer from image service"
							};
						}

						if (!result.Success)
						{
							return new ImageUploadResponse()
							{
								Success = false,
								Error = result.Error
							};
						}

						return new ImageUploadResponse()
						{
							Success = true,
							ImageNativeUrl = result.ImageNativeUrl,
							ImageThumbnailUrl = result.ImageThumbnailUrl,
						};
					}
				}
			}
		}
	}
}
