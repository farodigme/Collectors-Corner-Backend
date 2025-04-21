namespace Collectors_Corner_Backend.Models.DTOs.Card
{
	public class CardDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public string Category { get; set; }
		public string? ImageUrl { get; set; }
		public bool IsPublic { get; set; }
	}
}
