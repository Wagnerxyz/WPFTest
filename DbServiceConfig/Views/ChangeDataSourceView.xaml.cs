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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProductConfig
{
    /// <summary>
    /// ChangeDataSourceView.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeDataSourceView : Window
    {
        public ChangeDataSourceView()
        {
            InitializeComponent();
        }
        private void SourceInfoList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb.SelectedIndex = 0;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeDataSourceViewModel vm = (ChangeDataSourceViewModel) this.DataContext;
            cb.SelectedValue.ToString();
        }
    }
}
