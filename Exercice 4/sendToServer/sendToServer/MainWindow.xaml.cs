using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Media;

namespace sendToServer
{
    public partial class MainWindow : Window
    {
        private string myFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "All files (*.*)|*.*"
            };


            if (openFileDialog.ShowDialog() == true)
            {
                myFile = openFileDialog.FileName;
                myFileName.Content = Path.GetFileName(openFileDialog.FileName);
                send.IsEnabled = true;
                send.Foreground = Brushes.White;
            }
        }

        private void SendToServer_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x7B, 0xA4, 0x98));

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44313/Security/GetToken");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        Username = "admin@mohammadayoub.com",
                        Password = "P@ssword"
                    });
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                string result;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                    int lastCharacter = result.Length - 2;
                    result = result.Substring(1, lastCharacter);
                }

                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;
                client.Headers.Add("Authorization", "Bearer " + result);
                byte[] rawResponse = client.UploadFile(@"https://localhost:44313/Files/UploadFiles", "POST", myFile);
                string output = Encoding.ASCII.GetString(rawResponse);
                MessageBox.Show(output);
                client.Dispose();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            myFileName.Content = "";

            send.IsEnabled = false;
            send.Foreground = brush;
        }
    }
}
