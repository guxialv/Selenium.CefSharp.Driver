using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selenium.CefSharp.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace UnitTest
{
    [TestClass]
    public class CrawlersTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var html = new HtmlDocument();
            html.LoadHtml(GetHtml("https://news.cnblogs.com/n/digg", out HttpStatusCode code));

            Console.WriteLine(html.DocumentNode);

            var xpath1 = html.DocumentNode.SearchXPath("干一辈子吗", () => true).FirstOrDefault();
            var xpath2 = html.DocumentNode.SearchXPath("敞篷车", () => true).FirstOrDefault();


            //var node1 = html.DocumentNode.SelectSingleNodePlus(xpath1, SelectorFormat.XPath);
            //var node2 = html.DocumentNode.SelectSingleNodePlus(xpath2, SelectorFormat.XPath);

            var diff = XPath.GetMaxCompareXPath(new List<string> { xpath1, xpath2 });

            var nodes = html.DocumentNode.SelectNodes($"{diff}/div[*]");
            var list = new List<string>();
            foreach (var node in nodes)
            {
                list.Add(node.GetDataFromXPath($"{node.ParentNode.XPath}/div[2]/h2[1]/a[1]/#text[1]"));
            }

            var texts = new List<string>();
            nodes.ToList().ForEach(t => texts.Add(t.GetNodeText()));
        }

        public string GetHtml(string url, out HttpStatusCode code,
          string post = null)
        {
            string result = "";
            var helper = new HttpHelper();
            HttpHelper.HttpResponse response;
            code = HttpStatusCode.NotFound;

            var http = new HttpItem();
            response = helper.GetHtml(http, url, post).Result;
            result = response.Html;
            code = response.Code;
            result = JavaScriptAnalyzer.Decode(result);
            //if (IsSuperMode)
            //{
            //    result = JavaScriptAnalyzer.Parse2XML(result);
            //}

            return result;
        }
    }
}
