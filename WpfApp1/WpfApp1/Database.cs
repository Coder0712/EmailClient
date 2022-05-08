using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MimeKit;
using System.Data;

namespace WpfApp1
{
    public static class Database
    {
        /*
         * good Practices: 
         * using nutzen, da sofort geschlossen wird. 
         * Datenbankverbindung permanent offen halten
         * 
         * SQL Server kann entfernt werden, da String beim Client eingetragen werden kann
         * Nutzer kann entfernt werden, da Windows Authentifizierung
         * 
         */

        private static string server = @"LAPTOP-9PUBBRD1\SQLEXPRESS";
        //private static string usernameServer = "MailCient";
        //private static string passwordServer = "mailclient";
        private static string defaultDatabase = "master";
        public static string message = null;
        private static List<KeyVal<string, string>> results = new List<KeyVal<string, string>>();

        public static List<string> list = new List<string>();
        public static string MessageInput { get; private set; }

        private static List<string[]> strA_List = new List<string[]>();

        public static async Task ConnectionToServer()
        {

            SqlConnection connection = new SqlConnection(Connection().ConnectionString);

            try
            {
                await connection.OpenAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Datenbankverbindung", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // unnötig
        public static async Task DisconnectFromServer()
        {
            SqlConnection connection = new SqlConnection(Connection().ConnectionString);
            await connection.CloseAsync();
        }

        private static SqlConnectionStringBuilder Connection()
        {
            //Trusted_Connection=True;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = "Data Source=" + server + ";Initial Catalog=" + defaultDatabase + "; Integrated Security=true; encrypt=true; trustServerCertificate=true; ";
            //"User ID=" + usernameServer + ";Password=" + passwordServer;

            return builder;
        }

        public async static Task CheckDataBaseandCreate()
        {
            string queryCheckCreate = "IF DB_ID('MailsDB') IS NULL " +
                "BEGIN " +
                "CREATE DATABASE MailsDB END";

            SqlConnection connection = new SqlConnection(Connection().ConnectionString);

            SqlCommand command = new SqlCommand(queryCheckCreate, connection);

            using (connection)
            using (command)
            {
                try
                {
                    await connection.OpenAsync();

                    // Event um Message zu bekommen, welche geprinted wird 
                    //connection.InfoMessage += infomessage;
                    await command.ExecuteNonQueryAsync();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        public async static Task CreateTables()
        {
            string queryBuild = "USE[MailsDB] IF OBJECT_ID('ReceivedMails') IS NULL " +
                "BEGIN " +
                "CREATE TABLE ReceivedMails (" +
                "ID int NOT NULL PRIMARY KEY(ID) IDENTITY(1, 1)," +
                "Receiver nvarchar(100) NULL," +
                "Sender nvarchar(100) NULL," +
                "Subject nvarchar(250) NULL," +
                "Date nvarchar(50) NULL)" +

                "CREATE TABLE SenderMails(" +
                "ID int NOT NULL PRIMARY KEY(ID) IDENTITY(1, 1)," +
                "Receiver nvarchar(100) NULL," +
                "Sender nvarchar(100) NULL," +
                "Subject nvarchar(250) NULL," +
                "Date nvarchar(50) NULL) " +
                "END";

            SqlConnection connection = new SqlConnection(Connection().ConnectionString);

            SqlCommand command = new SqlCommand(queryBuild, connection);

            using (connection)
            using (command)
            {
                try
                {
                    await connection.OpenAsync();

                    // Event um Message zu bekommen, welche geprinted wird 
                    //connection.InfoMessage += infomessage;
                    await command.ExecuteNonQueryAsync();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        /// <summary>
        /// Prüft ob Mails in der Tabelle ReceivedMails existieren
        /// </summary>
        /// <returns></returns>
        public static bool CheckMails()
        {
            // parameter für WriteMails
            //string reseiver, string sender, string subject

            string select = "USE[MailsDB] SELECT * FROM ReceivedMails";
            object mails = null;

            SqlConnection connection = new SqlConnection(Connection().ConnectionString);


            SqlCommand command = new SqlCommand(select, connection);

            using (connection)
            using (command)
            {
                try
                {
                    connection.Open();

                    // Event um Message zu bekommen, welche geprinted wird 
                    //connection.InfoMessage += infomessage;
                    mails = command.ExecuteScalar();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return mails == null ? false : true;

        }

        /// <summary>
        /// Gibt SCOPE ID nach Insert aus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            MessageInput = e.Message;
        }

     

        /// <summary>
        /// Gibt den Betreff und das Datum aller Mails der ReceivedMails-Tabelle in einer Liste mit KeyVals zurürck
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<KeyVal<string, string>> CommandReader(string date)
        {
            string select = "SELECT [Subject], [Date] FROM MailsDB.dbo.ReceivedMails WHERE [date] = @actualDate";

            SqlConnection connection = new SqlConnection(Connection().ConnectionString);

            SqlCommand command = new SqlCommand(select, connection);
            command.Parameters.AddWithValue("@actualDate", date);

            using (connection)
            using (command)
                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        

                        /* liest jede Zeile der Abfrage
                         * und erstellt für jede Zeile ein KeyVal
                         * und packt Wert der Spalte in KeyVal Props
                         */
                        while (reader.Read())
                        {
                            KeyVal<string, string> keyval = new KeyVal<string, string>();
                            
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if(reader.GetName(i) == "Subject")
                                {
                                    keyval.id = reader.GetString(i);
                                }
                                else
                                {
                                    keyval.value = reader.GetString(i);

                                    results.Add(keyval);
                                }


                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            return results;
        }

        /// <summary>
        /// Führt einen Insert in die ReceivedMails Tabelle aus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="date"></param>
        public static void InsertQuery(string sender, string receiver, string subject, string date)
        {

            string insert = "DECLARE @id int INSERT INTO MailsDB.dbo.ReceivedMails (Receiver, Sender,Subject,Date) VALUES (@receiver, @sender ,@subject ,@date ) " +
                "SET @id = SCOPE_IDENTITY(); " +
                "PRINT @id;";

            SqlConnection connection = new SqlConnection(Connection().ConnectionString);


            SqlCommand command = new SqlCommand(insert, connection);

            command.Parameters.AddWithValue("@receiver", receiver);
            command.Parameters.AddWithValue("@sender", sender);
            command.Parameters.AddWithValue("@subject", subject);
            command.Parameters.AddWithValue("@date", date);

            using (connection)
            using (command)
            {
                try
                {
                    connection.OpenAsync();
                    connection.InfoMessage += connection_InfoMessage;
                    command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static List<string[]> SelectReceivedMails()
        {
            string query = "SELECT * FROM MailsDB.dbo.ReceivedMails";

            SqlConnection connection = new SqlConnection(Connection().ConnectionString);

            SqlCommand command = new SqlCommand(query, connection);

            using (connection)
            using (command)
                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {



                        /* liest jede Zeile der Abfrage
                         * und erstellt für jede Zeile ein KeyVal
                         * und packt Wert der Spalte in KeyVal Props
                         */
                        while (reader.Read())
                        {
                            string[] strA_mailData = new string[5];

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        //ID
                                        strA_mailData[0] = Convert.ToString(reader.GetInt32(i));
                                        break;
                                    case 1:
                                        // Receiver
                                        strA_mailData[1] = reader.GetString(i);
                                        break;
                                    case 2:
                                        // Sender
                                        strA_mailData[2] = reader.GetString(i);
                                        break;
                                    case 3:
                                        //Subject
                                        strA_mailData[3] = reader.GetString(i);
                                        break;
                                    case 4:
                                        //Date
                                        strA_mailData[4] = reader.GetString(i);
                                        break;
                                    default:
                                        break;
                                }

                                
                            }

                            strA_List.Add(strA_mailData);
                        }
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            return strA_List;
        }
    }
}
