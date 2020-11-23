using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpWPF.Model
{
    public class ScrapySampleModel : ObservableObject
    {
        private string _Url;

        public string Url
        {
            get { return _Url; }
            set { Set(ref _Url, value); }
        }

    }
}
