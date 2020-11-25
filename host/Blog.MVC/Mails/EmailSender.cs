using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;
using Blog.MVC.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Blog.MVC.Mails
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _options;

        public EmailSender(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.Username, "825646490@qq.com"));
            message.To.Add(new MailboxAddress(email.Split('@')[0], email));

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };

            await client.ConnectAsync(_options.Host, _options.Port, false);

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            // Note: only needed if the SMTP server requires authentication
            await client.AuthenticateAsync(_options.EmailAddress, _options.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}