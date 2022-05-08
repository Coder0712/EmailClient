using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class Verifizierung
    {
        public static string Username { get; set; }
        public static string Password { get; set; }

        

        /*bool notConnected;

        public async Task Connection()
        {

            try
            {
                await imapClient.ConnectAsync(server, port, ssl);
            }
            catch (Exception e)
            {
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

        public void Logout()
        {
            imapClient.Disconnect(true);
        }

        public bool ConnectedAndAuthenticatedToServer()
        {
            if (imapClient.IsConnected && imapClient.IsAuthenticated)
            {
                connectedAndAuthenticated = true;
            }

            return connectedAndAuthenticated;
        }

        public bool NotConnected()
        {
            if(imapClient.IsConnected == false)
            {
                notConnected = true;
            }

            return notConnected;
        }*/
    }

}
