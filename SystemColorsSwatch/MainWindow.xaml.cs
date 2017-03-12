using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.Reflection;

namespace SystemColorsSwatch
{
    /// <summary>
    /// Abstracts SystemColors as a swatch to easily view which system colors are available as wrapped WPF Colors and Brushes.
    /// 
    /// SystemColors exposes an assortment of colors extracted from the current Windows system theme (e.g. Aero).
    /// Each unique system color is exposed as both a Color and a Brush stactic property.  For each Color and Brush
    /// there is also a ResourceKey for binding purposes and dynamic change notification.  That is, if we bind
    /// Button.Background to SystemColors.ControlLightBrushKey instead of to SystemColors.ControlLightBrush directly,
    /// then Button.Background automatically gets updated to the new color when the system theme changes.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static MainWindow()
        {
            // Use reflection and LINQ query to determine all of the Brush-related ResourceKeys in SystemColors.
            IEnumerable<PropertyInfo> colorProperties =
                from p in typeof(SystemColors).GetProperties()
                where p.PropertyType == typeof(ResourceKey) &&
                      p.Name.Contains("BrushKey")
                select p;

            // Add these keys to our ObservableCollection so we can bind to them.
            foreach (PropertyInfo p in colorProperties)
            {
                ResourceKey k = p.GetValue(null, null) as ResourceKey;
                SystemColorBrushKeys.Add(k);
            }
        }

        public static ObservableCollection<ResourceKey> SystemColorBrushKeys = new ObservableCollection<ResourceKey>();

    }
}
