using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Core.Pages
{
    [MvxModalPresentation(WrapInNavigationPage = true, Title = "Nested")]
    public partial class NestedModalPage : MvxContentPage<NestedModalViewModel>
    {
        public NestedModalPage()
        {
            InitializeComponent();
        }
    }
}
