using Collectors_Corner_Backend.Models.DTOs.ImageService;
using Collectors_Corner_Backend.Models.Entities;
using Collectors_Corner_Backend.Models.Settings;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Http.Headers;
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
						fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
						form.Add(fileContent, "image", image.FileName);

						var response = await _httpClient.PostAsync(_serviceSettigns.GetImageUploadEndpoint(), form);
						var json = await response.Content.ReadAsStringAsync();

						var result = JsonSerializer.Deserialize<ImageUploadResponse>(json, new JsonSerializerOptions()
						{
							PropertyNameCaseInsensitive = true
						});

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
							NativeImageUrl = result.NativeImageUrl,
							ThumbnailImageUrl = result.ThumbnailImageUrl,
						};
					}
				}
			}
		}
	}
}
