namespace Collectors_Corner_Backend.Models
{
	public class EmailSettings
	{
		public string User { get; set; } 
		public string Password { get; set; } 
		public string SmtpServer { get; set; } 
		public int Port { get; set; }
	}
}
