using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace carpool_web_backend.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await sendFromOutLook(message);
        }

        private async Task sendFromOutLook(IdentityMessage message)
        {
            // Credentials:
            var credentialUserName = "carpool_team@outlook.com";
            var sentFrom = "carpool_team@outlook.com";
            var pwd = "Carpoolconestoga";

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new System.Net.Mail.MailMessage(sentFrom, message.Destination);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(message.Body, null, "text/html");

            mail.AlternateViews.Add(htmlView);
            mail.IsBodyHtml = true;

            mail.Subject = message.Subject;
            mail.Body = message.Body;

            // Send:
            try
            {
                await client.SendMailAsync(mail);

            }
            catch (Exception ex)
            {
                string sEx = ex.InnerException.Message;
            }
        }

    }
}