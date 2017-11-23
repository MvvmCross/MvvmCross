using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.Pages
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Master)]
    public partial class SplitMasterPage : MvxContentPage<SplitMasterViewModel>
    {
        public SplitMasterPage()
        {
            InitializeComponent();
        }

        public void ToggleClicked(object sender, EventArgs e)
        {
            if(Parent is MasterDetailPage md)
            {
                md.MasterBehavior = MasterBehavior.Popover;
                md.IsPresented = !md.IsPresented;
            }
        }
    }
}
