// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Master, WrapInNavigationPage = false, NoHistory = true)]
    public partial class MixedNavMasterDetailPage : MvxContentPage<MixedNavMasterDetailViewModel>
    {
        public MixedNavMasterDetailPage()
        {
            InitializeComponent();

#if __IOS__
            if(Parent is FlyoutPage master)
                master.IsGestureEnabled = false;
#endif
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.SelectedMenu))
                {
                    if (Parent is FlyoutPage master)
                    {
                        //master.MasterBehavior = MasterBehavior.Popover;
                        master.IsPresented = !master.IsPresented;
                    }
                }
            };
        }
    }
}
