using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;


namespace WpfTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : Window
    {
        Assembly assembly;
        private string _namespace;

        public Window1()
        {
            InitializeComponent();
            Type type = this.GetType();
            assembly = type.Assembly;
            _namespace = type.Namespace;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // Get the current button.
            Button cmd = (Button)e.OriginalSource;

            // Create an instance of the window named
            // by the current button.

            Window win = (Window)assembly.CreateInstance(
                _namespace + "." + cmd.Content);

            // Show the window.
            if (win != null)
            {
                win.ShowDialog();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(windowtb.Text))
            {
                try
                {
                    Window win = (Window)assembly.CreateInstance(_namespace + "." + windowtb.Text);

                    // Show the window.
                    win.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("«Î ‰»Î∞°£°");
            }

        }
    }
}