using CefSharp;
using CefSharp.DevTools.DOM;
using CefSharp.DevTools.Overlay;
using CefSharp.Wpf;
using CefSharpWPF.Helper;
using CefSharpWPF.Model;
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

        private ScrapyItemModel _SelectedScrapyItem;

        public ScrapyItemModel SelectedScrapyItem
        {
            get { return _SelectedScrapyItem; }
            set
            {
                Set(ref _SelectedScrapyItem, value);
                ScrapyItemSelectionChanged(value);
            }
        }


        public ObservableCollection<ScrapySampleModel> ScrapySamples { get; set; }
        public ObservableCollection<ScrapyItemModel> ScrapyItems { get; set; }
        public CefSharpDriver CefSharpDriver { get; private set; }

        public ICommand GoCommand { get; private set; }

        public ICommand FindAllElementCommand { get; private set; }
        public ICommand InJectJSCommand { get; private set; }
        public ICommand DevToolsCommand { get; private set; }


        public MainWindowViewModel()
        {
            Address = AddressEditable = @"https://news.cnblogs.com/n/digg";
            ScrapySamples = new ObservableCollection<ScrapySampleModel>();
            ScrapyItems = new ObservableCollection<ScrapyItemModel>();
            GoCommand = new RelayCommand(ExecuteGoCommand);
            FindAllElementCommand = new RelayCommand(ExecuteFindAllElement);
            InJectJSCommand = new RelayCommand(ExecuteInJectJSCommand);
            DevToolsCommand = new RelayCommand(ExecuteDevToolsCommand);

            ScrapySamples.Add(new ScrapySampleModel() { Url = Address });
            Scrapyhelper.Instance.Initialize();
        }

        public void InitCefSharpDriver(ChromiumWebBrowser browser)
        {
            CefSharpDriver = new CefSharpDriver(browser);
            CefSharpDriver.Browser.ConsoleMessage += Browser_ConsoleMessage;
        }

        private void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            if (e.Level == LogSeverity.Warning)
            {
                var scrapyItem = Scrapyhelper.Instance.Scrapy(e.Message);
                if (scrapyItem != null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() => ScrapyItems.Add(scrapyItem));
                }
            }
        }

        private void ExecuteGoCommand()
        {
            Address = AddressEditable;
            Keyboard.ClearFocus();
        }

        private void ExecuteFindAllElement()
        {
            try
            {

                var elements = CefSharpDriver.FindElementsByXPath("//*[@id=\"J_cate\"]/ul/li[10]/a[2]");

                foreach (var ele in elements.Cast<CefSharpWebElement>())
                {
                    ele.HighLight();
                }
                Console.WriteLine(elements.Count);
                //var frames = CefSharpDriver.FindElementsByXPath("*");
                //foreach (var frame in frames.Cast<CefSharpWebElement>())
                //{
                //    FindElements(frame);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ScrapyItemSelectionChanged(ScrapyItemModel scrapyItem)
        {
            if (scrapyItem == null)
            {
                return;
            }
            var elements = CefSharpDriver.FindElementsByXPath(scrapyItem.XPath);

            foreach (var ele in elements.Cast<CefSharpWebElement>())
            {
                ele.HighLight();
            }
        }

        //private void FindElements(CefSharpWebElement webElement)
        //{
        //   // var webmodel = new WebElementModel(webElement);
        //    //webmodel.XPath = (string)CefSharpDriver.ExecuteScript(ElementFinder.GetElementXPath(), webElement);
        //   //WebElements.Add(webmodel);

        //    var webElements = webElement.FindElementsByXPath("*");

        //    foreach (var web in webElements.Cast<CefSharpWebElement>())
        //    {
        //        FindElements(web);
        //    }
        //}

        private async void ExecuteInJectJSCommand()
        {
            var json = File.ReadAllText(@"Javascript\json2.js");
            var elementSearch = File.ReadAllText(@"Javascript\scrapy.js");
            var highlight = File.ReadAllText(@"Javascript\highlight.pack.js");

            CefSharpDriver.ExecuteScript2(json);
            CefSharpDriver.ExecuteScript2(elementSearch);
            CefSharpDriver.ExecuteScript2(highlight);
        }


        private async void ExecuteDevToolsCommand()
        {
            //var highlightConfig = new HighlightConfig();

            //highlightConfig.ContentColor = ToRGBA(Colors.Red);
            //highlightConfig.ContentColor = ToRGBA(Colors.Red);
            //highlightConfig.PaddingColor = ToRGBA(Colors.Red);
            //highlightConfig.CssGridColor = ToRGBA(Colors.Red);
            //highlightConfig.MarginColor = ToRGBA(Colors.Red);
            //highlightConfig.ShapeColor = ToRGBA(Colors.Red);

            CefSharpDriver.Browser.ShowDevTools();
            //var client = CefSharpDriver.Browser.GetDevToolsClient();
            //await client.Inspector.EnableAsync();
            //await client.Overlay.SetInspectModeAsync(InspectMode.CaptureAreaScreenshot, highlightConfig);
        }

        private RGBA ToRGBA(Color color)
        {
            return new RGBA() { R = color.R, G = color.G, B = color.B, A = color.A };
        }
    }

}
