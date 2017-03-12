using System;
using System.Collections.Generic;
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

namespace WpfTest.Views
{
    /// <summary>
    /// Validate.xaml 的交互逻辑
    /// </summary>
    public partial class Validate : Window
    {
        public Validate()
        {
            InitializeComponent();

            this.DataContext = new Customer() {Age = 3,FirstName = "josh",LastName = "groban"};

        }

    }
}
