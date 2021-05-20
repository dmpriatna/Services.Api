using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace OpticalCharacterRecognition.Api.Services
{
    public class EmailService
    {
        public EmailService()
        {
            Agent = new SmtpClient("smtp.gmail.com", 587);
            Agent.DeliveryMethod = SmtpDeliveryMethod.Network;
            Agent.EnableSsl = true;
            Agent.UseDefaultCredentials = false;
            Agent.Credentials = new NetworkCredential("apps.info@go-logs.com", "oYP24[zf=I2O");
        }

        private SmtpClient Agent { get; }

        public void Send(List<string> to, string subject, string body)
        {
            var sender = new MailAddress("noreply@go-logs.com", "go-logs");
            var message = new MailMessage();

            foreach (var receiver in to)
            {
                message.To.Add(receiver);
            }

            message.From = sender;
            message.Subject = subject;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            try
            {
                Agent.Send(message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
