using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Core.Pages
{
    [MvxMasterDetailPagePresentation]
    public partial class SplitDetailPage : MvxContentPage<SplitDetailViewModel>
    {
        public SplitDetailPage()
        {
            InitializeComponent();
        }
    }
}
