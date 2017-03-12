using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplication1"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplication1;assembly=WpfApplication1"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ClockUserCtrl/>
    ///
    /// </summary>
    public class ClockUserCtrl : Control
    {
        public static readonly RoutedUICommand SpeakCommand = new RoutedUICommand("Speak", "Speak", typeof(ClockUserCtrl));
        static ClockUserCtrl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClockUserCtrl), new FrameworkPropertyMetadata(typeof(ClockUserCtrl)));
            CommandBinding commandBinding =
              new CommandBinding(SpeakCommand, new ExecutedRoutedEventHandler(ExecuteSpeak),
              new CanExecuteRoutedEventHandler(CanExecuteSpeak));
        }

 

        private static void CanExecuteSpeak(object sender, CanExecuteRoutedEventArgs arg)
        {
            ClockUserCtrl clock = sender as ClockUserCtrl;
            arg.CanExecute = (clock != null);
        }
        private static void ExecuteSpeak(object sender, ExecutedRoutedEventArgs arg)
        {
            ClockUserCtrl clock = sender as ClockUserCtrl;
            if (clock != null)
            {
                clock.SpeakTheTime();
            }
        }
        private void SpeakTheTime()
        {
            DateTime localTime = this.Time.ToLocalTime();
            string textToSpeak = "现在时刻," +
                localTime.ToShortDateString() + "," +
                localTime.ToShortTimeString() +
                ",星期" + (int)localTime.DayOfWeek;

           // this.speecher.SpeakAsync(textToSpeak);
        }
        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(ClockUserCtrl), new PropertyMetadata(DateTime.Now,new PropertyChangedCallback(TimePropertyChangedCallback)));

        private static void TimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d!=null&d is ClockUserCtrl)
            {
                ClockUserCtrl clock = d as ClockUserCtrl;
                clock.OnTimeUpdated((DateTime)e.OldValue, (DateTime)e.NewValue);
            }
        }
        public static readonly RoutedEvent TimeUpdatedEvent =
           EventManager.RegisterRoutedEvent("TimeUpdated",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(ClockUserCtrl));

        [Description("日期或时间被更新后发生")]
        public event RoutedPropertyChangedEventHandler<DateTime> TimeUpdated
        {
            add
            {
                this.AddHandler(TimeUpdatedEvent, value);
            }
            remove
            {
                this.RemoveHandler(TimeUpdatedEvent, value);
            }
        }
        protected virtual void OnTimeUpdated(DateTime oldValue, DateTime newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime> arg =
                new RoutedPropertyChangedEventArgs<DateTime>(oldValue, newValue, TimeUpdatedEvent);
            this.RaiseEvent(arg);

        }
    }
}
