using CefSharpWPF.WebScraping;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpWPF.Model
{
    public class ScrapingItemModel : ObservableObject
    {
        public ScrapyCommand Command { get; private set; }


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

        public ScrapingItemModel()
        {

        }

        public ScrapingItemModel(ScrapyCommand command)
        {
            Command = command;
            Text = command.ElementText;
            XPath = command.XPathValue;
            CssSelector = command.CssSelector;
            TagName = command.ElementTagName;
        }
    }

    public class ScrapyItemModel : ObservableObject
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
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


        public List<ScrapyCommand> Cmds { get; private set; }

        public ScrapyItemModel()
        {
            Cmds = new List<ScrapyCommand>();
        }

    }
}
