using System;
using System.Threading;
using OpenQA.Selenium;
using CefSharp.Wpf;
using CefSharp;
using System.Drawing;

namespace Selenium.CefSharp.Driver.Inside
{
    class ChromiumWebBrowserDriver : ICefSharpBrowser
    {
        public ChromiumWebBrowser Browser { get; }

        public CefSharpFrameDriver MainFrame { get; }

        public CefSharpFrameDriver CurrentFrame { get; set; }

        public Size Size
        {
            get
            {
                var size = this.Browser.RenderSize;
                return new Size((int)size.Width, (int)size.Height);
            }
        }

        public IBrowser BrowserCore => Browser.GetBrowser();

        public IntPtr WindowHandle => IntPtr.Zero;

        internal IJavascriptObjectRepository JavascriptObjectRepository => Browser.JavascriptObjectRepository;

        internal ChromiumWebBrowserDriver(CefSharpDriver driver)
        {
            Browser = driver.Browser;
            MainFrame = CurrentFrame = new CefSharpFrameDriver(driver, null, Browser.GetMainFrame(), new IWebElement[0]);
        }

        public Point PointToScreen(Point clientPoint)
        {
            var pt = Browser.PointFromScreen(new System.Windows.Point(clientPoint.X, clientPoint.Y));

            return new Point((int)pt.X, (int)pt.Y);
        }

        public void Activate()
        {
            System.Windows.Window.GetWindow(Browser)?.Activate();
            this.Browser.Focus();
        }

        public void WaitForLoading()
        {
            while (Browser.IsLoading)
            {
                Thread.Sleep(100);
            }
        }

        internal IFrame GetMainFrame()
            => Browser.GetMainFrame();

        internal void ShowDevTools()
            => Browser.ShowDevTools();

        public void Close()
        {
            System.Windows.Window.GetWindow(Browser)?.Close();
        }
    }
}
