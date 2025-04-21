namespace Collectors_Corner_Backend.Models.DTOs.Card
{
	public class GetCardsByCollectionResponse : BaseResponse
	{
		public List<CardDto>? Cards { get; set; }
	}
}
