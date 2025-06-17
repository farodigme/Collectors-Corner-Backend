namespace Collectors_Corner_Backend.Models.Entities
{
	public class Collection
	{
		public int Id { get; set; }
		public int UserId {  get; set; }
		public User? User { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public int CategoryId { get; set; }
		public CollectionCategory Category { get; set; }
		public string? ImageUrl { get; set; }
		public bool IsPublic { get; set; }
		public decimal Rating { get; set; }
		public int Views {  get; set; }
	}
}
