using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1
{
    public class SendMailViewModel : INotifyPropertyChanged
    {

        MailModel mailModel = new MailModel();
        public string fileName = null;

        public SendMailViewModel()
        {
            this.SendCommand = new SendMailCommand(
                (o) =>
                {
                    ClientModel clientModel = new ClientModel();
                    clientModel.sendMessage(mailModel.BuildMail());
                },
                (o) => !string.IsNullOrEmpty(Sender) && !string.IsNullOrEmpty(Receiver) && !string.IsNullOrEmpty(Subject)
                );

            this.AddCommand = new SendMailCommand(
                (o) =>
                {
                    /* OPenFilaDialog kann auch mehrere Dateien auswählen
                     * mit Multiselect
                     * kann säter implementiert werden
                     */

                    OpenFileDialog fileDialog = new OpenFileDialog();
                    if (fileDialog.ShowDialog() == true)
                    {
                        Name = fileDialog.SafeFileName;
                        AttachmentList.Add(Name);
                        mailModel.AddAttachments(fileDialog.FileName);
                    }
                },
                (o) => !string.IsNullOrEmpty(Sender)
                );

           
        }

        // das was in der View eingegeben wird, wird hier rein gespeichert

        private string name;
        public string Name
        {
            get => name;

            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<HandleFileName> attachmentList;
        public ObservableCollection<HandleFileName> AttachmentList
        {
            get
            {
                if(attachmentList == null)
                {
                    attachmentList = new ObservableCollection<HandleFileName>();
                }

                return attachmentList;
            }

            set
            {
                if(attachmentList!= value)
                {
                    attachmentList = value;
                    OnPropertyChanged();
                }
            }
        }


        private string sender;
        public string Sender {
            
            get => sender;

            set
            {
                if(sender != value)
                {
                    sender = value;
                    mailModel.Sender = sender;
                    OnPropertyChanged();

                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string receiver;
        public string Receiver {
            get => receiver;

            set
            {
                if(receiver != value)
                {
                    receiver = value;
                    mailModel.Receiver = receiver;
                    OnPropertyChanged();
                }
            }
        
        }

        private string subject;
        public string Subject {

            get => subject;

            set
            {
                if(subject != value)
                {
                    subject = value;
                    mailModel.Subject = subject;
                    OnPropertyChanged();

                    // Feuert Event ab für Button
                    SendCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string message;
        public string Message {

            get => message;

            set
            {
                if(message != value)
                {
                    message = value;
                    mailModel.Message = message;
                    OnPropertyChanged();
                }
            }
        
        }


        /* OnPropertyChanged
         * registriert sich auf das Event
         * Wenn sich nun Property ändert soll Event geworfen werden
         * allgemein formuliert, da jede Property dieses Event aufruft
         */

        /* CallerMemberName:
        * Name der Eigenschaft, welche Event ausgelöst hat
        * 
        */

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(name)));
        }


        /* Unter welcher Vorrausetzung soll der Button ausgeführt werden dürfen. 
         */

        public event PropertyChangedEventHandler PropertyChanged;


        public SendMailCommand SendCommand { get; set;}

        public SendMailCommand AddCommand { get; set; }

        


    }
}
