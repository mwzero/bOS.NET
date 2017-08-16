using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using log4net;
using System.IO;

namespace bOS.Commons.Mail
{
    public class SmtpHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(SmtpHelper));

        SmtpClient smtp = new SmtpClient();

        String from = String.Empty;

        public SmtpHelper(String host, int port, String user, String password, Boolean enableSSL)
        {
            smtp.Host = host;
            if ( port != 0 ) smtp.Port = port;

            from = user;

            if ( !String.IsNullOrEmpty(password) )
                smtp.Credentials = new System.Net.NetworkCredential(user, password);

            smtp.EnableSsl = enableSSL;
        }

        public Boolean Send(Email email, Boolean IsHtml)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.To.Add(email.To);
                    message.Subject = email.Subject;
                    if (String.IsNullOrEmpty(email.From))
                    {
                        message.From = new System.Net.Mail.MailAddress(from);
                    }
                    else
                    {
                        message.From = new System.Net.Mail.MailAddress(email.From);
                    }

                    message.Body = email.Body;
                    message.IsBodyHtml = IsHtml;

                    List<Attachment> attachments = email.Attachments;
                    foreach (Attachment attachment in attachments)
                    {
                        System.Net.Mail.Attachment toAttach = new System.Net.Mail.Attachment(new MemoryStream(attachment.Content), attachment.FileName);
                        message.Attachments.Add(toAttach);
                    }

                    smtp.Send(message);
                }
            }
            catch (Exception err)
            {
                logger.Error("Impossible to send a message", err);
                return false;
            }
            return true;
        }
    }
}