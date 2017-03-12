using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace ProductConfig
{
    /// <summary>
    /// Load ServiceName from ini file and get status
    /// </summary>
    public partial class AtpServiceManager : UserControl
    {
        private  string _servicename;
        public AtpServiceManager()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            _servicename = App.ConfigFile.GetIniFileString("General", "ServiceName");
            this.Visibility = Visibility.Collapsed;
            CheckStatus();
        }
     
        private void CheckStatus()
        {
            if (!string.IsNullOrEmpty(_servicename))
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController s in services)
                {
                    if (s.ServiceName.ToUpperInvariant() == _servicename.ToUpperInvariant())
                    {
                        try
                        {
                            ServiceNameButton.Content = "打开/关闭" + _servicename + "服务";

                            ServiceController sc = new ServiceController(_servicename);
                            switch (sc.Status)
                            {
                                case ServiceControllerStatus.Running:
                                    ServeceStatus.Text = "Status:Running"; break;
                                case ServiceControllerStatus.Stopped:
                                    ServeceStatus.Text = "Status:Stopped"; break;
                                case ServiceControllerStatus.Paused:
                                    ServeceStatus.Text = "Status:Paused"; break;
                                case ServiceControllerStatus.StopPending:
                                    ServeceStatus.Text = "Status:Stopping"; break;
                                case ServiceControllerStatus.StartPending:
                                    ServeceStatus.Text = "Status:Starting"; break;
                                default:
                                    ServeceStatus.Text = "Status:Changing"; break;
                            }
                            this.Visibility = Visibility.Visible; 
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e.Message);
                        }
                    }
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
            ServiceController service = new ServiceController(_servicename);

            if (service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(10000));
                ServeceStatus.Text = "Stopped";
            }
            else if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(10000));
                ServeceStatus.Text = "Running";
            }

        }

      
    }
}
