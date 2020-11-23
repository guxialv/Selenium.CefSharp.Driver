using System.ComponentModel;

namespace Selenium.CefSharp.Driver
{
    public class CrawlItem
    {
        private string name;

        private string xpath;
        private bool _isSelected;

        public CrawlItem()
        {
            SampleData1 = "";
            IsEnabled = true;
        }

        /// <summary>
        ///     属性名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                }
            }
        }
        public CrawlType CrawlType { get; set; }


        public SelectorFormat Format { get; set; }

        public string XPath
        {
            get { return xpath; }
            set
            {
                if (xpath != value)
                {
                    xpath = value;
                }
            }
        }

        public bool IsEnabled { get; set; }

        [Browsable(false)]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                }
            }
        }


        public string SampleData1 { get; set; }


        public void CopyFrom(CrawlItem item)
        {
            Name = item.Name;
            XPath = item.XPath;
            CrawlType = item.CrawlType;
            IsEnabled = item.IsEnabled;
            Format = item.Format;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}