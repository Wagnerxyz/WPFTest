using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfTest
{
    /// <summary>
    /// Attached behavior that keeps the window on the screen
    /// </summary>
    public static class WindowService
    {
        /// <summary>
        /// KeepOnScreen Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty EscapeClosesWindowProperty = DependencyProperty.RegisterAttached(
           "EscapeClosesWindow",
           typeof(bool),
           typeof(WindowService),
           new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnEscapeClosesWindowChanged)));

        /// <summary>
        /// Gets the EscapeClosesWindow property.  This dependency property 
        /// indicates whether or not the escape key closes the window.
        /// </summary>
        /// <param name="d"><see cref="DependencyObject"/> to get the property from</param>
        /// <returns>The value of the EscapeClosesWindow property</returns>
        public static bool GetEscapeClosesWindow(DependencyObject d)
        {
            return (bool)d.GetValue(EscapeClosesWindowProperty);
        }

        /// <summary>
        /// Sets the EscapeClosesWindow property.  This dependency property 
        /// indicates whether or not the escape key closes the window.
        /// </summary>
        /// <param name="d"><see cref="DependencyObject"/> to set the property on</param>
        /// <param name="value">value of the property</param>
        public static void SetEscapeClosesWindow(DependencyObject d, bool value)
        {
            d.SetValue(EscapeClosesWindowProperty, value);
        }

        /// <summary>
        /// Handles changes to the EscapeClosesWindow property.
        /// </summary>
        /// <param name="d"><see cref="DependencyObject"/> that fired the event</param>
        /// <param name="e">A <see cref="DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        private static void OnEscapeClosesWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window target = (Window)d;
            if (target != null)
            {
                target.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(Window_PreviewKeyDown);
            }
        }

        /// <summary>
        /// Handle the PreviewKeyDown event on the window
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="KeyEventArgs"/> that contains the event data.</param>
        private static void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Window target = (Window)sender;

            // If this is the escape key, close the window
            if (e.Key == Key.Escape)
                target.Close();
        }
    }
}
