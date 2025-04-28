using Collectors_Corner_Backend.Models.DTOs.Card;
using Collectors_Corner_Backend.Models.Entities;

namespace Collectors_Corner_Backend.Extensions.Mappers
{
	public class CardMapper
	{
		public static CardDto ToDto(Card card) => new()
		{
			Id = card.Id,
			Title = card.Title,
			Description = card.Description,
			Category = card.Category.Title,
			ImageUrl = card.ImageUrl,
			IsPublic = card.IsPublic
		};
		public static List<CardDto> ToDtoList(IEnumerable<Card> cards)
		{
			return cards.Select(ToDto).ToList();
		}
	}
}
