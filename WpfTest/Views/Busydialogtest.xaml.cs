using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CustomControls;
using GalaSoft.MvvmLight.Command;

namespace WpfTest.Views
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Busydialogtest : Window, INotifyPropertyChanged
    {
        private Action _work;
        private int num = 0;
        private Action<BusyDialog.BusyDialogResult> _work1;
        private ICommand doCommand;
        private void Work1(BusyDialog.BusyDialogResult result)
        {
            for (int i = 0; i < 5; i++)
            {
                num += i;
                Console.WriteLine("AAAAAAAAAAAAAAAAAA");
                Thread.Sleep(600);
            }
            Console.WriteLine("回调方法执行完毕结果为   " + num);
        }

        private string _aaa = "aaa";

        public Busydialogtest()
        {
            InitializeComponent();
            // ps.Title = "asdasdasd";
            Work = Do;
            _work1 = Work1;
            DoCommand=new RelayCommand(Do);
        }
        private void Do()
        {
            for (int i = 0; i < 8000; i++)
            {
                num += i;
                Console.WriteLine("AAAAAAAAAAAAAAAAAA");
            }
            Console.WriteLine("传入方法执行完毕结果为   " + num);
        }
        public string AAA
        {
            get { return _aaa; }
            set
            {
                _aaa = value;
                OnPropertyChanged("AAA");
            }
        }

        public Action Work
        {
            get { return _work; }
            set
            {
                _work = value;
                OnPropertyChanged("Work");
            }
        }

        public ICommand DoCommand
        {
            get { return doCommand; }
            set { doCommand = value; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //  ps.ExeBindedAction();
            // ps.DoWork(_work);

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string StartDirectory = @"c:\aa";
            string EndDirectory = @"c:\cc";

            //foreach (string filename in Directory.EnumerateFiles(StartDirectory))
            //{
            //    using (FileStream SourceStream = File.Open(filename, FileMode.Open))
            //    {
            //        using (FileStream DestinationStream = File.Create(EndDirectory + filename.Substring(filename.LastIndexOf('\\'))))
            //        {
            //            await SourceStream.CopyToAsync(DestinationStream);
            //        }
            //    }
            //}

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
