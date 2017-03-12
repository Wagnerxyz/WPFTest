
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfTest
{
    class ReportTimeEventArgs : RoutedEventArgs
    {
        public ReportTimeEventArgs(System.Windows.RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }
        public DateTime ClickTime { get; set; }
    }

    class TimeButton : Button
    {
        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly System.Windows.RoutedEvent ReportTimeEvent = EventManager.RegisterRoutedEvent(
            "ReportTime", RoutingStrategy.Tunnel, typeof(EventHandler<ReportTimeEventArgs>), typeof(TimeButton));

        // Provide CLR accessors for the event
        public event RoutedEventHandler ReportTime
        {
            add { AddHandler(ReportTimeEvent, value); }
            remove { RemoveHandler(ReportTimeEvent, value); }
        }

     
        protected override void OnClick()
        {
            base.OnClick();

            ReportTimeEventArgs args = new ReportTimeEventArgs(ReportTimeEvent, this);
            args.ClickTime = DateTime.Now;
            this.RaiseEvent(args);
        }
    }
}
