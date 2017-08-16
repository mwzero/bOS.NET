using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bOS.Commons.Mail
{
    [Serializable]
    public class Email
    {
        public Email()
        {
            this.Attachments = new List<Attachment>();
        }
        public String MessageId { get; set; }
        public int MessageNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public DateTime DateSent { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    [Serializable]
    public class Attachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}