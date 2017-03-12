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
using System.Reflection;

namespace WpfTest.Views
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowsTheme : Window
    {
        List<Person> SouthPark = new List<Person>() {
            new Person() { Name = "Eric", Surname="Cartman" },
            new Person() { Name = "Stan", Surname="Marsh" },
            new Person() { Name = "Kyle", Surname="Broflovski" },
            new Person() { Name = "Kenny", Surname="McCormick" },
            new Person() { Name = "Bebe", Surname="Stevens" },
            new Person() { Name = "Clyde", Surname="Donovan" },
            new Person() { Name = "Craig", Surname="Tucker" },
            new Person() { Name = "Jimmy", Surname="Vulmer" },
            new Person() { Name = "Pip", Surname="Pirrup" },
            new Person() { Name = "Token", Surname="Black" },
            new Person() { Name = "Tweek", Surname="Tweak" },
            new Person() { Name = "Wendy", Surname="Testaburger" },
            new Person() { Name = "Annie", Surname="Polk" },
            new Person() { Name = "Randy", Surname="Marsh" },
            new Person() { Name = "Sharon", Surname="Marsh" },
            new Person() { Name = "Shelley", Surname="Marsh" },
            new Person() { Name = "Marvin", Surname="Marsh" },
            new Person() { Name = "Jimbo", Surname="Kern" },
            new Person() { Name = "Gerald", Surname="Broflovski" },
            new Person() { Name = "Sheila", Surname="Broflovski" },
            new Person() { Name = "Ike", Surname="Broflovski" },
            new Person() { Name = "Kyle", Surname="Schwartz" },
            new Person() { Name = "Liane", Surname="Cartman" },
            new Person() { Name = "Stuart", Surname="McCormick" },
            new Person() { Name = "Carol", Surname="McCormick" },
            new Person() { Name = "Kevin", Surname="McCormick" },
            new Person() { Name = "Stephen", Surname="Stotch" },
            new Person() { Name = "Linda", Surname="Stotch" },
            new Person() { Name = "Richard", Surname="Tweak" }
        };

        public WindowsTheme()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            themes.ItemsSource = ThemeManager.GetThemes();
            comboBox.ItemsSource = SouthPark;
            listBox.ItemsSource = SouthPark;
        }

        private void themes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string theme = e.AddedItems[0].ToString();

                ResourceDictionary resource = null;
                //  ComboBoxItem cbi = (ComboBoxItem)comboBox1.SelectedItem;
                switch (theme)
                {
                    case "LNormal": resource = Application.LoadComponent((new Uri("/PresentationFramework.luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Luna.NormalColor.xaml", UriKind.Relative))) as ResourceDictionary; break;
                    case "Aero": resource = Application.LoadComponent((new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Aero.normalcolor.xaml", UriKind.Relative))) as ResourceDictionary; break;
                    case "lmetallic": resource = Application.LoadComponent((new Uri("/PresentationFramework.luna;component/themes/luna.metallic.xaml", UriKind.Relative))) as ResourceDictionary; break;
                    case "lhomestead": resource = Application.LoadComponent((new Uri("/PresentationFramework.luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.homestead.xaml", UriKind.Relative))) as ResourceDictionary; break;
                    case "Royale": resource = Application.LoadComponent((new Uri("/PresentationFramework.Royale;component/themes/Royale.normalcolor.xaml", UriKind.Relative))) as ResourceDictionary; break;
                    case "classic": resource = Application.LoadComponent((new Uri("/PresentationFramework.classic;component/themes/classic.xaml", UriKind.Relative))) as ResourceDictionary; break;
                    case "Aerolite": resource = Application.LoadComponent((new Uri("/PresentationFramework.aerolite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/aerolite.normalcolor.xaml", UriKind.Relative))) as ResourceDictionary; break;
                    case "Aero2": resource = Application.LoadComponent((new Uri("/PresentationFramework.Aero2,, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/aero2.normalcolor.xaml", UriKind.Relative))) as ResourceDictionary; break;

                    //从Wpftheme 控件抽取的xaml里读取
                    default: resource = Application.LoadComponent(new Uri(@"Themes\" + theme + ".xaml", UriKind.Relative)) as ResourceDictionary; break;
                }

                if (resource != null)
                {//要配合APP.xaml看清主题到底是MergedDictionaries[0]还是第N个
                    if (Application.Current.Resources.MergedDictionaries.Count > 0)
                    {
                        Application.Current.Resources.MergedDictionaries.Clear();
                    }
                    Application.Current.Resources.MergedDictionaries.Add(resource);
                }
            }
        }
    }
}
