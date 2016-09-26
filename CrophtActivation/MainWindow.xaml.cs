using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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
using DAL;
using Microsoft.Win32;
using ModelsLib;

namespace CrophtActivation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public XmlRepository<Login> ConfigRepository { get; set; }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UnameBox.Text.Length < 3 || PwdBox.Text.Length < 3)
            {
                MessageBox.Show("Ya el haj ! User or password must contain more then 3 words");
                return;
            }
            var key = "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00";
            if (ActivationChk.IsChecked != null && (bool) ActivationChk.IsChecked)
                key = ToMd5(GetMacAddress());
            var login = new Login
            {
                Id = 0,
                Nom = UnameBox.Text,
                MotDePasse = ToMd5(PwdBox.Text),
                Secret = ToMd5(UnameBox.Text + PwdBox.Text + DateTime.Today.Month + DateTime.Today.Year),
                Serial = key
            };
            var dlg = new SaveFileDialog();
            dlg.FileName = "config.xml";
            dlg.ShowDialog();
            if (File.Exists(dlg.FileName))
                File.Delete(dlg.FileName);
            ConfigRepository = new XmlRepository<ModelsLib.Login>(dlg.FileName);
            ConfigRepository.AddOrUpdate(login);
        }

        public void ResetDatabase()
        {
            var dbfname = "OphtalmoDb.sdf";
            var dlg = new SaveFileDialog();
            dlg.FileName = dbfname;
            dlg.DefaultExt = ".sdf";
            dlg.ShowDialog();
            if (File.Exists(dlg.FileName))
            {
                var str = DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                File.Copy(dlg.FileName, "OphtalmoDb" + str + ".sdf.old");
                File.Delete(dlg.FileName);
            }
            if (dlg.FileName != null) File.AppendAllText(dlg.FileName, "");
        }



        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetDatabase();
        }

        public static string ToMd5(string txtToHash)
        {
            if ((txtToHash == null) || (txtToHash.Length == 0))
            {
                return string.Empty;
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            var textToHash = Encoding.Default.GetBytes(txtToHash);
            var result = md5.ComputeHash(textToHash);
            return System.BitConverter.ToString(result);
        }

        public static string GetMacAddress()
        {
            return (from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where
                        nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
        }
    }
}
