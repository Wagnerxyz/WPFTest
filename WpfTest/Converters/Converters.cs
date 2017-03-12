using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfTest.Converters
{
    public class NameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string name;

            switch ((string)parameter)
            {
                case "FormatLastFirst":
                    name = values[1] + ", " + values[0];
                    break;
                case "FormatNormal":
                default:
                    name = values[0] + " " + values[1];
                    break;
            }

            return name;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            string[] splitValues = ((string)value).Split(' ');
            return splitValues;
        }
    }
    public class QDataColorConvert : IMultiValueConverter
    {
        /// 需传入一组对象，（基础值 比对值）
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double proNum = Math.Round((double)values[1], 2);//目前实时阶段性价格
            double basepronum = Math.Round((double)values[0], 2);//昨收价格

            if (proNum > basepronum)
            {
                return new SolidColorBrush(Color.FromArgb(255, 255, 96, 96));
            }
            else if (proNum < basepronum)
            {
                return new SolidColorBrush(Color.FromArgb(255, 83, 187, 108));
            }
            return new SolidColorBrush(Color.FromArgb(255, 227, 227, 227));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
