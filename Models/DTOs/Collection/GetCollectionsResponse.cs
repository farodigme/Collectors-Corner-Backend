using Collectors_Corner_Backend.Models.Entities;

namespace Collectors_Corner_Backend.Models.DTOs.Collection
{
	public class GetCollectionsResponse : BaseResponse
	{
		public List<CollectionDto>? Collections { get; set; }
	}
}
