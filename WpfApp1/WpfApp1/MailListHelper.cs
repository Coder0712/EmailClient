using MimeKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    /// <summary>
    /// Stellt eine Methdoe bereit, die zum aktualisieren der MailList im MainWindow dient. 
    /// Die MailList befindet sich im MainWindowViewModel.
    /// </summary>
    public class MailListHelper
    {
        
        /// <summary>
        /// Updated die MailList im MainWindow.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="viewModel"></param>
        public static void UpdateMailList(ServerService service, MainWindowViewModel viewModel)
        {
            Mails mailsobject = new Mails();

            if (service.IsConnectedAndAuthenticatedToServer())
            {
                viewModel.Mails.Clear();

                MainWindow.service.NewMailCheckThread();

                if (service.MailList.Count != 0)
                {
                    /*foreach (MimeMessage message in MainWindow.service.MailList)
                    {
                        mails.Add(message);
                    }*/

                    mailsobject.GetandSaveMails(service.MailList);

                    if (viewModel != null)
                    {
                        //MainWindowViewModel.mainwindowviewmodel.Mails.CollectionChanged
                        // effizentiere Lösung

                        viewModel.LoadOrUpdateList();

                    }

                }


            }
        }
    }
}
