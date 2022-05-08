using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    public class Mails
    {
        public string SenderName { get; set; }

        public string SenderMail { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverMail { get; set; }

        public string MailSubject { get; set; }

        public string MailText { get; set; }

        private List<KeyVal<string, string>> list = new List<KeyVal<string, string>>();

        public static Mails Parse(MimeMessage message)
        {
            throw new NotImplementedException();
        }

        /*
            VM -> var mail = RecieveMailService.ReadMessagesFromServer()
            VM -> Mails.Parse(mail)
            VM -> var saveResult = DatabaseSercice.SaveMail(mail);
            VM -> FileService.Save(mail, saveResult.Id)
         */

        public void GetandSaveMails(ObservableCollection<MimeMessage> mails)
        {
            InternetAddressList receivers;
            InternetAddressList sender;

            string strActualDate = Convert.ToString(DateTime.Today);
            string actualDate = CutString(Convert.ToString(DateTime.Today));

            foreach (MimeMessage message in mails)
            {
                string filename = null;
                int id = 0;

                receivers = message.To;
                sender = message.From;
                MailSubject = message.Subject;
                MailText = message.TextBody;

                string mailDate = CutString(Convert.ToString(message.Date.LocalDateTime));

                foreach (MailboxAddress address in receivers)
                {
                    ReceiverMail = address.Address;
                }

                foreach (MailboxAddress address in sender)
                {
                    SenderMail = address.Address;
                }

                if (Database.CheckMails())
                {
                    // zum Start, wenn Tabelle leer ist. 
                    // als prozedur wegen Scope Idendity, um die Datei danach zu benennen

                    // Notlösung -> Muss noch umgebaut werden
                    list = Database.CommandReader(actualDate);

                    KeyVal<string, string> keyFound = list.Find(keyVal => keyVal.id == MailSubject && keyVal.value == actualDate);

                    if (keyFound == null)
                    {
                        Database.InsertQuery(SenderMail, ReceiverMail, MailSubject, mailDate);
                        id = Convert.ToInt32(Database.MessageInput);
                        filename = Convert.ToString(id);

                        SaveMessages(MailText, filename, mailDate);
                    }

                    list.Clear();

                   
                }
                else
                {
                    list = Database.CommandReader(actualDate);

                    /*
                     * Für jedes Keyval soll geprüft werden,
                     * ob der Betreff schon vorhanden ist
                     */

                    KeyVal<string,string> keyFound = list.Find(keyVal => keyVal.id == MailSubject && keyVal.value == actualDate);

                    if(keyFound == null)
                    {
                        Database.InsertQuery(SenderMail, ReceiverMail, MailSubject, mailDate);
                        id = Convert.ToInt32(Database.MessageInput);
                        filename = Convert.ToString(id);

                        SaveMessages(MailText, filename, mailDate);
                    }

                    list.Clear();

                }
            }
        }

        public static string CutString(string dateString)
        {
            string newDateString = dateString.Substring(0, 11);

            return newDateString;
        }

        private void SaveMessages(string message, string filename, string datum)
        {
            // Orte speichern in seperate Klasse auslagern
            string pathfolder = @"c:\Mailprogramm2";
            DirectoryInfo di = new DirectoryInfo(pathfolder);
            JsonSerializer serializer = new JsonSerializer();
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Nachricht");
                writer.WriteValue(message);
                writer.WritePropertyName("Datum");
                writer.WriteValue(datum);
                writer.WriteEndObject();
            }

            string path = @"c:\Mailprogramm2\" + filename + ".txt";

            if (di.Exists)
            {
                //do nothing because in both ways he should safe the file
            }
            else
            {
                di.Create();
            }

            using (StreamWriter sW = File.AppendText(path))
            {

                sW.WriteLine(sb);

            }
        }
    }
}
