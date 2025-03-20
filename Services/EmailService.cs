using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Collectors_Corner_Backend.Models;

namespace Collectors_Corner_Backend.Services
{
	public class EmailService
	{
		private readonly EmailSettings _settings;
		private readonly ILogger<EmailService> _logger;
		public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
		{
			_settings = settings.Value;
			_logger = logger;
		}
		public async Task SendAsync(string email, string subject, string body)
		{
			try
			{
				using (var smtpClient = new SmtpClient(_settings.SmtpServer, _settings.Port))
				{
					smtpClient.UseDefaultCredentials = false;
					smtpClient.Credentials = new NetworkCredential()
					{
						UserName = _settings.User,
						Password = _settings.Password,
					};
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtpClient.EnableSsl = true;
					using (var message = new MailMessage(_settings.User, email))
					{
						message.IsBodyHtml = true;
						message.Subject = subject;
						message.Body = body;
						await smtpClient.SendMailAsync(message);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("Error sending message: " + ex);
			}
		}
	}
}
