using CefSharp;
using OpenQA.Selenium;
using System;
using System.Drawing;
using System.Linq;

namespace Selenium.CefSharp.Driver.Inside
{
    class CefSharpFrameDriver : IJavaScriptExecutor
    {
        readonly JavaScriptAdaptor _javaScriptAdaptor;
        //readonly Func<AppVar> _frameGetter;
        //readonly CotnrolAccessor _cotnrolAccessor;

        //public WindowsAppFriend App => (WindowsAppFriend)AppVar.App;

        public IFrame Frame { get; private set; }
        public IJavascriptObjectRepository JavascriptObjectRepository => CefSharpDriver.JavascriptObjectRepository;

        //  public Size Size => FrameElements.Any() ? FrameElements.Last().Size : CefSharpDriver.CurrentBrowser.Size;

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

        public object ExecuteAsyncScript(string script, params object[] args)
            => _javaScriptAdaptor.ExecuteAsyncScript(script, args);

        public void WaitForLoading()
            => CefSharpDriver.CurrentBrowser.WaitForLoading();

        public Point PointToScreen(Point clientPoint)
        {
            var offset = new Point();
            foreach (var e in FrameElements)
            {
                offset.Offset(e.Location);
            }
            clientPoint.Offset(offset);
            //return CefSharpDriver.CurrentBrowser.PointToScreen(clientPoint);
            throw new NotImplementedException();
        }

        public void Activate()
        {
            //=> CefSharpDriver.CurrentBrowser.Activate();
            throw new NotImplementedException();
        }

        public IWebElement CreateWebElement(int id)
            => new CefSharpWebElement(this, id);

        public Screenshot GetScreenshot()
        {
            //=> _cotnrolAccessor.GetScreenShot(new Point(0, 0), Size);
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            var target = obj as CefSharpFrameDriver;
            if (target == null) return false;
            return this.Frame.Identifier.Equals(target.Frame.Identifier);
        }

        public override int GetHashCode() => (int)this.Frame.Identifier;
    }
}
