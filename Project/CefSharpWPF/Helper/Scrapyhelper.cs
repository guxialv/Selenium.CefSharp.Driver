using CefSharpWPF.Model;
using CefSharpWPF.WebScraping;
using Newtonsoft.Json;
using Selenium.CefSharp.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpWPF.Helper
{
    public class Scrapyhelper
    {
        public static Scrapyhelper Instance = new Scrapyhelper();

        private const string SCRAPY_COMMAND = "scrapy";
        private const string SCRAPY_COMMAND_PREFIX = "ScrapyCommand:";

        private string _LastCommandId;
        private ScrapingItemModel _PreScrapingItem;

        private List<ScrapyItemModel> _ScrapyItems;

        private Scrapyhelper()
        {

        }

        public void Initialize()
        {
            _PreScrapingItem = null;
            _ScrapyItems = new List<ScrapyItemModel>();
        }


        private bool TryParse(string json, out ScrapyCommand browserCmd)
        {
            browserCmd = null;

            if (json.Contains(SCRAPY_COMMAND_PREFIX) == false)
            {
                return false;
            }

            var cmd = json.Substring(json.IndexOf(SCRAPY_COMMAND_PREFIX) + SCRAPY_COMMAND_PREFIX.Length);
            var jsonCommand = cmd;//.Replace("\\\"", "\"").Replace("\\\"", "\"").TrimEnd('\"');

            browserCmd = JsonConvert.DeserializeObject<ScrapyCommand>(jsonCommand);

            if (_LastCommandId == browserCmd.CommandId)
            {
                return false;
            }

            _LastCommandId = browserCmd.CommandId;

            return browserCmd != null;
        }

        public ScrapyItemModel Scrapy(ScrapyCommand cmd)
        {
            var scrapingItem = new ScrapingItemModel(cmd);
            if (_PreScrapingItem != null)
            {
                var scrapyItem = new ScrapyItemModel();
                scrapyItem.Name = "Field" + (_ScrapyItems.Count + 1);
                scrapyItem.Cmds.Add(_PreScrapingItem.Command);
                scrapyItem.Cmds.Add(scrapingItem.Command);
                scrapyItem.XPath = XPath.GetMaxCompareXPath(_PreScrapingItem.XPath, scrapingItem.XPath);
                _ScrapyItems.Add(scrapyItem);

                _PreScrapingItem = null;
                return scrapyItem;
            }
            else
            {
                _PreScrapingItem = scrapingItem;
                return null;
            }
        }

        public ScrapyItemModel Scrapy(string json)
        {
            if (TryParse(json, out ScrapyCommand cmd) &&
                SCRAPY_COMMAND.Equals(cmd.Command, StringComparison.OrdinalIgnoreCase))
            {
                return Scrapy(cmd);
            }

            return null;
        }
    }
}
