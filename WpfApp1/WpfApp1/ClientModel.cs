using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class ClientModel
    {
        private string host = "mail.gmx.net";
        private int port = 465;
        private bool ssl = true;

        private SmtpClient client = new SmtpClient();

        /// <summary>
        /// Connect the client with the mailserver.
        /// </summary>
        private void connect() 
        {
            client.Connect(host, port, ssl);

            // vielleicht nochmal überdenken und überarbeiten?
            client.Authenticate(Verifizierung.Username, Verifizierung.Password);
        }

        /// <summary>
        /// Call the createMail method from Mail class and send this message to the receiver.
        /// </summary>
        public void sendMessage(MimeMessage message)
        {
            try
            {
                connect();
                client.Send(message);
                client.Disconnect(true);

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
            

        }
    }
}
