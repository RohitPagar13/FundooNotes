using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Crypto.Macs;

namespace RabbitMQConsumer.Utilities
{
    public class EmailSender
    {

        public static void SendEmail(EmailModel emailModel)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("rohitpagar133@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailModel.To));
            email.Subject = emailModel.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailModel.Body
            };


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            string pass = Environment.GetEnvironmentVariable("fundoo");
            if( pass != null)
            {
                smtp.Authenticate("rohitpagar133@gmail.com", pass);
                smtp.Send(email);
            }
            smtp.Disconnect(true);
        }
    }
}