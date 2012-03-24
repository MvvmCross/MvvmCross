using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitterSearch.Core.ViewModels;
using TwitterSearch.UI.Win8.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace TwitterSearch.UI.Win8.Views
{
    public class BaseHomePage
        : LayoutAwarePage
    {
        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }

    public sealed partial class HomePage : BaseHomePage
    {
        public HomePage()
        {
            this.InitializeComponent();
        }
    }
}
