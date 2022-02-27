// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.UI.Pages
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
            if (Parent is FlyoutPage md)
            {
                md.FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
                md.IsPresented = !md.IsPresented;
            }
        }
    }
}
