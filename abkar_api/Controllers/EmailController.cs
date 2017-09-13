using System.Net;
using System.Web.Http;
using System.Net.Mail;

namespace abkar_api.Controllers
{
    public class EmailController : ApiController
    {
        public static bool Send(string message, string to, string subject)
        {
            SmtpClient smtp = new SmtpClient("smtp.yandex.com.tr", 587);
            smtp.Credentials = new NetworkCredential("satinalma@abkar.com.tr", "Satinalma651!");
            smtp.EnableSsl = true;

            MailMessage MailMessage = new MailMessage();
            MailMessage.To.Add(new MailAddress(to));
            MailMessage.From = new MailAddress("satinalma@abkar.com.tr", "Abkar Satın Alma");
            MailMessage.Subject = subject;
            MailMessage.Body = message;
            MailMessage.IsBodyHtml = false;

            try
            {
                smtp.Send(MailMessage);
            }
            catch (SmtpException ex)
            {
                return false;
            }

            return true;

        }
    }
}
