using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Graduation_Project.Application.Utils {

    public class EmailHandler : IEmailSender {
        private readonly SmtpSettings smtpSettings;

        public EmailHandler(IOptions<SmtpSettings> smtpSettings) {
            this.smtpSettings = smtpSettings.Value;
        }

        private async Task PrivateSendEmailAsync(string email, string subject, string htmlMessage) {
            //Create smtp client
            SmtpClient client = new SmtpClient(smtpSettings.SmtpServer, smtpSettings.SmtpPort);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential(smtpSettings.SmtpUsername, smtpSettings.SmtpPassword);

            //Prepare mail message
            MailMessage message = new MailMessage {
                From = new MailAddress(smtpSettings.FromAddress, smtpSettings.FromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(email);
            await client.SendMailAsync(message);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage) {
            await PrivateSendEmailAsync(email, subject, htmlMessage);
        }
    }
}