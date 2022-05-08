using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Mail_Receive 
    {

        // Eigenschaften

        private ObservableCollection<MimeMessage> mailList;
        public ObservableCollection<MimeMessage> MailList
        {
            get
            {
                if (mailList == null)
                {
                    mailList = new ObservableCollection<MimeMessage>();
                }

                return mailList;
            }

            set
            {
                if (mailList != value)
                {
                    mailList = value;
                }
            }
        }

        /*
         * Instanziierung
         */

        private static readonly Lazy<Mail_Receive> lazy =
        new Lazy<Mail_Receive>(() => new Mail_Receive());

        

        public static Mail_Receive MailReceive { get { return lazy.Value; } }


        private Mail_Receive()
        {

        }

        public List<MimeMessage> ReadMessagesFromServer(ImapClient client)
        {
            List<MimeMessage> list = new List<MimeMessage>();
            string dateMail;

            client.Inbox.Open(FolderAccess.ReadOnly);

            foreach (UniqueId id in client.Inbox.Search(SearchQuery.DeliveredOn(System.DateTime.Today)))
            {
                // Datetime prüfen und darauf prüfen
                DateTime date = client.Inbox.GetMessage(id).Date.LocalDateTime;

                dateMail = Mails.CutString(Convert.ToString(date));

                string today = Mails.CutString(Convert.ToString(System.DateTime.Today));

                if (dateMail == today)
                {
                    list.Add(client.Inbox.GetMessage(id));
                }
            }

            return list;

        }
    }
}
