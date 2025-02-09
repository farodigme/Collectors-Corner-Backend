namespace CardCollectionApi.Models
{
	public class Storage
	{
		public int Id { get; set; }
		public int CardsId { get; set; }
		public Cards? Cards { get; set; }

		public int CollectionsId { get; set; }
		public Collections? Collections { get; set; }

		public int UsersId { get; set; }
		public Users? Users { get; set; }
	}
}
