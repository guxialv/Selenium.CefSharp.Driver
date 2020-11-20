using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CefSharpWPF.Handlers
{
    public class LifeSpanHandler : ILifeSpanHandler
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        private static string GetWindowTitle(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            var sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public LifeSpanHandler()
        {
        }

        bool ILifeSpanHandler.OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            //NOTE: DevTools popups DO NOT trigger OnBeforePopup.

            //Set newBrowser to null unless your attempting to host the popup in a new instance of ChromiumWebBrowser
            //This option is typically used in WPF. This example demos using IWindowInfo.SetAsChild
            //Older branches likely still have an example of this method if you choose to go down that path.
            newBrowser = null;


            return true;
        }

        void ILifeSpanHandler.OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            if (!browser.IsDisposed && browser.IsPopup)
            {
                var windowTitle = GetWindowTitle(browser.GetHost().GetWindowHandle());

                //CEF doesn't currently provide an option to determine if the new Popup is
                //DevTools so we use a hackyworkaround to check the Window Title.
                //DevTools is hosted in it's own popup, we don't perform any action here
                if (windowTitle != "DevTools")
                {
                    var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

                    chromiumWebBrowser.Dispatcher.Invoke(() =>
                    {
                        var owner = Window.GetWindow(chromiumWebBrowser);

                        if (owner != null && owner.Content == browserControl)
                        {
                            owner.Show();
                        }
                    });
                }
            }
        }

        bool ILifeSpanHandler.DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return false;
        }

        void ILifeSpanHandler.OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            if (!browser.IsDisposed && browser.IsPopup)
            {
                //DevTools is hosted in it's own popup, we don't perform any action here
                if (!browser.MainFrame.Url.Equals("devtools://devtools/devtools_app.html"))
                {
                    var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

                    chromiumWebBrowser.Dispatcher.Invoke(() =>
                    {
                        var owner = Window.GetWindow(chromiumWebBrowser);

                        if (owner != null && owner.Content == browserControl)
                        {
                            owner.Close();
                        }
                    });
                }
            }
        }
    }
}
