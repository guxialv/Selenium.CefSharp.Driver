using CefSharpWPF.WebScraping;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpWPF
{
    public class WebScrapingElementModel : ViewModelBase
    {
        public BrowserScrapingCommand Command { get; private set; }

        private string _Text;

        public string Text
        {
            get { return _Text; }
            set { Set(ref _Text, value); }
        }

        private string _XPath;

        public string XPath
        {
            get { return _XPath; }
            set { Set(ref _XPath, value); }
        }

        private string _CssSelector;

        public string CssSelector
        {
            get { return _CssSelector; }
            set { Set(ref _CssSelector, value); }
        }

        private string _TagName;

        public string TagName
        {
            get { return _TagName; }
            set { Set(ref _TagName, value); }
        }

        private string _Column;

        public string Column
        {
            get { return _Column; }
            set { Set(ref _Column, value); }
        }

        public WebScrapingElementModel()
        {

        }

        public WebScrapingElementModel(BrowserScrapingCommand command)
        {
            Command = command;
            Text = command.ElementText;
            XPath = command.XPathValue;
            CssSelector = command.CssSelector;
            TagName = command.ElementTagName;
            Column = command.ColumnName;
        }

    }
}
