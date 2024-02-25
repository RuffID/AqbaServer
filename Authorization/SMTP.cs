using AqbaServer.Helper;
using System.Net;
using System.Net.Mail;

namespace AqbaServer.Authorization
{
    public class SMTP
    {
        public static MailMessage? CreateMail(string name, string emailFrom, string emailTo, string subject, string body)
        {
            try
            {
                var from = new MailAddress(emailFrom, name);
                var to = new MailAddress(emailTo);
                var mail = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                return mail;
            }
            catch (Exception e)
            {
                WriteLog.Error(e.ToString());
                return null;
            }
        }

        public static bool SendMail(string host, int smtpPort, string emailFrom, string pass, MailMessage? mail)
        {
            if (mail == null) return false;
            try
            {
                using SmtpClient smtp = new(host, smtpPort);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(emailFrom, pass);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                smtp.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                WriteLog.Warn(e.ToString());
                return false;
            }
        }
    }
}
