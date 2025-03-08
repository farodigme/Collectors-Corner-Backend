using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using VivaWebSite.Models;

namespace VivaWebSite.Utils
{
	public class GmailService
	{
		private readonly GmailSettings _settings;
		private readonly ILogger<GmailService> _logger;
		public GmailService(IOptions<GmailSettings> settings, ILogger<GmailService> logger)
		{
			_settings = settings.Value;
			_logger = logger;
		}
		public async Task Send()
		{
			string body = "some message";
			
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
					using (var message = new MailMessage(_settings.User, _settings.User))
					{
						message.IsBodyHtml = true;
						message.Body = body;
						message.Subject = "Запрос с сайта";
						await smtpClient.SendMailAsync(message);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError("Ошибка отправки запроса Gmail: " + ex);
			}
		}
	}
}
