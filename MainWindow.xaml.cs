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

using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace MyToastNotification
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyToastNotifications _toast = null;
        private String _appID = String.Empty;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Toast_Click(object sender, RoutedEventArgs e)
        {
            if (_toast == null )
            {
                // Get App ID
                using (PowerShell powershell = PowerShell.Create())
                {
                    powershell.AddScript("Get-StartApps");

                    foreach (PSObject result in powershell.Invoke())
                    {
                        // Find MyToastNotification.exe APP_ID
                        if (result.Members["Name"].Value.ToString().Contains("MyToastNotification"))
                        {
                            _appID = result.Members["AppID"].Value.ToString();
                        }
                    }
                }

            }

            if (!String.IsNullOrEmpty(_appID))
            {
                // Send Toast Notification
                _toast = new MyToastNotifications(_appID);
                _toast.ShowNotification(String.IsNullOrEmpty(_tbCaption.Text) ? "MyCaption" : _tbCaption.Text,
                        String.IsNullOrEmpty(_tbMessage.Text) ? "MyMessage" : _tbMessage.Text);
            }
            else
            {
                MessageBox.Show("Can't find App_ID. Please create a shortcut and copy to \"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\" ");
            }

        }
    }
}
