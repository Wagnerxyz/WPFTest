using System.Windows.Data;

namespace SystemColorsSwatch
{
    /// <summary>
    /// Converts an object instance to a string by calling that object's ToString() method.
    /// </summary>
    class ToStringConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
