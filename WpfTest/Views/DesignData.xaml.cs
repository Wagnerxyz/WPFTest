using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTest.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DesignData : Window, INotifyPropertyChanged
    {
        public bool Tab1Selected;
        Tuple<string, int>[] scores =       
                    { new Tuple<string, int>("Jack", 78),
                      new Tuple<string, int>("Abbey", 92), 
                      new Tuple<string, int>("Dave", 88),
                      new Tuple<string, int>("Sam", 91), 
                      new Tuple<string, int>("Ed", 123),
                      new Tuple<string, int>("Penelope", 82),
                      new Tuple<string, int>("Linda", 99),
                      new Tuple<string, int>("Judith", 84) };

        private string _name;

        public DesignData()
        {
            InitializeComponent();
           // textBox1.DataContext = this;
           
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(typeof(ABase).IsAssignableFrom(typeof(Iinterface)));    // false
            Console.WriteLine(typeof(ABase).IsSubclassOf(typeof(Iinterface)));        // false

            Console.WriteLine(typeof(Iinterface).IsAssignableFrom(typeof(ABase)));    // true
            Console.WriteLine(typeof(Iinterface).IsAssignableFrom(typeof(Bderived)));    // true
            Console.WriteLine(typeof(Bderived).IsSubclassOf(typeof(Iinterface)));        // false

            Console.WriteLine(typeof(ABase).IsAssignableFrom(typeof(ABase)));    // true
            Console.WriteLine(typeof(ABase).IsSubclassOf(typeof(ABase)));        // false

            Console.WriteLine(typeof(ABase).IsAssignableFrom(typeof(Bderived)));    // true
            Console.WriteLine(typeof(ABase).IsSubclassOf(typeof(Bderived)));        // false

            Console.WriteLine(typeof(Bderived).IsAssignableFrom(typeof(ABase)));    // false
            Console.WriteLine(typeof(Bderived).IsSubclassOf(typeof(ABase)));        // true
            Console.WriteLine(typeof(Iinterface).IsAssignableFrom(typeof(Iinterface))); //true
            Console.WriteLine(typeof(abs).IsClass.ToString());
            Console.WriteLine(typeof(abs).IsAbstract.ToString());
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            lb1.ItemsSource = scores;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
          
            MessageBox.Show("asdasd", "asdadas", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            throw new ArgumentException();
        }
        public String Name
        {
            get { return _name; }
            set { _name = value;
            OnPropertyChanged("Name");}
        }
    }

    internal abstract class abs
    {
    }
}
