using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CustomControls
{
    /// <summary>
    ///     传入Action执行时进度条显示，可向Do方法传入两个Action第二个作为回调方法。
    /// </summary>
    /// [TemplateVisualState( Name = VisualStates.StateIdle, GroupName = VisualStates.GroupBusyStatus )]
    [TemplateVisualState(Name = VisualStates.StateBusy, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateVisible, GroupName = VisualStates.GroupVisibility)]
    [TemplateVisualState(Name = VisualStates.StateHidden, GroupName = VisualStates.GroupVisibility)]
    [StyleTypedProperty(Property = "OverlayStyle", StyleTargetType = typeof (Rectangle))]
    [StyleTypedProperty(Property = "ProgressBarStyle", StyleTargetType = typeof (ProgressBar))]
    [TemplatePart(Name = "PART_btncancel", Type = typeof (Button))]
    [TemplatePart(Name = "PART_progress", Type = typeof (ProgressBar))]
    public sealed class BusyDialog : ContentControl
    {
        #region 执行逻辑

        public void Do(Action action)
        {
            Do(action, null);
        }

        public void Do(Action action, Action<BusyDialogResult> _callback)
        {
            _workThread = new Thread(o => DoActionWithCallBack(action, _callback));
            _workThread.Start();
        }

        private void DoActionWithCallBack(Action ac, Action<BusyDialogResult> callback)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() => IsBusy = true));
                ac();
                if (callback != null)
                {
                    var result = new BusyDialogResult {exitcode = 0, exp = null};
                    callback(result);
                }
            }
            catch (ThreadAbortException e)
            {
                var result = new BusyDialogResult {exitcode = 1, exp = e};
                if (callback != null)
                    callback(result);
            }
            catch (Exception e)
            {
                var result = new BusyDialogResult {exitcode = 2, exp = e};
                if (callback != null)
                    callback(result);
            }
            finally
            {
                Dispatcher.BeginInvoke(new Action(() => IsBusy = false));
            }
        }

        /// <summary>
        ///     取消执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel(object sender, RoutedEventArgs e)
        {
            _workThread.Abort();
        }

        public class BusyDialogResult
        {
            public int exitcode { get; set; }
            public Exception exp { get; set; }
        }

        #endregion

        #region Private Members

        /// <summary>
        ///     Timer used to delay the initial display and avoid flickering.
        /// </summary>
        private readonly DispatcherTimer _displayAfterTimer = new DispatcherTimer();

        private Thread _workThread;

        private Button btncancel;
        private ProgressBar progress;

        #endregion //Private Members

        #region Constructors

        static BusyDialog()
        {
            #region 通过命令

            //CommandManager.RegisterClassCommandBinding(typeof (BusyDialog), new CommandBinding(cancelcommand, (s, e) =>
            //    {
            //        var window = s as BusyDialog;
            //        if (window != null)
            //        {
            //            window.tokenSource.Cancel();
            //        }
            //    }));
            //CommandManager.RegisterClassInputBinding(typeof (BusyDialog), new InputBinding(cancelcommand, new MouseGesture(MouseAction.RightClick)));

            #endregion

            DefaultStyleKeyProperty.OverrideMetadata(typeof (BusyDialog),
                new FrameworkPropertyMetadata(typeof (BusyDialog)));
        }

        public BusyDialog()
        {
            _displayAfterTimer.Tick += DisplayAfterTimerElapsed;
        }

        #endregion //Constructors

        #region Base Class Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            // this.ApplyTemplate();
            // DependencyObject d1 = VisualTreeHelper.GetChild(this, 0);
            btncancel = GetTemplateChild("PART_btncancel") as Button;
            progress = GetTemplateChild("PART_progress") as ProgressBar;
            btncancel.Click += Cancel;
            //DataTemplate null 无法解决
            //  DataTemplate contentTemplate = contro.ContentTemplate;
            //  Button btncancel = this.BusyContentTemplate.FindName("PART_btncancel", contro) as Button;
            //  Button btncancel = LogicalTreeHelper.FindLogicalNode(contro, "btncancel") as Button;
            //  Button btncancel = LogicalTreeHelper.FindLogicalNode(d1, "btncancel") as Button;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the BusyContent is visible.
        /// </summary>
        protected bool IsContentVisible { get; set; }

        #endregion //Properties

        #region Dependency Properties

        public static readonly DependencyProperty WorkProperty = DependencyProperty.Register("Work", typeof (Action),
            typeof (BusyDialog), new PropertyMetadata(DoWorkChanged));

        public static readonly DependencyProperty CallBackProperty = DependencyProperty.Register("CallBack",
            typeof (Action<BusyDialogResult>), typeof (BusyDialog), new PropertyMetadata(CallBackChanged));

        public static readonly DependencyProperty AllowCancelProperty = DependencyProperty.Register("AllowCancel",
            typeof (bool), typeof (BusyDialog), new PropertyMetadata(false));

        public Action<BusyDialogResult> CallBack
        {
            get { return (Action<BusyDialogResult>) GetValue(CallBackProperty); }
            set { SetValue(CallBackProperty, value); }
        }


        public Action Work
        {
            get { return (Action) GetValue(WorkProperty); }
            set { SetValue(WorkProperty, value); }
        }


        public bool AllowCancel
        {
            get { return (bool) GetValue(AllowCancelProperty); }
            set { SetValue(AllowCancelProperty, value); }
        }

        #region IsBusy

        /// <summary>
        ///     Identifies the IsBusy dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof (bool),
            typeof (BusyDialog),
            new PropertyMetadata(false, OnIsBusyChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the busy indicator should show.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool) GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        ///     IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="d">BusyDialog that changed its IsBusy.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyDialog) d).OnIsBusyChanged(e);
        }

        /// <summary>
        ///     IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // Go visible now
                    IsContentVisible = true;
                }
                else
                {
                    // Set a timer to go visible
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // No longer visible
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }

            ChangeVisualState(true);
        }

        #endregion //IsBusy

        #region Busy Content

        /// <summary>
        ///     Identifies the BusyContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof (object),
            typeof (BusyDialog),
            new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets a value indicating the busy content to display to the user.
        /// </summary>
        public object BusyContent
        {
            get { return GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        #endregion //Busy Content

        #region Busy Content Template

        /// <summary>
        ///     Identifies the BusyTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof (DataTemplate),
            typeof (BusyDialog),
            new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets a value indicating the template to use for displaying the busy content to the user.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate) GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        #endregion //Busy Content Template

        #region Display After

        /// <summary>
        ///     Identifies the DisplayAfter dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof (TimeSpan),
            typeof (BusyDialog),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        ///     Gets or sets a value indicating how long to delay before displaying the busy content.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan) GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        #endregion //Display After

        #region Overlay Style

        /// <summary>
        ///     Identifies the OverlayStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof (Style),
            typeof (BusyDialog),
            new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets a value indicating the style to use for the overlay.
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style) GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        #endregion //Overlay Style

        #region ProgressBar Style

        /// <summary>
        ///     Identifies the ProgressBarStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof (Style),
            typeof (BusyDialog),
            new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets a value indicating the style to use for the progress bar.
        /// </summary>
        public Style ProgressBarStyle
        {
            get { return (Style) GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        #endregion //ProgressBar Style

        private static void CallBackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void DoWorkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            object t = e.NewValue;
        }

        #endregion //Dependency Properties

        #region 与显示相关的方法

        /// <summary>
        ///     Handler for the DisplayAfterTimer.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        ///     Changes the control's visual state(s).
        /// </summary>
        /// <param name="useTransitions">True if state transitions should be used.</param>
        private void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden,
                useTransitions);
        }

        #endregion //Methods

        #region Unused

        #region Task

        // private CancellationTokenSource tokenSource;
        // private readonly TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();
        //private void Cancel(object sender, RoutedEventArgs e)
        //{
        //    tokenSource.Cancel();
        //}

        //public void DoTask(Action action)
        //{

        //    tokenSource = new CancellationTokenSource();
        //    CancellationToken token = tokenSource.Token;

        //    var sw = new Stopwatch();
        //    IsBusy = true;
        //    Task.Factory.StartNew(() =>
        //        {
        //            sw.Start();
        //            // Thread.Sleep(8000);
        //            while (true)
        //            {
        //                for (int i = 0; i < 1000; i++)
        //                {
        //                    action();
        //                    //     DoWorkCommand();
        //                    //   Console.WriteLine(tb.Text);
        //                    sw.GetHashCode().GetHashCode();
        //                }
        //                token.ThrowIfCancellationRequested();
        //            }


        //        }, token).ContinueWith(ant =>
        //            {
        //                Console.WriteLine(sw.Elapsed);
        //                IsBusy = false;
        //            }, uiThread);
        //}

        //public void RunSynchronously()
        //{
        //    tokenSource = new CancellationTokenSource();
        //    CancellationToken token = tokenSource.Token;
        //    var ExecuteProgressBarTask = new Task(() =>
        //        {
        //            var sw = new Stopwatch();
        //            sw.Start();
        //            // Thread.Sleep(8000);
        //            while (true)
        //            {
        //                for (int i = 0; i < 1000; i++)
        //                {
        //                    // Console.WriteLine(tb.Text);
        //                    sw.GetHashCode().GetHashCode().GetHashCode();
        //                }
        //                token.ThrowIfCancellationRequested();
        //            }

        //            // DoWork();
        //        });
        //    ExecuteProgressBarTask.RunSynchronously();
        //} 

        #endregion

        //public static RoutedCommand cancelcommand = new RoutedCommand("Cancel..", typeof(BusyDialog));
        //public static RoutedCommand CancelCommand
        //{
        //    get { return cancelcommand; }
        //}              
        //public static readonly DependencyProperty DoWorkCommandProperty =
        //    DependencyProperty.Register("DoWorkCommand", typeof(Action), typeof(BusyDialog));

        //public Action DoWorkCommand
        //{
        //    set { SetValue(DoWorkCommandProperty, value); }
        //    get { return (Action)GetValue(DoWorkCommandProperty); }
        //}

        #endregion
    }
}