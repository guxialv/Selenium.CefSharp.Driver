using OpenQA.Selenium;
using System;
using System.Drawing;
using Selenium.CefSharp.Driver.InTarget;
using System.Threading;
using CefSharp;

namespace Selenium.CefSharp.Driver.Inside
{
    class CefSharpWindowBrowser : ICefSharpBrowser
    {
        //WindowControl _core;

        public IntPtr WindowHandle { get; }

        //public WindowsAppFriend App => _core.App;

        //public Size Size => _core.Size;

        public CefSharpFrameDriver MainFrame { get; }

        public CefSharpFrameDriver CurrentFrame { get; set; }

        public IBrowser BrowserCore { get; }

        public CefSharpWindowBrowser(CefSharpDriver driver, IntPtr window, IBrowser browser)
        {
            WindowHandle = window;
            //_core = new WindowControl((WindowsAppFriend)browser.App, GetWindow(window, GW_CHILD));
            BrowserCore = browser;
            MainFrame = CurrentFrame = new CefSharpFrameDriver(driver, null, FrameFinder.GetMainFrame(browser), new IWebElement[0]);
        }

        public void Activate()
        {
            //=> _core.Activate(); 
            throw new NotImplementedException();
        }

        public Point PointToScreen(Point clientPoint)
        {
            //=> _core.PointToScreen(clientPoint);
            throw new NotImplementedException();
        }

        public void WaitForLoading()
        {
            while (BrowserCore.IsLoading)
            {
                Thread.Sleep(10);
            }
        }

        public void Close()
        {
            //=> new WindowControl(App, WindowHandle).Close();
            throw new NotImplementedException();
        }
    }
}
