using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public static MainWindowViewModel mainwindowviewmodel;

        public MainWindowViewModel()
        {
            Mails = new ObservableCollection<Element>();
            LoadOrUpdateList();

            mainwindowviewmodel = this;

            this.ReceiveCommand = new SendMailCommand(
                (o) =>
                {
                    Mails mails = new Mails();
                    MailListHelper.UpdateMailList(MainWindow.service, MainWindowViewModel.mainwindowviewmodel);
                },
                (o) => true
                );
            this.OpenSendWindow = new SendMailCommand(

                (o) =>
                {
                    SendMails sendMail = new SendMails();
                    sendMail.Show();
                },
                (o) => true
                );

        }

        private ObservableCollection<Element> mails = new ObservableCollection<Element>();
        public ObservableCollection<Element> Mails
        {
            get => mails;
            set
            {
                if (mails != value)
                {
                    mails = value;
                    OnPropertyChanged();
                }
            }
        }
        public SendMailCommand ReceiveCommand { get; set; }

        public SendMailCommand OpenSendWindow { get; set; }

        /*
         * Properties für die Mails
         */

        private string Id { get; set; }

        private string sender;
        public string Sender
        {
            get => sender;

            set
            {

                sender = value;
                OnPropertyChanged();

            }
        }

        private string receiver;
        public string Receiver
        {
            get => receiver;

            set
            {

                receiver = value;
                OnPropertyChanged();

            }
        }

        private string subject;
        public string Subject
        {
            get => subject;

            set
            {

                subject = value;
                OnPropertyChanged();

            }
        }

        private string mailText;
        public string MailText
        {
            get => mailText;

            set
            {

                mailText = value;
                OnPropertyChanged();

            }
        }

        public void ShowMail(object sender)
        {
            ListBox listBox = (ListBox)sender;

            Element element = (Element)listBox.SelectedItem;

            Sender = element.Sender;
            Receiver = element.Receiver;
            Subject = element.Subject;
            Id = element.Id;

            string fileName = searchFileName();

            MailText = ReadFile(fileName);
        }

        // in eine andere Klasse auslagern
        private string searchFileName()
        {
            string fileName = null;
            string path = @"c:\Mailprogramm2";
            DirectoryInfo di = new DirectoryInfo(path);

            FileInfo[] files = di.GetFiles();

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(Id))
                {
                    fileName = file.Name;
                }
            }

            return fileName;
        }

        private string ReadFile(string fileName)
        {
            string message = null;
            PropertiesForJSON jsonObject = new PropertiesForJSON();
            string path = @"c:\Mailprogramm2";

            using (StreamReader sr = File.OpenText(path + @"\" + fileName))
            {
                string json = sr.ReadToEnd();

                jsonObject = JsonConvert.DeserializeObject<PropertiesForJSON>(json);

            }

            return message = jsonObject.Nachricht;
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void LoadOrUpdateList()
        {
            foreach (Element e in Element.ReadData(Database.SelectReceivedMails()))
            {
                Mails.Add(e);
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
