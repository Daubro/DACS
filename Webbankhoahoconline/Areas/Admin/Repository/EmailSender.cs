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
                Credentials = new NetworkCredential("dxk1708@gmail.com", "bicchfeiogstmkcp")
            };

            return client.SendMailAsync(
                new MailMessage(from: "dxk1708@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
