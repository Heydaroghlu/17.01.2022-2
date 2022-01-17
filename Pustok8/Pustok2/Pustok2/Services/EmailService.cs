using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html);
    }


    public class EmailService : IEmailService
    {
        public void Send(string to, string subject, string html)
        {
          

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("removeaccsoon@yandex.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.yandex.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("isaheyderoglu@yandex.com", "0513779384isa");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

