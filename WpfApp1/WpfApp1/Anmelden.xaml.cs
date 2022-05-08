using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaktionslogik für Anmelden.xaml
    /// </summary>
    public partial class Anmelden : Window
    {
        private ObservableCollection<MimeMessage> mails = new ObservableCollection<MimeMessage>();

        public Anmelden()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            Verifizierung.Username = mail.Text;
            Verifizierung.Password = password.Password;
            Mails mailsobject = new Mails();
            //ServerService readMessages = new ServerService();
            

            // instanz vom ServerService
            await Task.Run(()=>MainWindow.service.Connection());


            MailListHelper.UpdateMailList(MainWindow.service, MainWindowViewModel.mainwindowviewmodel);

            /*if (MainWindow.service.IsConnectedAndAuthenticatedToServer())
            {
                MainWindowViewModel.mainwindowviewmodel.Mails.Clear();

                MainWindow.service.NewMailCheckThread();

                if(MainWindow.service.MailList.Count != 0)
                {
                    foreach (MimeMessage message in MainWindow.service.MailList)
                    {
                        mails.Add(message);
                    }

                    mailsobject.GetandSaveMails(mails);

                    if (MainWindowViewModel.mainwindowviewmodel != null)
                    {
                        //MainWindowViewModel.mainwindowviewmodel.Mails.CollectionChanged
                        // effizentiere Lösung

                        MainWindowViewModel.mainwindowviewmodel.LoadOrUpdateList();
                            
                    }
                    
                }


            }*/
            Close();


        }
    }
}
