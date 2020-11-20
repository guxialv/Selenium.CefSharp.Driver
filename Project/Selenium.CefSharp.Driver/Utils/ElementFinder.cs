using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Selenium.CefSharp.Driver.Inside
{
    public static class ElementFinder
    {
        internal static IWebElement FindElementFromDocument(IJavaScriptExecutor js, By by)
        {
            var text = by.ToString();
            var script = "";
            if (text.StartsWith("By.Id:"))
            {
                script = $"return document.getElementById('{text.Substring("By.Id:".Length).Trim()}');";
            }
            if (text.StartsWith("By.Name:"))
            {
                script = $"return document.getElementsByName('{text.Substring("By.Name:".Length).Trim()}')[0];";
            }
            if (text.StartsWith("By.ClassName[Contains]:"))
            {
                script = $"return document.getElementsByClassName('{text.Substring("By.ClassName[Contains]:".Length).Trim()}')[0];";
            }
            if (text.StartsWith("By.CssSelector:"))
            {
                script = $"return document.querySelector(\"{text.Substring("By.CssSelector:".Length).Trim()}\");";
            }
            if (text.StartsWith("By.TagName:"))
            {
                script = $"return document.getElementsByTagName('{text.Substring("By.TagName:".Length).Trim()}')[0];";
            }
            if (text.StartsWith("By.XPath:"))
            {
                script = $"return window.__seleniumCefSharpDriver.getElementsByXPath('{text.Substring("By.XPath:".Length).Trim()}')[0];";
            }
            if (text.StartsWith("By.LinkText:"))
            {
                script = $"return window.__seleniumCefSharpDriver.getElementsByXPath('//a[text()=\"{text.Substring("By.LinkText:".Length).Trim()}\"]')[0];";
            }
            if (text.StartsWith("By.PartialLinkText:"))
            {
                script = $"return window.__seleniumCefSharpDriver.getElementsByXPath('//a[contains(., \"{text.Substring("By.PartialLinkText:".Length).Trim()}\")]')[0];";
            }
            if (!(js.ExecuteScript(script) is CefSharpWebElement result))
            {
                throw new NoSuchElementException($"Element not found: {text}");
            }
            return result;
        }

        internal static ReadOnlyCollection<IWebElement> FindElementsFromDocument(IJavaScriptExecutor js, By by)
        {
            var text = by.ToString();
            var script = "";
            if (text.StartsWith("By.Id:"))
            {
                script = $"return document.querySelectorAll('[id=\"{text.Substring("By.Id:".Length).Trim()}\"]');";
            }
            if (text.StartsWith("By.Name:"))
            {
                script = $"return document.getElementsByName('{text.Substring("By.Name:".Length).Trim()}');";
            }
            if (text.StartsWith("By.ClassName[Contains]:"))
            {
                script = $"return document.getElementsByClassName('{text.Substring("By.ClassName[Contains]:".Length).Trim()}');";
            }
            if (text.StartsWith("By.CssSelector:"))
            {
                script = $"return document.querySelectorAll(\"{text.Substring("By.CssSelector:".Length).Trim()}\");";
            }
            if (text.StartsWith("By.TagName:"))
            {
                script = $"return document.getElementsByTagName('{text.Substring("By.TagName:".Length).Trim()}');";
            }
            if (text.StartsWith("By.XPath:"))
            {
                script = $"return window.__seleniumCefSharpDriver.getElementsByXPath('{text.Substring("By.XPath:".Length).Trim()}');";
            }
            if (text.StartsWith("By.LinkText:"))
            {
                script = $"return window.__seleniumCefSharpDriver.getElementsByXPath('//a[text()=\"{text.Substring("By.LinkText:".Length).Trim()}\"]');";
            }
            if (text.StartsWith("By.PartialLinkText:"))
            {
                script = $"return window.__seleniumCefSharpDriver.getElementsByXPath('//a[contains(., \"{text.Substring("By.PartialLinkText:".Length).Trim()}\")]');";
            }
            if (!(js.ExecuteScript(script) is ReadOnlyCollection<IWebElement> result))
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
            return result;
        }

        internal static IWebElement FindElementFromElement(IJavaScriptExecutor js, int id, By by)
        {
            var text = by.ToString();
            var script = "";
            if (text.StartsWith("By.Id:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.querySelector('[id=""{text.Substring("By.Id:".Length).Trim()}""]');";
            }
            if (text.StartsWith("By.Name:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.querySelector('[name=""{text.Substring("By.Name:".Length).Trim()}""]');";
            }
            if (text.StartsWith("By.ClassName[Contains]:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.getElementsByClassName('{text.Substring("By.ClassName[Contains]:".Length).Trim()}')[0];";
            }
            if (text.StartsWith("By.CssSelector:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.querySelector(""{text.Substring("By.CssSelector:".Length).Trim()}"");";
            }
            if (text.StartsWith("By.TagName:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.getElementsByTagName('{text.Substring("By.TagName:".Length).Trim()}')[0];";
            }
            if (text.StartsWith("By.XPath:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return window.__seleniumCefSharpDriver.getElementsByXPath('{text.Substring("By.XPath:".Length).Trim()}', element)[0];";
            }
            if (text.StartsWith("By.LinkText:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return window.__seleniumCefSharpDriver.getElementsByXPath('.//a[text()=""{text.Substring("By.LinkText:".Length).Trim()}""]', element)[0];";
            }
            if (text.StartsWith("By.PartialLinkText:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return window.__seleniumCefSharpDriver.getElementsByXPath('.//a[contains(., ""{text.Substring("By.PartialLinkText:".Length).Trim()}"")]', element)[0];";
            }
            if (!(js.ExecuteScript(script) is CefSharpWebElement result))
            {
                throw new NoSuchElementException($"Element not found: {text}");
            }
            return result;
        }

        internal static ReadOnlyCollection<IWebElement> FindElementsFromElement(IJavaScriptExecutor js, int id, By by)
        {
            var text = by.ToString();
            var script = "";
            if (text.StartsWith("By.Id:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.querySelectorAll('[id=""{text.Substring("By.Id:".Length).Trim()}""]');";
            }
            if (text.StartsWith("By.Name:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.querySelectorAll('[name=""{text.Substring("By.Name:".Length).Trim()}""]');";
            }
            if (text.StartsWith("By.ClassName[Contains]:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.getElementsByClassName('{text.Substring("By.ClassName[Contains]:".Length).Trim()}');";
            }
            if (text.StartsWith("By.CssSelector:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.querySelectorAll(""{text.Substring("By.CssSelector:".Length).Trim()}"");";
            }
            if (text.StartsWith("By.TagName:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return element.getElementsByTagName('{text.Substring("By.TagName:".Length).Trim()}');";
            }
            if (text.StartsWith("By.XPath:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return window.__seleniumCefSharpDriver.getElementsByXPath('{text.Substring("By.XPath:".Length).Trim()}', element);";
            }
            if (text.StartsWith("By.LinkText:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return window.__seleniumCefSharpDriver.getElementsByXPath('.//a[text()=""{text.Substring("By.LinkText:".Length).Trim()}""]', element);";
            }
            if (text.StartsWith("By.PartialLinkText:"))
            {
                script = $@"
const element = window.__seleniumCefSharpDriver.getElementByEntryId({id});
return window.__seleniumCefSharpDriver.getElementsByXPath('.//a[contains(., ""{text.Substring("By.PartialLinkText:".Length).Trim()}"")]', element);";
            }
            if (!(js.ExecuteScript(script) is ReadOnlyCollection<IWebElement> result))
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
            return result;
        }



        internal static void HighlightElement(IWebElement webElement, IJavaScriptExecutor js, By by)
        {
            var element = webElement.FindElement(by);
            js.ExecuteScript(
            @"
                element = arguments[0];
                original_style = element.getAttribute('style');
                element.setAttribute('style', original_style + ""; background: yellow; border: 2px solid red;"");
                setTimeout(function(){
                    element.setAttribute('style', original_style);
                }, 300);

           ", element);
        }

        public static string GetElementXPath()
        {
            return
 @"
function getPathTo(element) {
    if (element === document.body)
        return '/html/' + element.tagName.toLowerCase();

    var ix = 0;
    var siblings = element.parentNode.childNodes;
    for (var i = 0; i < siblings.length; i++) {
        var sibling = siblings[i];
        if (sibling === element)
        {
            return getPathTo(element.parentNode) + '/' + element.tagName.toLowerCase() + '[' + (ix + 1) + ']';
        }
        if (sibling.nodeType === 1 && sibling.tagName === element.tagName)
            ix++;
    }
}

var element = arguments[0];
var xpath = '';
xpath = getPathTo(element);
return xpath;
";
        }
    }
}
