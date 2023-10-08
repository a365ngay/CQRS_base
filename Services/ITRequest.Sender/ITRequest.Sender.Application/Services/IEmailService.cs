using System.Net;
using System.Net.Mail;
using ITRequest.Sender.Domain.Models.EntityModels;
using ITRequest.Sender.Domain.ValueSettings;

namespace ITRequest.Sender.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailModel message);
    }

    public class EmailService : IEmailService
    {
        private readonly AppSetting _appSetting;

        public EmailService(AppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        public async Task SendEmailAsync(SendEmailModel message)
        {
            ArgumentNullException.ThrowIfNull(message);
            using (var email = new MailMessage())
            {
                email.From = new MailAddress(_appSetting?.Smtp?.From ?? string.Empty);
                email.Subject = message.Subject;
                if (message.ToEmails == null)
                {
                    return;
                }
                foreach (var item in message.ToEmails)
                {
                    email.To.Add(new MailAddress(item));
                }
                if (message.BccEmails != null)
                {
                    foreach (var item in message.BccEmails)
                    {
                        email.To.Add(new MailAddress(item));
                    }
                }
                if (message.CcEmails != null)
                {
                    foreach (var item in message.CcEmails)
                    {
                        email.To.Add(new MailAddress(item));
                    }
                }

                email.Body = message.Content;
                email.IsBodyHtml = true;
                using (var client = new SmtpClient(_appSetting?.Smtp?.SmtpServer))
                {
                    client.UseDefaultCredentials = false;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Port = _appSetting?.Smtp?.Port ?? default;
                    client.Credentials = new NetworkCredential(_appSetting?.Smtp?.Username, _appSetting?.Smtp?.Password);
                    client.EnableSsl = true;
                    await client.SendMailAsync(email);
                }
                return;
            }
        }
    }
}
