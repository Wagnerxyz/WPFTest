using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfTest.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RibbonChangeTheme : Window
    {
        public RibbonChangeTheme()
        {
            InitializeComponent();
        }
        private void RibbonGallery_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Application.Current.ApplyTheme("ShinyDarkTeal");
            ResourceDictionary resource = null;
            //  ComboBoxItem cbi = (ComboBoxItem)comboBox1.SelectedItem;

            RibbonGalleryItem cbi = (RibbonGalleryItem)cb.SelectedItem;
            string sitem = cbi.Content.ToString();
            try
            {
                resource = Application.LoadComponent((new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.normalcolor.xaml", UriKind.Relative))) as ResourceDictionary;
               // resource = Application.LoadComponent((new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.homestead.xaml", UriKind.Relative))) as ResourceDictionary;
               //resource = Application.LoadComponent((new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.metallic.xaml", UriKind.Relative))) as ResourceDictionary;
               // resource = Application.LoadComponent((new Uri("/PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/classic.xaml", UriKind.Relative))) as ResourceDictionary;
               // resource = Application.LoadComponent(new Uri(@"Themes\" + sitem + ".xaml", UriKind.Relative)) as ResourceDictionary;
            }
            catch
            {
            }

            if (resource != null)
            {
                if (Application.Current.Resources.MergedDictionaries.Count > 0)
                {
                    Application.Current.Resources.MergedDictionaries.Clear();
                }
                Application.Current.Resources.MergedDictionaries.Add(resource);
            }
        }
    }
}
