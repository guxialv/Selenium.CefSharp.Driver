using CefSharp;
using System;
using System.Drawing;

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

        Size Size { get; }

        void Activate();
        Point PointToScreen(Point clientPoint);
    }
}
