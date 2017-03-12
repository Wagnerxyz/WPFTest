using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace WpfTest.Views
{
    /// <summary>
    ///     Interaction logic for CircularProgressBar.xaml
    /// </summary>
    public partial class CircularProgressBar : Window, INotifyPropertyChanged
    {
        private bool _isOperationFinished = true;

        public CircularProgressBar()
        {
            InitializeComponent();
        }

        public bool IsOperationFinished
        {
            get { return _isOperationFinished; }
            set
            {
                _isOperationFinished = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            IsOperationFinished = false;
            var thread = new Thread(DoWork);
            thread.Start();
        }

        private void DoWork()
        {
            Thread.Sleep(5000);
            IsOperationFinished = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}