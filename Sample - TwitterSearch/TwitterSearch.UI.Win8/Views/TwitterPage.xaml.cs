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

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace TwitterSearch.UI.Win8.Views
{
    public sealed partial class TwitterPage : LayoutAwarePage
    {
        public TwitterPage()
        {
            this.InitializeComponent();
        }

        public TwitterViewModel ViewModel
        {
            get { return (TwitterViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
