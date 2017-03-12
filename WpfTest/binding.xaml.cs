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
using WpfTest.Model;

namespace WpfTest
{
    /// <summary>
    /// bingding.xaml 的交互逻辑
    /// </summary>
    public partial class binding : Window
    {
        public binding()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Student stu = new Student();

            Binding binding = new Binding("Text") { Source = textBox1, Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(stu, Student.NameProperty, binding);
            //stu.SetValue(Student.NameProperty, textBox1.Text);
            //textBox2.Text = (string)stu.GetValue(Student.NameProperty);
            Binding bd = new Binding("Name") { Source=stu,Mode=BindingMode.TwoWay};
            BindingOperations.SetBinding(textBox2, TextBox.TextProperty, bd);


        }
    }
}
