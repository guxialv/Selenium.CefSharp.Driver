using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.CefSharp.Driver.Utils
{
    public static class ElementHighLight
    {
        public static void highlight(IJavaScriptExecutor js, CefSharpWebElement element)
        {
            js.ExecuteScript("element = arguments[0];" +
            "original_style = element.getAttribute('style');" +
            "element.setAttribute('style', original_style + \";" +
            " border: 2px solid red;\");" +
            "setTimeout(function(){element.setAttribute('style', original_style);}, 1000);", element);
        }
    }
}
