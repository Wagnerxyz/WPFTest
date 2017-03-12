using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ProductConfig
{
    /// <summary>
    /// Interaction logic for ConfigModifyView.xaml
    /// </summary>
    public partial class ConfigSetupView : UserControl
    {
        private const string SectionName = "Setting";
        private readonly ObservableCollection<IniData> _myData = new ObservableCollection<IniData>();
        public ConfigSetupView()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            LoadSetting();
        }

        private void LoadSetting()
        {
            try
            {
                //List<string> categories = App.ConfigFile.GetCategories();
                //foreach (string category in categories)
                //{
                //List<string> keys = App.ConfigFile.GetKeys(category);
                List<string> keys = App.ConfigFile.GetKeys(SectionName);
                foreach (string key in keys)
                {
                    _myData.Add(new IniData(key, App.ConfigFile.GetIniFileString(SectionName, key, "")));
                }
                //}
                datagrid.ItemsSource = _myData;
            }
            catch (Exception e)
            {
                MessageBox.Show("配置文件有问题，请手动打开修改");
            }

        }

        private void Button_AddRow(object sender, RoutedEventArgs e)
        {
            _myData.Add(new IniData());
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            for (int i = datagrid.SelectedItems.Count - 1; i >= 0; i--)
            {
                _myData.Remove(datagrid.SelectedItem as IniData);
            }

        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            App.ConfigFile.Write(SectionName, null, null);
            foreach (var data in _myData)
            {
                App.ConfigFile.Write(SectionName, data.Key, data.Value);
            }
        }

        private class IniData
        {
            public IniData()
            {
                Key = "请输入设置项名称";
                Value = "请输入值";
            }
            public IniData(string a, string b)
            {
                Key = a;
                Value = b;
            }
            public string Key { get; set; }

            public string Value { get; set; }

        }

    }
}
