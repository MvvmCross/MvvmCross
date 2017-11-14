using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.Pages
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Root, WrapInNavigationPage =false)]
    public partial class SplitRootPage : MvxMasterDetailPage<SplitRootViewModel>
    {
        public SplitRootPage()
        {
            InitializeComponent();
        }
    }
}
