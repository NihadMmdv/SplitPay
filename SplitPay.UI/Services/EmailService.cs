﻿using SplitPay.UI.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Text;
using SplitPay.UI.Config;
using Microsoft.Extensions.Options;
using SplitPay.DAL.Models;

namespace SplitPay.UI.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;
		public EmailService(IOptions<EmailSettings> settings)
		{
			_emailSettings = settings.Value;
		}
		public async Task SendEmail(string url, string toEmail)
		{
			SmtpClient client = new SmtpClient(_emailSettings.Host, _emailSettings.Port);
			client.EnableSsl = true;
			client.UseDefaultCredentials = false;
			client.Credentials = new NetworkCredential(_emailSettings.FromMail, _emailSettings.Password);
			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(_emailSettings.FromMail);
			mailMessage.To.Add(toEmail);
			mailMessage.Subject = "Reset password";
			mailMessage.IsBodyHtml = true;
			StringBuilder mailBody = new StringBuilder();
			mailBody.AppendFormat("<br />");
			mailBody.AppendFormat($"<a href={url}>Reset Application Password</a>");
			mailMessage.Body = mailBody.ToString();
			await client.SendMailAsync(mailMessage);
		}
		public async Task SendPaymentEmail(string url, string toEmail, Loan loan)
		{
			SmtpClient client = new SmtpClient(_emailSettings.Host, _emailSettings.Port);
			client.EnableSsl = true;
			client.UseDefaultCredentials = false;
			client.Credentials = new NetworkCredential(_emailSettings.FromMail, _emailSettings.Password);
			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(_emailSettings.FromMail);
			mailMessage.To.Add(toEmail);
			mailMessage.Subject = "Loan Payment";
			mailMessage.IsBodyHtml = true;
			StringBuilder mailBody = new StringBuilder();
			mailBody.AppendFormat("<br />");
			mailBody.AppendFormat($"<p>Title:{loan.Title}</p>");
			mailBody.AppendFormat("<br />");
			mailBody.AppendFormat($"<p>Monthly price:{loan.MonthlyPrice}</p>");
			mailBody.AppendFormat("<br />");
			mailBody.AppendFormat($"<p>Loan take date{loan.CreatedDate}</p>");
			mailBody.AppendFormat("<br />");
			mailBody.AppendFormat($"a href={url}>Pay your loan</p>");
			mailMessage.Body = mailBody.ToString();
			await client.SendMailAsync(mailMessage);	
		}
	}
}
