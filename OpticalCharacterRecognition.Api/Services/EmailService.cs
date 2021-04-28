using System;
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
            Agent.Credentials = new NetworkCredential("f3124ri@gmail.com", "w0lverin3");
            Agent.EnableSsl = true;
        }

        private SmtpClient Agent { get; }

        public void Send(string to, string subject, string body)
        {
            var sender = new MailAddress("noreply@go-logs.com", "go-logs");
            var receiver = new MailAddress(to);
            var message = new MailMessage(sender, receiver)
            {
                Subject = subject,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

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
