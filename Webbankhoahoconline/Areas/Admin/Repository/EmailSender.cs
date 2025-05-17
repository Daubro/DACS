using System.Net.Mail;
using System.Net;

namespace Webbankhoahoconline.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("khanh1652554@gmail.com", "lmdvvqwybrubrtrd")
            };

            return client.SendMailAsync(
                new MailMessage(from: "khanh1652554@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
