using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Selenium.CefSharp.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public CefSharpDriver CefSharpDriver;

        public ICommand GoCommand { get; private set; }

        public ICommand FindAllElementCommand { get; private set; }

        public MainWindowViewModel()
        {
            Address = @"https://blog.csdn.net/lanwilliam/article/details/79640954";
            GoCommand = new RelayCommand(ExecuteGoCommand);
            FindAllElementCommand = new RelayCommand(ExecuteFindAllElement);
        }

        private void ExecuteGoCommand()
        {
            Address = AddressEditable;
            Keyboard.ClearFocus();
        }

        private void ExecuteFindAllElement()
        {
            var result = CefSharpDriver.FindElementsByXPath("*");
        }
    }
}
