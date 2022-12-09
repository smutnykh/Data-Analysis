using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace TravelnessAPI.Managers
{
    public class EmailManager
    {
        private readonly IConfiguration configuration;

        public EmailManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void SendEmail(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Confirm your registration", configuration["EmailForForSendingLetters:Address"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.Auto);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(configuration["EmailForForSendingLetters:Address"], configuration["EmailForForSendingLetters:Password"]);
                client.Send(emailMessage);

                client.Disconnect(true);
            }
        }
    }
}
