using Collectors_Corner_Backend.Models.DTOs.Collection;
using Collectors_Corner_Backend.Models.Entities;

namespace ImageHosting.Extensions.Mappers
{
	public static class CollectionMapper
	{
		public static CollectionDto ToDto(Collection collections) => new()
		{
			Id = collections.Id,
			Title = collections.Title,
			Description = collections.Description,
			Category = collections.Category.Title,
			ImageUrl = collections.ImageUrl,
			IsPublic = collections.IsPublic,
			Rating = collections.Rating
		};
		public static List<CollectionDto> ToDtoList(IEnumerable<Collection> collections)
		{
			return collections.Select(ToDto).ToList();
		}
	}
}
