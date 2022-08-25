using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace officeA
{
    public partial class MainWindow : Window
    {
        private string encryptedString;
        private string fileContent;

        public MainWindow()
        {
            InitializeComponent();
        }

        string Encrypt_Encode(string textToEncrypt, string passphrase)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128
            };

            byte[] pwdBytes = Encoding.UTF8.GetBytes(passphrase);
            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = new byte[16];

            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
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
                fileContent = File.ReadAllText(openFileDialog.FileName);
                myFileName.Content = Path.GetFileName(openFileDialog.FileName);
                saveToTxt.IsEnabled = true;
                saveToTxt.Foreground = Brushes.White;
            }
        }

        private void SaveEncyptedFile_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x7B, 0xA4, 0x98));

            encryptedString = Encrypt_Encode(fileContent, "A16ByteKey......");

            var sfd = new SaveFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "TXT Files (*.txt)|*.txt"
            };

            if (sfd.ShowDialog() == true)
            {
                string filename = sfd.FileName;
                File.WriteAllText(filename, encryptedString);
            }

            MessageBox.Show("File has been saved successfully.");

            myFileName.Content = "";

            saveToTxt.IsEnabled = false;
            saveToTxt.Foreground = brush;
        }

    }

}
