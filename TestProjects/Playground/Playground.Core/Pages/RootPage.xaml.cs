using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Core.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class RootPage : MvxContentPage<RootViewModel>
    {
        public RootPage()
        {
            InitializeComponent();
        }
    }
}
