using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace downloadFromServer
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RequestedFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xAA, 0xAA, 0xAA));

            if ( ! requestedFile.Text.Equals(String.Empty))
            {
                download.IsEnabled = true;
                download.Foreground = Brushes.White;
            }
            else
            {
                download.IsEnabled = false;
                download.Foreground = brush;
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
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
                Stream stream = client.OpenRead(@"https://localhost:44313/Files/DownloadFiles/" + requestedFile.Text);
                StreamReader reader = new StreamReader(stream);
                string output = reader.ReadToEnd();
                MessageBox.Show(output);
                client.Dispose();

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = "/C python -u C:/Users/user/Desktop/\"Mohammad Ayoub\"/\"Part 1\"/\"Exercice 2\"/officeB/officeB.py C:/Users/user/Desktop/\"Mohammad Ayoub\"/\"Part 1\"/\"Exercice 3\"/officeCommunication/\"Downloaded Files\"/" + requestedFile.Text + " C:/Users/user/Desktop/\"Mohammad Ayoub\"/\"Part 1\"/\"Exercice 5\"/DownloadFromServer/\"Downloaded Files\"/" + requestedFile.Text
                };
                process.StartInfo = startInfo;
                process.Start();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            requestedFile.Text = "";

            download.IsEnabled = false;
            download.Foreground = brush;
        }

    }
}
