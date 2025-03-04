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
		public async Task Send(OrderInfo order)
		{
			string body = "<head><h1>Запрос с сайта.</h1></head>" +
			"<body>" +
			"<h2>Обьект заказа:</h2>" + Environment.NewLine +
			$"<p>Наименование: {order.Title} </p>" + Environment.NewLine +
			$"<p>OEM: {order.Oem} </p>" + Environment.NewLine +
			$"<p>Цена: {order.Price} RUB</p>" + Environment.NewLine +
			$"<p>Имя пользователя: {order.Name}</p>" + Environment.NewLine +
			$"<p>Фамилия пользователя: {order.Surname}</p>" + Environment.NewLine +
			$"<p>Email пользователя: {order.Email}</p>" + Environment.NewLine +
			$"<p>Телефон пользователя: {order.Phone}</p>" + Environment.NewLine +
			$"<p>Комментарий пользователя: {order.Comment}</p>" + Environment.NewLine +
			"</body>";
			
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
