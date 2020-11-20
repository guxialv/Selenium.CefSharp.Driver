using Selenium.CefSharp.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CefSharpWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainWindowViewModel;
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            this.DataContext = new MainWindowViewModel();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ViewModel.CefSharpDriver = new CefSharpDriver(browser);
            }
        }

        private void Browser_Initialized(object sender, EventArgs e)
        {
            ViewModel.CefSharpDriver = new CefSharpDriver(browser);
        }

        private void OnTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void OnTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }
    }
}
