namespace Collectors_Corner_Backend.Models.Entities
{
	public class Rating
	{
		public int Id { get; set; }
		public int CollectionId { get; set; }
		public Collection? Collection { get; set; }
		public int UserId { get; set; }
		public User? User { get; set; }
		public decimal StarCount { get; set; }
	}
}
