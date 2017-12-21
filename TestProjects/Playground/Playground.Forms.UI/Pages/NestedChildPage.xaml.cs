using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.Pages
{
    public partial class NestedChildPage : MvxContentPage<NestedChildViewModel>
    {
        public NestedChildPage()
        {
            InitializeComponent();
        }
    }
}
