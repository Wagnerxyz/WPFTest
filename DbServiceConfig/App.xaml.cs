using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace ProductConfig
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IniFile ConfigFile;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ConfigFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory+("ATP.ini"));
        }
        
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (e.ExceptionObject as Exception);

            if (exception is Exception)
            {
                MessageBox.Show(exception.ToString(), "错误CurrentDomain_UnhandledException");
                Application.Current.Shutdown();
            }
        }
    }
  
}
