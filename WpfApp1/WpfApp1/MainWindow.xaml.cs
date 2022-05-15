using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Mails mails = new Mails();
        public static ServerService service = ServerService.serverService;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {

            if (MainWindow.service.NotConnected())
            {
                MessageBox.Show("Eine Abmeldung ist nicht notwendig, da keine Verbindung zur Zeit besteht.", "Abmeldung", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            else
            {
                MainWindow.service.Logout();
                MessageBox.Show("Sie sind abgemeldet.", "Abmeldung", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Anmelden anmelden = new Anmelden();
            anmelden.Owner = this;
            anmelden.ShowDialog();
            

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void MailsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindowViewModel.mainwindowviewmodel.ShowMail(sender);
        }
    }
}
