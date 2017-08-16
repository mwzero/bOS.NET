using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using bOS.Commons.Configuration;

namespace bOS.Commons.Mail
{
    [TestClass]
    public class MailTest
    {
        [TestMethod]
        public void Smtp()
        {
            EmailElement emailElement = ConfigurationHelper.GetEmailElement("smtp");
            SmtpHelper helper = MailFactoryHelper.MakeSmtpClient(emailElement);

            Email mail = new Email();
            mail.Body = "Test smtp";
            mail.From = String.Empty;
            mail.To = "maurizio.farina@gmail.com";
            mail.Subject = "Test";
            helper.Send(mail, false);
       }
    }
}
