using System.Net;
using System.Net.Mail;

namespace Assignment_01
{    
    public class EmailEventArgs : EventArgs 
    {
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; } 
        public string Body { get; set; }
    }
    public class EmailExtendingEventArgs 
    {
        // Define the event using the EventHandler with the EmailExtendingEventArgs type
        private event EventHandler<EmailEventArgs> OnSend;

        public static void EmailEvent(string emailFrom, string emailTo, string subject, string body)
        {
            var mailService = new EmailExtendingEventArgs();

            mailService.OnSend += MailService.SendMail;

            mailService.CreateEmail(emailFrom, emailTo, subject, body);
        }

        private void CreateEmail(string emailFrom, string emailTo, string subject, string body)
        {
            // Raising an Event
            if (OnSend != null)
            {
                OnSend(this, new EmailEventArgs 
                {
                    EmailFrom = emailFrom,
                    EmailTo = emailTo,
                    Subject = subject,
                    Body = body
                });
            }
        }
    }

    public class MailService
    {               
        public static void SendMail(object sender, EmailEventArgs e)
        {       
            var smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("crc933619@gmail.com", "xosvgrxwessduajt")                
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(e.EmailFrom),
                Subject = e.Subject,
                Body = e.Body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(e.EmailTo);

            smtpClient.Send(mailMessage);

            Console.WriteLine("Email Sent!");
        }
    }
}
