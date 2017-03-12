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
    /// CstomeroutedEvent.xaml 的交互逻辑
    /// </summary>
    public partial class CustomroutedEvent : Window
    {
        public CustomroutedEvent()
        {
            InitializeComponent();
        }

        private void TimeButton_ReportTime(object sender, ReportTimeEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            string timeStr = e.ClickTime.ToString();
            string content = string.Format("{0}到达{1}", timeStr, element.Name);
            this.listBox.Items.Add(content);
        }

        private void sp1_ReportTime(object sender, RoutedEventArgs e)
        {

        }
    }
}
