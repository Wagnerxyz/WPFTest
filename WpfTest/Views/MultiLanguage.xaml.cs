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

namespace WpfTest.Views
{
    /// <summary>
    /// Interaction logic for MultiLanguage.xaml
    /// </summary>
    public partial class MultiLanguage : Window
    {
        public MultiLanguage()
        {
            InitializeComponent();
        }
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResourceDictionary langRd = null;

            string lang = "";

            ComboBoxItem cbi = (ComboBoxItem)comboBox1.SelectedItem;

            string sitem = cbi.Content.ToString();


            if (sitem == "中文")
                lang = "zh-CN";
            if (sitem == "English")
                lang = "en-US";

            try
            {
                langRd =
                    Application.LoadComponent(
                             new Uri(@"Lang\" + lang + ".xaml", UriKind.Relative))
                    as ResourceDictionary;
            }
            catch
            {
            }

            if (langRd != null)
            {
                if (Application.Current.Resources.MergedDictionaries.Count > 0)
                {
                    Application.Current.Resources.MergedDictionaries.Clear();
                }
                Application.Current.Resources.MergedDictionaries.Add(langRd);
            }
        }
    }
}
