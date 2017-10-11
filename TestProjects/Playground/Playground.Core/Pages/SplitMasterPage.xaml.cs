using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Core.Pages
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Master)]
    public partial class SplitMasterPage : MvxContentPage<SplitMasterViewModel>
    {
        public SplitMasterPage()
        {
            InitializeComponent();
        }
    }
}
