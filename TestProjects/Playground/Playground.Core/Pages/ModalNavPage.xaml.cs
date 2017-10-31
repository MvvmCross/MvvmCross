using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Core.Pages
{
    [MvxModalPresentation(WrapInNavigationPage = true)]
    public partial class ModalNavPage : MvxContentPage<ModalNavViewModel>
    {
        public ModalNavPage()
        {
            InitializeComponent();
        }
    }
}
