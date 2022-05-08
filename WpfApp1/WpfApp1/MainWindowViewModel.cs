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
                if(mails != value)
                {
                    mails = value;
                    OnPropertyChanged();
                }
            }
         }
        public SendMailCommand ReceiveCommand { get; set; }

        public SendMailCommand OpenSendWindow { get; set; }

        /*
         * Properties für die 
         */

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Subject { get; set; }

        public string MailText { get; set; }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(name)));
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
