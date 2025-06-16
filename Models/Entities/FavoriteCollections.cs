namespace Collectors_Corner_Backend.Models.Entities
{
	public class FavoriteCollections
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User? User { get; set; }
		public string? CollectionsJson { get; set; }
	}
}
