using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivingRebooking
{
    public static class Notifier
    {
        public static async Task SendNotification(string subject, string emailContent)
        {
            Console.WriteLine("sending email");

            string apiKey = ConfigurationManager.AppSettings["SENDGRID_KEY"];

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email("andrei.mihalciuc@gmail.com");
            Email to = new Email("andrei.mihalciuc@gmail.com");
            Content content = new Content("text/plain", emailContent);
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}
 