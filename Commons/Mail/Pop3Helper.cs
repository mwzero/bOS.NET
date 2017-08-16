using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OpenPop.Pop3;
using OpenPop.Mime;
using System.Data;

namespace bOS.Commons.Mail
{
    public class Pop3Helper
    {
        Pop3Client pop3Client = new Pop3Client();

        public Pop3Helper(String server, int port, Boolean useSSL, String username, String password)
        {
            pop3Client.Connect(server, port, useSSL);
            pop3Client.Authenticate(username, password,AuthenticationMethod.UsernameAndPassword);
        }

        public void DeleteMessage(Email email)
        {
            pop3Client.DeleteMessage(email.MessageNumber);
        }

        public List<Email> ReadMessage()
        {
            List<Email> emails = new List<Email>();

            int count = pop3Client.GetMessageCount();

            for (int i = count; i >= 1; i--)
            {
                Message message = pop3Client.GetMessage(i);
                
                Email email = new Email()
                {
                    MessageNumber = i,
                    Subject = message.Headers.Subject,
                    DateSent = message.Headers.DateSent,
                    From = message.Headers.From.Address,
                    MessageId = message.Headers.MessageId
                };

                MessagePart body = message.FindFirstPlainTextVersion(); 
                if (body != null)
                {
                    email.Body = body.GetBodyAsText();
                }
                else
                {
                    body = message.FindFirstHtmlVersion();
                    if (body != null)
                    {
                        email.Body = body.GetBodyAsText();
                    }
                }

                List<MessagePart> attachments = message.FindAllAttachments();
                foreach (MessagePart attachment in attachments)
                {
                    email.Attachments.Add(new Attachment
                    {
                        FileName = attachment.FileName,
                        ContentType = attachment.ContentType.MediaType,
                        Content = attachment.Body
                    });
                }

                emails.Add(email);
            }

            return emails;
        }

        public void Disconnect()
        {
            pop3Client.Disconnect();
        }
    }
}