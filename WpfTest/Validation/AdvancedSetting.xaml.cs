using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// Advanced_Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Advanced_Setting : Window, INotifyPropertyChanged
    {
        private ObservableCollection<ValidationError> _validationErrors = new ObservableCollection<ValidationError>();
        private ProjectConfig _config = ProjectConfig.Default();
        public Advanced_Setting()
        {
           // Config = new ProjectConfig();
            InitializeComponent();
        }
        public class ProjectConfig
        {
            public string ServerReceiveIP { get; set; }

            public int ServerReceivePort { get; set; }

            public string ClientReceiveIP { get; set; }

            public int ClientReceivePort { get; set; }

            public static ProjectConfig Default()
            {
                var config = new ProjectConfig();
                config.ServerReceiveIP = "230.120.1.130";
                config.ServerReceivePort = 8825;
                config.ClientReceiveIP = "230.120.1.110";
                config.ClientReceivePort = 8830;
                return config;
            }

        }

        public int Port
        {
            get { return _port; }
            set
            {
                if (value<5000)
                {
                    throw new ArgumentException("参数太小");
                }
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged("Port");
                }
            }
        }

        public ProjectConfig Config
        {
            get { return _config; }
            set
            {
                if (_config != value)
                {
                    _config = value;
                    OnPropertyChanged("Config");
                }
            }
        }private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Config.ServerReceiveIP = Config.;
            //Config.ServerReceivePort = ServerReceivePort;
            //Config.ClientReceiveIP = ClientReceiveIP;
            //Config.ClientReceivePort = ClientReceivePort;
            Close();
        }
        public ObservableCollection<ValidationError> ValidationErrors
        {
            get { return _validationErrors; }
            private set { _validationErrors = value; }
        }

        private void Validation_OnError(object sender, ValidationErrorEventArgs e)
        {
            if (btnsave == null)
            {
                return;
            }
            if (e.Action == ValidationErrorEventAction.Added)
            {
                ValidationErrors.Add(e.Error);
            }
            else
            {
                ValidationErrors.Remove(e.Error);
            }
        }

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
private  int _port;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
