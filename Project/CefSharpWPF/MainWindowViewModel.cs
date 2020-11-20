using CefSharp;
using CefSharp.DevTools.DOM;
using CefSharp.Wpf;
using CefSharpWPF.WebScraping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using OpenQA.Selenium;
using Selenium.CefSharp.Driver;
using Selenium.CefSharp.Driver.Inside;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CefSharpWPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _AddressEditable;

        public string AddressEditable
        {
            get { return _AddressEditable; }
            set { Set(ref _AddressEditable, value); }
        }

        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { Set(ref _Address, value); }
        }

        private ObservableCollection<WebElementModel> _WebElements;

        public ObservableCollection<WebElementModel> WebElements
        {
            get { return _WebElements; }
            set { Set(ref _WebElements, value); }
        }


        public ObservableCollection<WebScrapingElementModel> WebScrapingElements { get; set; }


        public CefSharpDriver CefSharpDriver { get; private set; }

        public ICommand GoCommand { get; private set; }

        public ICommand FindAllElementCommand { get; private set; }
        public ICommand InJectJSCommand { get; private set; }
        public ICommand DevToolsCommand { get; private set; }

        public MainWindowViewModel()
        {
            Address = AddressEditable = @"https://www.jd.com";
            WebElements = new ObservableCollection<WebElementModel>();
            WebScrapingElements = new ObservableCollection<WebScrapingElementModel>();
            GoCommand = new RelayCommand(ExecuteGoCommand);
            FindAllElementCommand = new RelayCommand(ExecuteFindAllElement);
            InJectJSCommand = new RelayCommand(ExecuteInJectJSCommand);
            DevToolsCommand = new RelayCommand(ExecuteDevToolsCommand);
        }

        public void InitCefSharpDriver(ChromiumWebBrowser browser)
        {
            CefSharpDriver = new CefSharpDriver(browser);
            CefSharpDriver.Browser.ConsoleMessage += Browser_ConsoleMessage;
        }

        private void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            if (e.Level == LogSeverity.Warning &&
               BrowserScrapingCommandUtils.TryParse(e.Message, out BrowserScrapingCommand cmd) &&
               cmd.Command == "click")
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                WebScrapingElements.Add(new WebScrapingElementModel(cmd))
                );
            }
        }

        private void ExecuteGoCommand()
        {
            Address = AddressEditable;
            Keyboard.ClearFocus();
        }

        private void ExecuteFindAllElement()
        {
            var frames = CefSharpDriver.FindElementsByXPath("*");
            foreach (var frame in frames.Cast<CefSharpWebElement>())
            {
                FindElements(frame);
            }
        }

        private void FindElements(CefSharpWebElement webElement)
        {
            var webmodel = new WebElementModel(webElement);
            //webmodel.XPath = (string)CefSharpDriver.ExecuteScript(ElementFinder.GetElementXPath(), webElement);
            WebElements.Add(webmodel);

            var webElements = webElement.FindElementsByXPath("*");

            foreach (var web in webElements.Cast<CefSharpWebElement>())
            {
                FindElements(web);
            }
        }

        private async void ExecuteInJectJSCommand()
        {
            var json = File.ReadAllText(@"Javascript\json2.js");
            var elementSearch = File.ReadAllText(@"Javascript\WebScraping.js");

            CefSharpDriver.ExecuteScript2(json);
            CefSharpDriver.ExecuteScript2(elementSearch);
        }


        private void ExecuteDevToolsCommand()
        {
            CefSharpDriver.Browser.ShowDevTools();
        }
    }

    public class WebElementModel : ViewModelBase
    {
        private int _Id;

        public int Id
        {
            get { return _Id; }
            set { Set(ref _Id, value); }
        }


        private string _Text;

        public string Text
        {
            get { return _Text; }
            set { Set(ref _Text, value); }
        }

        private string _TagName;

        public string TagName
        {
            get { return _TagName; }
            set { Set(ref _TagName, value); }
        }

        private string _XPath;

        public string XPath
        {
            get { return _XPath; }
            set { Set(ref _XPath, value); }
        }

        public CefSharpWebElement WebElement;
        public WebElementModel()
        {

        }

        public WebElementModel(CefSharpWebElement webElement)
        {
            WebElement = webElement;
            TagName = webElement.TagName;
            //Text = webElement.Text;
        }
    }
}
