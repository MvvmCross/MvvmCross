using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Core.Pages
{
    public partial class RootPage : MvxContentPage<RootViewModel>
    {
        public RootPage()
        {
            InitializeComponent();
        }
    }
}
