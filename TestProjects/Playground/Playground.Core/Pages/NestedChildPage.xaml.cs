using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Core.Pages
{
    public partial class NestedChildPage : MvxContentPage<NestedChildViewModel>
    {
        public NestedChildPage()
        {
            InitializeComponent();
        }
    }
}
