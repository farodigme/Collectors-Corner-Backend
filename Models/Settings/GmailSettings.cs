namespace VivaWebSite.Models
{
	public class GmailSettings
	{
		public string? User { get; set; } = Environment.GetEnvironmentVariable("GmailUser");
		public string? Password { get; set; } = Environment.GetEnvironmentVariable("GmailPassword");
		public string? SmtpServer { get; set; } = Environment.GetEnvironmentVariable("GmailSmtp");
		public int Port { get; set; } = Convert.ToInt32(Environment.GetEnvironmentVariable("GmailSmtpPort"));
	}
}
