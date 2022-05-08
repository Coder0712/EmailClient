using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    public class Element
    {
        public static List<Element> e_List = new List<Element>();

        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Receiver { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }

        private string Id { get; set; }
        
        public Element(string sender, string receiver, string subject, string date)
        {
            Sender = sender ;
            Receiver = receiver;
            Subject = subject;
            Date = date;
        }

        public static List<Element> ReadData(List<string[]> strA_List)
        {
            string id = null;
            string receiver = null;
            string subject = null;
            string sender = null;
            string date = null;


            foreach (string[] strA_Data in strA_List)
            {
                for (int i = 0; i < strA_Data.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            id = strA_Data[i];
                            break;
                        case 1:
                            receiver = strA_Data[i];
                            
                            break;
                        case 2:
                            sender = strA_Data[i];
                           
                            break;
                        case 3:
                            subject = strA_Data[i];
                            
                            break;
                        case 4:
                           date = strA_Data[i];
                           
                            break;
                    }

                }

                e_List.Add(new Element(sender, receiver, subject, date));
               
            }

            return e_List;

        }
    }
}
