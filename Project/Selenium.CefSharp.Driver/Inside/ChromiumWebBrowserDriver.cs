using System;
using System.Threading;
using OpenQA.Selenium;
using CefSharp.Wpf;
using CefSharp;
using System.Windows;

namespace Selenium.CefSharp.Driver.Inside
{
    class ChromiumWebBrowserDriver : ICefSharpBrowser
    {
        //readonly dynamic _webBrowserExtensions;

        //public WindowsAppFriend App => (WindowsAppFriend)AppVar.App;

        public ChromiumWebBrowser Browser { get; }

        public CefSharpFrameDriver MainFrame { get; }

        public CefSharpFrameDriver CurrentFrame { get; set; }

        //public Size Size
        //{
        //    get
        //    {
        //        if (IsWPF)
        //        {
        //            var size = this.Dynamic().RenderSize;
        //            return new Size((int)(double)size.Width, (int)(double)size.Height);
        //        }
        //        return new WindowControl(this.AppVar).Size;
        //    }
        //}

        //bool IsWPF
        //{
        //    get
        //    {
        //        var finder = App.Type<TypeFinder>()();
        //        var wpfType = (AppVar)finder.GetType("CefSharp.Wpf.ChromiumWebBrowser");
        //        var t = this.Dynamic().GetType();
        //        var isWPF = !wpfType.IsNull && (bool)wpfType["IsAssignableFrom", new OperationTypeInfo(typeof(Type).FullName, typeof(Type).FullName)]((AppVar)t).Core;
        //        return isWPF;
        //    }
        //}

        public IBrowser BrowserCore => Browser.GetBrowser();

        public IntPtr WindowHandle => IntPtr.Zero;

        internal IJavascriptObjectRepository JavascriptObjectRepository => Browser.JavascriptObjectRepository;

        internal ChromiumWebBrowserDriver(CefSharpDriver driver)
        {
            Browser = driver.Browser;
            MainFrame = CurrentFrame = new CefSharpFrameDriver(driver, null, Browser.GetMainFrame(), new IWebElement[0]);

        }

        public System.Drawing.Point PointToScreen(System.Drawing.Point clientPoint)
        {
            //if (IsWPF)
            //{
            //    var pos = this.Dynamic().PointToScreen(App.Type("System.Windows.Point")((double)clientPoint.X, (double)clientPoint.Y));
            //    return new Point((int)(double)pos.X, (int)(double)pos.Y);
            //}
            //return new WindowControl(AppVar).PointToScreen(clientPoint);

            var pos = Browser.PointFromScreen(new System.Windows.Point(clientPoint.X, clientPoint.Y));

            return new System.Drawing.Point((int)pos.X, (int)pos.Y);

        }

        public void Activate()
        {
            //if (IsWPF)
            //{
            //    var source = System.Windows.Interop.HwndSource.FromVisual(Browser);
            //    new WindowControl(App, (IntPtr)source.Handle).Activate();
            //}
            //else
            //{
            //    new WindowControl(AppVar).Activate();
            //}
            //var win =Window.GetWindow(Browser);
            //win.Activate();
            //this.Browser.Focus();
        }

        public void WaitForLoading()
        {
            while (Browser.IsLoading)
            {
                Thread.Sleep(10);
            }
        }

        internal dynamic GetMainFrame()
            => Browser.GetMainFrame();

        internal void ShowDevTools()
            => Browser.ShowDevTools();

        public void Close()
        {
            Window.GetWindow(Browser)?.Close();
        }
    }
}
