
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SystemColorsSwatch
{
    public class SolidColorBrushKeyToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length >= 2 &&
                values[0] is ResourceKey &&
                values[1] is FrameworkElement &&
                parameter is ColorToStringConversionMode)
            {
                ResourceKey key = (ResourceKey)values[0];
                FrameworkElement fe = (FrameworkElement)values[1];
                object resource = fe.FindResource(key);
                var converterMode = (ColorToStringConversionMode)parameter;

                if (resource is SolidColorBrush)
                {
                    SolidColorBrush brush = (SolidColorBrush)resource;
                    Color color = brush.Color;

                    switch (converterMode)
                    {
                        case ColorToStringConversionMode.Hex:
                            return color.ToString();
                        case ColorToStringConversionMode.RGB:
                            return "" + color.R + ", " + color.G + ", " + color.B + 
                                (color.A == (byte)255 ? "" : " (A=" + color.A + ")");
                        default:
                            Debug.Fail("Invalid SolidColorBrushKeyToStringConverterMode value.",
                                "Supported values are SolidColorBrushKeyToStringConverterMode.Hex and SolidColorBrushKeyToStringConverterMode.RGB");
                            return null;    // should never hit this.
                    }
                }
                else
                {
                    Debug.Fail("Found matching resource but it is not of type SolidColorBrush", "Found resource with key " +
                        key + " on FrameworkElement " + fe + ", but it is not of type SolidColorBrush.  This converter only works when the " + 
                        "supplied key indicates a SolidColorBrush resource");
                    return null;
                }
            }
            else
            {
                throw new ArgumentException("values[0] must be a ResourceKey and values[1] must be a FrameworkElement, " +
                                            "and parameter must be a SolidColorBrushKeyToStringConverterMode.");
            }
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Enumerates different ways we can represent a Color as a String.
    /// (Only Hex and RGB are needed for SystemColorSwatch)
    /// </summary>
    public enum ColorToStringConversionMode
    {
        Hex,
        RGB
    }
}