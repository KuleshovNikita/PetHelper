using PetHelper.Domain;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using PetHelper.Domain.Properties;

namespace PetHelper.Business.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _emailConfig;

        public EmailService(IConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailConfirmMessage(UserModel userModel)
        {
            var confirmUrl = BuildConfirmUrl(userModel);
            var message = BuildEmailMessage(confirmUrl);
            await SendEmail(message, userModel.Login);
        }

        private string BuildConfirmUrl(UserModel userModel)
        {
            var serverUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(";").First();

            var encodedHashKey = Uri.EscapeDataString(userModel.Password);
            var confirmEndpoint = $"/api/authentication/confirmEmail/{encodedHashKey}";

            return serverUrl + confirmEndpoint;
        }

        private string BuildEmailMessage(string confirmUrl)
        {
            var messageTemplate = Resources.EmailMessageTemplate;
            var messageWithLink = string.Format(messageTemplate, confirmUrl);

            return messageWithLink;
        }

        private async Task SendEmail(string message, string consumer)
        {
            var host = _emailConfig.GetSection("EmailBotData:Host").Value;
            var sender = _emailConfig.GetSection("EmailBotData:BotMail").Value;
            var password = _emailConfig.GetSection("EmailBotData:Password").Value;

            var client = new SmtpClient
            {
                Port = 587,
                Host = host,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, password)
            };

            await client.SendMailAsync(sender, consumer, Resources.ConfirmYourEmail, message);
        }
    }
}
