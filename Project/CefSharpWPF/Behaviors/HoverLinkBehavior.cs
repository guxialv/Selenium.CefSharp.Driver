using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace CefSharpWPF.Behaviors
{
    public class HoverLinkBehavior : Behavior<ChromiumWebBrowser>
    {
        // Using a DependencyProperty as the backing store for HoverLink. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverLinkProperty = DependencyProperty.Register("HoverLink", typeof(string), typeof(HoverLinkBehavior), new PropertyMetadata(string.Empty));

        public string HoverLink
        {
            get { return (string)GetValue(HoverLinkProperty); }
            set { SetValue(HoverLinkProperty, value); }
        }

        public string OutputMessage
        {
            get { return (string)GetValue(OutputMessageProperty); }
            set { SetValue(OutputMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputMessageProperty =
            DependencyProperty.Register("OutputMessage", typeof(string), typeof(HoverLinkBehavior), new PropertyMetadata(string.Empty));



        protected override void OnAttached()
        {
            AssociatedObject.StatusMessage += OnStatusMessageChanged;
            //AssociatedObject.ConsoleMessage += OnConsoleMessageChanged;
        }

        private void OnConsoleMessageChanged(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine($"{e.Level} {e.Message}");
            var chromiumWebBrowser = sender as ChromiumWebBrowser;
            chromiumWebBrowser.Dispatcher.BeginInvoke((Action)(() => OutputMessage = e.Message));
        }

        protected override void OnDetaching()
        {
            AssociatedObject.StatusMessage -= OnStatusMessageChanged;
            // AssociatedObject.ConsoleMessage -= OnConsoleMessageChanged;
        }

        private void OnStatusMessageChanged(object sender, StatusMessageEventArgs e)
        {
            var chromiumWebBrowser = sender as ChromiumWebBrowser;
            chromiumWebBrowser.Dispatcher.BeginInvoke((Action)(() => HoverLink = e.Value));
        }
    }
}
