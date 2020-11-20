using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpWPF.WebScraping
{

    public class BrowserScrapingCommand
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
        public string ColumnName { get; set; }

        public override string ToString()
        {
            return $"{Command}, {CommandValue}, {XPathValue}";
        }


    }

    public class BrowserScrapingCommandUtils
    {
        static string lastCommandId;
        const string javaScript_RdCommand = "RecorderCommand:";

        public static bool TryParse(string json, out BrowserScrapingCommand browserCmd)
        {
            browserCmd = null;

            if (json.Contains(javaScript_RdCommand) == false)
            {
                return false;
            }

            var cmd = json.Substring(json.IndexOf(javaScript_RdCommand) + javaScript_RdCommand.Length);
            var jsonCommand = cmd;//.Replace("\\\"", "\"").Replace("\\\"", "\"").TrimEnd('\"');

            browserCmd = JsonConvert.DeserializeObject<BrowserScrapingCommand>(jsonCommand);

            if (lastCommandId == browserCmd.CommandId)
            {
                return false;
            }

            lastCommandId = browserCmd.CommandId;

            return browserCmd != null;
        }
    }
}
