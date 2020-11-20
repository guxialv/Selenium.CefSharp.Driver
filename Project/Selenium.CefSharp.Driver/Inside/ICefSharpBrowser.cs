using CefSharp;
using System;

namespace Selenium.CefSharp.Driver.Inside
{
    interface ICefSharpBrowser
    {
        CefSharpFrameDriver MainFrame { get; }
        CefSharpFrameDriver CurrentFrame { get; set; }
        IBrowser BrowserCore { get; }
        IntPtr WindowHandle { get; }
        void WaitForLoading();
        void Close();
    }
}
