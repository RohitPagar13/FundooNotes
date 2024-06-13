using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using ModelLayer;
using RepositoryLayer.Utilities.emailModel;

namespace RepositoryLayer.Utilities
{
    public class EmailSender
    {

        public static void SendEmail(EmailModel emailModel, IConfiguration config)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(config.GetSection("EmailSettings:EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(emailModel.To));
            email.Subject = emailModel.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailModel.Body
            };


            using var smtp = new SmtpClient();
            smtp.Connect(config.GetSection("EmailSettings:EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("EmailSettings:EmailUsername").Value, Environment.GetEnvironmentVariable("emailpass(rohitpagar133@gmail.com)"));
            smtp.Send(email);
            smtp.Disconnect(true);
            smtp.Dispose();
        }
    }
}