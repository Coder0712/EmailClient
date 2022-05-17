using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace WpfApp1
{
    public class ServerService : INotifyPropertyChanged
    {   // Diese Klasse muss Threadsafe sein

        /*
         * Server Connection
         */

        //Props für Service
        public event PropertyChangedEventHandler PropertyChanged;

        private static ObservableCollection<MimeMessage> mailList = new ObservableCollection<MimeMessage>();
        public ObservableCollection<MimeMessage> MailList
        {
            get
            {
                return mailList;
            }

            set
            {
                if (mailList != value)
                {
                    mailList = value;
                    OnPropertyChanged();
                }
            }
        }

        /*
         * Instanziierung
         */

        private static ServerService service = null;
        private static readonly object padlock = new object();

        ServerService()
        {
        }

        public static ServerService serverService
        {
            get
            {
                if (service == null)
                {
                    lock (mailList)
                    {
                        if (service == null)
                        {
                            service = new ServerService();
                        }
                    }
                }
                return service;
            }
        }

        /// <summary>
        /// Liefert einen Bool zurück, ob éine Verbindung zum Server besteht
        /// </summary>
        public bool IsConnectedAndAuthenticated { get; private set; }

        private ImapClient imapClient = new ImapClient();

        /// <summary>
        /// Stellt eine Verbindung zum Server her. 
        /// </summary>
        /// <returns></returns>
        public async Task Connection()
        {

            try
            {
                await imapClient.ConnectAsync(ServerInfoHelper.Server, ServerInfoHelper.Port, ServerInfoHelper.SSL);
            }
            catch (Exception e)
            {
                // Auch noch einen Serevr bauen
                MessageBox.Show(e.Message, "Achtung", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                await imapClient.AuthenticateAsync(Verifizierung.Username, Verifizierung.Password);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Achtung", MessageBoxButton.OK, MessageBoxImage.Error);
                imapClient.Disconnect(true);
            }

        }

        /// <summary>
        /// Trennt die Verbindung zum Server.
        /// </summary>
        public void Logout()
        {
            imapClient.Disconnect(true);
        }

        /// <summary>
        /// Prüft, ob eine Verbindung zum Server besteht und eine Authentifizierung erfolgte.
        /// </summary>
        /// <returns></returns>
        public bool IsConnectedAndAuthenticatedToServer()
        {

            if (imapClient.IsConnected && imapClient.IsAuthenticated)
            {
                IsConnectedAndAuthenticated = true;
            }

            return IsConnectedAndAuthenticated;
        }

        public bool NotConnected()
        {
            bool notConnected = false;
            if (imapClient.IsConnected == false)
            {
                notConnected = true;
            }

            return notConnected;
        }

        /*
         * event stellt eine Liste von Action<MimeMessage> bereit
         */
        //public event EventHandler<MimeMessage> NewMailReceived;
       

        public void NewMailCheckThread()
        {
            if (IsConnectedAndAuthenticatedToServer())
            {
                Thread ReceiveMailsThread = new Thread(() => {
                    string dateMail;

                    ObservableCollection<MimeMessage> mails = new ObservableCollection<MimeMessage>();

                    imapClient.Inbox.Open(FolderAccess.ReadOnly);

                    foreach (UniqueId id in imapClient.Inbox.Search(SearchQuery.DeliveredOn(System.DateTime.Today)))
                    {
                        // Datetime prüfen und darauf prüfen
                        DateTime date = imapClient.Inbox.GetMessage(id).Date.LocalDateTime;

                        string dateString = Convert.ToString(date);
                        dateMail = dateString.Substring(0, 11);

                        string todayString = Convert.ToString(System.DateTime.Today);
                        string today = todayString.Substring(0, 11);

                        if (dateMail == today)
                        {
                            mails.Add(imapClient.Inbox.GetMessage(id));
                        }
                    }

                    mailList = mails;

                });
                ReceiveMailsThread.Name = "ReceiveMails";
                ReceiveMailsThread.Start();
                ReceiveMailsThread.Join(8000);
            }
            // Testen
            // Wenn neue mail
            //NewMailReceived?.Invoke(this, new MimeMessage());
        }
        
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(name)));
        }
    }
}
