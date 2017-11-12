using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class ChildPage : MvxContentPage<ChildViewModel>
    {
        public ChildPage()
        {
            InitializeComponent();
        }
    }
}
