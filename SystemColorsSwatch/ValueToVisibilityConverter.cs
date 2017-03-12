using System.Windows.Data;
using System.Windows;

namespace SystemColorsSwatch
{
    /// <summary>
    /// Given an object, returns Visibility.Collapsed if that object is null or if it is an empty string and
    /// returns Visibility.Visible otherwise.
    /// </summary>
    class ValueToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
                return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;
            else
                return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
