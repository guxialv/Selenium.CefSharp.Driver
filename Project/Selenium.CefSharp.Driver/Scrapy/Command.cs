using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpWPF.WebScraping
{

    public class ScrapyCommand
    {
        public string Command { get; set; }
        public string Caller { get; set; }
        public string CommandId { get; set; }
        public string CommandValue { get; set; }
        public string CssSelector { get; set; }
        public string XPathValue { get; set; }
        public string ElementId { get; set; }
        public string ElementText { get; set; }
        public string ElementTagName { get; set; }
        public string Relative_X { get; set; }
        public string Relative_Y { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        public override string ToString()
        {
            return $"{Command}, {CommandValue}, {XPathValue}";
        }


    }
}
