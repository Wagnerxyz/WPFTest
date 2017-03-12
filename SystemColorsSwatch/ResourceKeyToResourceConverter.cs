

using System.Windows.Data;
using System.Windows;
using System;

namespace SystemColorsSwatch
{
    /// <summary>
    /// Takes a FrameworkElement and a ResourceKey, and returns the matching resource from the FrameworkElement's resources.
    /// 
    /// In the DynamicResourceExtension MarkupExtension, ResourceKey is a regular CLR property and not a WPF DependencyProperty.
    /// Therefore, we can't write something like this in XAML:
    ///     <Button Background="{DynamicResource {Binding}}" />
    /// This is also illegal:
    ///     <Button>
    ///         <Button.Background>
    ///             <DynamicResource ResourceKey="{Binding}" />
    ///         </Button.Background>
    ///     </Button>
    ///     
    ///  Therefore, we use a MultiValueConverter instead.  Note that we could have also written our own MarkupExtension, too.
    /// </summary>
    public class ResourceKeyToResourceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length >= 2 &&
                values[0] is ResourceKey &&
                values[1] is FrameworkElement)
            {
                ResourceKey key = (ResourceKey)values[0];
                FrameworkElement fe = (FrameworkElement)values[1];
                return fe.FindResource(key);
            }
            else
            {
                throw new ArgumentException("values[0] must be a ResourceKey and values[1] must be a FrameworkElement!");
            }
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}