using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class MailModel
    {
        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        private List<string> AttachedFiles = new List<string>();

        /// <summary>
        /// Erstellt eine MimeMessage und gibt diese zurück.
        /// </summary>
        /// <returns></returns>
        public MimeMessage BuildMail()
        {
            MimeMessage mail = new MimeMessage();

            MailboxAddress adressSender = new MailboxAddress("", Sender);
            MailboxAddress adressReceiver = new MailboxAddress("", Receiver);

            mail.From.Add(adressSender);
            mail.To.Add(adressReceiver);

            mail.Subject = Subject;

            BodyBuilder body = new BodyBuilder();

            foreach(string files in AttachedFiles)
            {
                body.Attachments.Add(@"" + files);
            }

            body.TextBody = Message;

            mail.Body = body.ToMessageBody();

            return mail;
        }

        public void AddAttachments(string file)
        {
            if(file != null)
            {
                AttachedFiles.Add(file);
            }
            
        }
    }
}
