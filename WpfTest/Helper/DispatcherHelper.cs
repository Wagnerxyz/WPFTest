using System;
using System.Windows.Threading;

namespace WpfTest.Helper
{
    public static class DispatcherHelper
    {
        /// <summary>
        ///     Gets a reference to the UI thread's dispatcher, after the
        ///     GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize()
        ///     method has been called on the UI thread.
        /// </summary>
        public static Dispatcher UIDispatcher { get; private set; }

        /// <summary>
        ///     Executes an action on the UI thread. If this method is called from the UI
        ///     thread, the action is executed immendiately. If the method is called from
        ///     another thread, the action will be enqueued on the UI thread's dispatcher
        ///     and executed asynchronously.
        ///     For additional operations on the UI thread, you can get a reference to the
        ///     UI thread's dispatcher thanks to the property GalaSoft.MvvmLight.Threading.DispatcherHelper.UIDispatcher
        /// </summary>
        /// <param name="action">The action that will be executed on the UI thread.</param>
        public static void CheckBeginInvokeOnUI(Action action)
        {
            if (UIDispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                UIDispatcher.BeginInvoke(action, new object[0]);
            }
        }

        /// <summary>
        ///     This method should be called once on the UI thread to ensure that the
        ///     GalaSoft.MvvmLight.Threading.DispatcherHelper.UIDispatcher
        ///     property is initialized.
        ///     In a Silverlight application, call this method in the Application_Startup
        ///     event handler, after the MainPage is constructed.
        ///     In WPF, call this method on the static App() constructor.
        /// </summary>
        public static void Initialize()
        {
            if ((UIDispatcher == null) || !UIDispatcher.Thread.IsAlive)
            {
                UIDispatcher = Dispatcher.CurrentDispatcher;
            }
        }

        /// <summary>
        ///     Invokes an action asynchronously on the UI thread.
        /// </summary>
        /// <param name="action">The action that must be executed.</param>
        /// <returns></returns>
        public static DispatcherOperation RunAsync(Action action)
        {
            return UIDispatcher.BeginInvoke(action, new object[0]);
        }
    }
}