using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Core.Pages
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false)]
    public partial class Tab3Page : MvxContentPage<Tab3ViewModel>
    {
        public Tab3Page()
        {
            InitializeComponent();
        }
    }
}
