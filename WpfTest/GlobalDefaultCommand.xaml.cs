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
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class GlobalDefaultCommand : Window
    {
        public GlobalDefaultCommand()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent MyButtonClickEvent =
             EventManager.RegisterRoutedEvent("MyButtonClick", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(CustomControls.UserControl1));

        public event RoutedPropertyChangedEventHandler<object> MyButtonClick
        {
            add
            {
                this.AddHandler(MyButtonClickEvent, value);
            }

            remove
            {
                this.RemoveHandler(MyButtonClickEvent, value);
            }
        }

        public void OnMyButtonClick(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg =
                new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, MyButtonClickEvent);

            this.RaiseEvent(arg);
        }
    }
}
