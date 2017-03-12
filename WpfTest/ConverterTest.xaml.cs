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
    /// ConverterTest.xaml 的交互逻辑
    /// </summary>
    public partial class ConverterTest : Window
    {
        List<PersonName> persons = new List<PersonName>();
        Student stu;
        public ConverterTest()
        {
            InitializeComponent();
            var obj = new { Firstname = "Wagern", LastName = "wang" };
            Loaded += ConverterTest_Loaded;


        }

        private void ConverterTest_Loaded(object sender, RoutedEventArgs e)
        {
            persons.Add(new PersonName("Willa", "Cather"));
            persons.Add(new PersonName("Isak", "Dinesen"));
            persons.Add(new PersonName("Victor", "Hugo"));
            persons.Add(new PersonName("Jules", "Verne"));
            textBox1.DataContext = persons[1];

            textbox2.DataContext = new { Baseprice = 15.5, Newprice = 20.3 };

            #region 绑定方法一
            //stu = new Student();
            //Binding bind = new Binding("Text");
            //bind.Source = textBox3;
            //BindingOperations.SetBinding(stu, Student.NameProperty, bind);
            #endregion
            #region 绑定方法二
            stu = new Student();
            Binding bind = new Binding("Name");
            bind.Source = stu;
            textBox3.SetBinding(TextBox.TextProperty, bind);
            #endregion

        }


        public class PersonName
        {
            public PersonName(string first, string last)
            {
                FirstName = first;
                LastName = last;
            }

            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(stu.Name);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((e.OriginalSource as FrameworkElement).Name);
        }
        
    }
}
