using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OpenPop.Pop3;
using OpenPop.Mime;
using System.Data;
using bOS.Commons.Configuration;
using log4net;
using System.Configuration;

namespace bOS.Commons.Mail
{
    public class MailFactoryHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(MailFactoryHelper));

        public static String SUPPORT_SMTP_KEY = "smtp";

        public static Pop3Helper MakePop3Client(EmailElement email)
        {
            Pop3Helper pop3 = null;

            try
            {
                pop3 = new Pop3Helper(email.Server, email.Port, false, email.Username, email.Password);
                return pop3;
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }

            return null;
                
        }

        public static SmtpHelper MakeSmtpClient(EmailElement email)
        {
            SmtpHelper helper = null;

            try
            {
                helper = new SmtpHelper(email.Server, email.Port, email.Username, email.Password, false);
                return helper;
            }
            catch (Exception err)
            {
                logger.Error(err.Message);
            }

            return null;

        }

        public static void SendMail(String smtpKey, Email email, bool IsHtml)
        {
            EmailElement emailElement = ConfigurationHelper.GetEmailElement(smtpKey);
            SmtpHelper helper = MailFactoryHelper.MakeSmtpClient(emailElement);
            helper.Send(email, IsHtml);
        }
    }
}