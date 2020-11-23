using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.CefSharp.Driver
{
    public enum SelectorFormat
    {
        XPath,
        CssSelecor,
    }

    /// <summary>
    /// 爬虫获取数据运行方式
    /// </summary>
    public enum CrawlType
    {
        InnerText,
        InnerHtml,
        OuterHtml,

    }
    /// <summary>
    /// 转换器脚本运行方式
    /// </summary>
    public enum ScriptWorkMode
    {
        List,
        One,
        NoTransform,
    }
}
