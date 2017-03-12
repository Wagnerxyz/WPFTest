using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace ProductConfig
{
    /// <summary>
    /// Interaction logic for NtpService.xaml
    /// </summary>
    public partial class NtpService : UserControl
    {
        private readonly string _keyName = Registry.LocalMachine + "\\SYSTEM\\CurrentControlSet\\services\\W32Time\\TimeProviders\\NtpServer";
        private readonly string _keyName2 = Registry.LocalMachine + "\\SYSTEM\\CurrentControlSet\\services\\W32Time\\Config";
        public NtpService()
        {
            InitializeComponent();
            CheckStatus();
        }
        private void CheckStatus()
        {
            if ((int)Registry.GetValue(_keyName, "Enabled", null) == 1)
            {
                NtpButton.Content = "关闭本机时间同步服务器";
                ntpstatus.Text = "本机时间同步服务器状态:open";
            }
        }


        #region NTP
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {//打开状态
                if (NtpButton.Content.ToString() == "关闭本机时间同步服务器")
                {
                    Registry.SetValue(_keyName, "Enabled", 0);
                    Registry.SetValue(_keyName2, "AnnounceFlags", 10);
                    NtpButton.Content = "开启本机时间同步服务器";
                    ntpstatus.Text = "本机时间同步服务器状态:closed";
                }
                    //关闭状态
                else
                {
                    Registry.SetValue(_keyName, "Enabled", 1);
                    Registry.SetValue(_keyName2, "AnnounceFlags", 5);
                    NtpButton.Content = "关闭本机时间同步服务器";
                    ntpstatus.Text = "本机时间同步服务器状态:opened";
                }

                RestartWindowsService("W32Time");
                // System.Diagnostics.Process.Start("w32tm","resync");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        private void RestartWindowsService(string serviceName)
        {
            ServiceController serviceController = new ServiceController(serviceName);
            try
            {
                if ((serviceController.Status.Equals(ServiceControllerStatus.Running)) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    serviceController.Stop();
                }
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(1000 * 10));
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(1000 * 10));
            }
            catch (System.ServiceProcess.TimeoutException e)
            {
                MessageBox.Show(Application.Current.MainWindow, e.Message, "重启服务错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

    }
}
