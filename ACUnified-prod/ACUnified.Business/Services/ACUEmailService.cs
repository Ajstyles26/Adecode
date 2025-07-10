using ACUnified.Business.Services.IServices;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MimeKit;
using System.Data.Entity;
using System.Net.Mail;

namespace ACUnified.Portal.Data
{
    public class ACUEmailService : IACUEmailSender
    {
        private readonly IConfiguration _configuration;
        public ACUEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlmessage)
        {
            // Implement your email sending logic here
            //write to a log and send email
            var host = _configuration.GetSection("Mail")["MailHost"];
            var username = _configuration.GetSection("Mail")["MailKey"];
            var password = _configuration.GetSection("Mail")["MailPassword"];
            try
            {
                var base64Image = "";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ACU", "noreply@acu.edu.ng"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                var htmlEmail = new BodyBuilder();
                htmlEmail.HtmlBody = htmlmessage;
                message.Body = htmlEmail.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(host, 25, false);
                    client.Authenticate(username, password);

                    client.Send(message);
                    client.Disconnect(true);
                    await Console.Out.WriteLineAsync("Mail Successfully Sent");
                };
            }

            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("E-Mail was not Successfully Sent");

            }
           


            
            // return Task.CompletedTask;
        }

        public async Task SendEmailWithAttachmentAsync(string email, string subject, string htmlmessage, byte[] bytes)
        {
            // Implement your email sending logic here
            //write to a log and send email

            var host = _configuration.GetSection("Mail")["MailHost"];
            var username = _configuration.GetSection("Mail")["MailKey"];
            var password = _configuration.GetSection("Mail")["MailPassword"];

            var base64Image = "";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ACU", "noreply@acu.edu.ng"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            var htmlEmail = new BodyBuilder();
            var attachment = htmlEmail.Attachments.Add("Document.pdf", new MemoryStream(bytes));
            
            htmlEmail.HtmlBody = htmlmessage;
            message.Body = htmlEmail.ToMessageBody();
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(host, 25, false);
                    client.Authenticate(username, password);

                    client.Send(message);
                    client.Disconnect(true);
                    await Console.Out.WriteLineAsync("Mail Successfully Sent");
                }
            }
            catch (Exception)
            {

                await Console.Out.WriteLineAsync("E-Mail was not Successfully Sent");
            }
           
        }
    }
}