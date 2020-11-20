using CefSharp;
using OpenQA.Selenium;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Selenium.CefSharp.Driver.Inside
{
    class CefSharpFrameDriver : IJavaScriptExecutor
    {
        readonly JavaScriptAdaptor _javaScriptAdaptor;

        public IFrame Frame { get; private set; }
        public IJavascriptObjectRepository JavascriptObjectRepository => CefSharpDriver.JavascriptObjectRepository;

        public Size Size => FrameElements.Any() ? FrameElements.Last().Size : CefSharpDriver.CurrentBrowser.Size;

        internal string Url
        {
            get => (string)ExecuteScript("return window.location.href;");
            set
            {
                Frame.LoadUrl(value);
                WaitForLoading();
            }
        }

        internal CefSharpDriver CefSharpDriver { get; }

        internal CefSharpFrameDriver ParentFrame { get; }

        internal IWebElement[] FrameElements { get; }

        internal string Title => (string)ExecuteScript("return document.title;");

        internal CefSharpFrameDriver(CefSharpDriver cefSharpDriver, CefSharpFrameDriver parentFrame, IFrame frame, IWebElement[] frameElement)
        {
            ParentFrame = parentFrame;
            _javaScriptAdaptor = new JavaScriptAdaptor(this);
            CefSharpDriver = cefSharpDriver;
            Frame = frame;
            FrameElements = frameElement;
            // _cotnrolAccessor = new CotnrolAccessor(this);
        }

        public object ExecuteScript(string script, params object[] args)
            => _javaScriptAdaptor.ExecuteScript(script, args);
        public object ExecuteScript2(string script, params object[] args)
    => _javaScriptAdaptor.ExecuteScript2(script, args);

        public object ExecuteAsyncScript(string script, params object[] args)
            => _javaScriptAdaptor.ExecuteAsyncScript(script, args);

        public void WaitForLoading()
            => CefSharpDriver.CurrentBrowser.WaitForLoading();

        public Point PointToScreen(Point clientPoint)
        {
            var offset = new Point();
            foreach (var e in FrameElements)
            {
                offset.Offset(e.Location.X, e.Location.Y);
            }
            clientPoint.Offset(offset.X, offset.Y);
            return CefSharpDriver.CurrentBrowser.PointToScreen(clientPoint);
        }

        public void Activate()
        {
            CefSharpDriver.CurrentBrowser.Activate();
        }

        public IWebElement CreateWebElement(int id)
        {
            return new CefSharpWebElement(this, id);
        }


        public Screenshot GetScreenshot()
        {
            return GetScreenShot(new Point(0, 0), CefSharpDriver.CurrentBrowser.Size);
        }

        public override bool Equals(object obj)
        {
            var target = obj as CefSharpFrameDriver;
            if (target == null) return false;
            return this.Frame.Identifier.Equals(target.Frame.Identifier);
        }

        public override int GetHashCode()
        {
            return (int)this.Frame.Identifier;
        }

        internal Screenshot GetScreenShot(Point location, Size size)
        {
            CefSharpDriver.CurrentBrowser.Activate();
            using (var bmp = new Bitmap((int)size.Width, (int)size.Height))
            using (var g = Graphics.FromImage(bmp))
            {
                var upLeft = CefSharpDriver.CurrentBrowser.PointToScreen(location);
                g.CopyFromScreen(upLeft, new Point(0, 0), bmp.Size);
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    return new Screenshot(Convert.ToBase64String(ms.ToArray()));
                }
            }
        }
    }
}
