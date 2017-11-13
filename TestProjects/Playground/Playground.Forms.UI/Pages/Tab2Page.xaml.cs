using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.Pages
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false)]
    public partial class Tab2Page : MvxContentPage<Tab2ViewModel>
    {
        public Tab2Page()
        {
            InitializeComponent();
        }
    }
}
