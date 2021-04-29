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
            //Agent = new SmtpClient("smtp-relay.sendinblue.com", 587);
            //Agent.DeliveryMethod = SmtpDeliveryMethod.Network;
            //Agent.EnableSsl = true;
            //Agent.UseDefaultCredentials = false;
            //Agent.Credentials = new NetworkCredential("dedemaulanapriatna@gmail.com", "BYrqA2RwG3ZTCdDy");
            Agent = new SmtpClient("smtp.gmail.com", 587);
            Agent.DeliveryMethod = SmtpDeliveryMethod.Network;
            Agent.EnableSsl = true;
            Agent.UseDefaultCredentials = false;
            Agent.Credentials = new NetworkCredential("zain.hernadi@fellow.lpkia.ac.id", "zain261914");
        }

        private SmtpClient Agent { get; }

        public void Send(List<string> to, string subject, string body)
        {
            var sender = new MailAddress("noreply@go-logs.com", "go-logs");
            //var receiver = new MailAddress(to);
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
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}
