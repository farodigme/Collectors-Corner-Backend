namespace Collectors_Corner_Backend.Models.Entities
{
	public class Card
	{
		public int Id { get; set; }
		public int CollectionId { get; set; }
		public Collection? Collection { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public int CategoryId { get; set; }
		public CardCategory Category { get; set; }
		public string? ImageUrl { get; set; }
		public bool IsPublic { get; set; }
	}
}
