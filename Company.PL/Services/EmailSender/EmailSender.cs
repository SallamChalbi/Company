using Company.PL.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
//using System.Net.Mail;
using System.Threading.Tasks;

namespace Company.PL.Services.EmailSender
{
	public class EmailSender : IEmailSender
	{
		private readonly EmailSetting _options;
		//private readonly IConfiguration _configuration;

		public EmailSender(IOptions<EmailSetting> options/*, IConfiguration configuration*/)
        {
			_options = options.Value;
			//_configuration = configuration;
		}

        public void SendAsync(string from, string recipients, string subject, string body)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.SenderEmail),
                Subject = subject,
            };
            mail.To.Add(MailboxAddress.Parse(recipients));
            mail.From.Add(new MailboxAddress(_options.DisplayName, from));

            var builder = new BodyBuilder();
            builder.TextBody = body;
            mail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_options.SmtpClientServer, _options.SmtpClientPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.SenderEmail, _options.SenderPassword);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }

        //      public async Task SendAsync(string from, string recipients, string subject, string body)
        //{
        //	var emailMessage = new MailMessage();
        //	emailMessage.From = new MailAddress(from);
        //	emailMessage.To.Add(recipients);
        //	emailMessage.Subject = subject;
        //	emailMessage.Body = $"<html><body>{body}</body></html>";
        //	emailMessage.IsBodyHtml = true;

        //	var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpClientServer"], int.Parse(_configuration["EmailSettings:SmtpClientPort"]))
        //	{
        //		Credentials = new NetworkCredential(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderPassword"]),
        //		EnableSsl = true
        //	};

        //	await smtpClient.SendMailAsync(emailMessage);
        //}
    }
}
