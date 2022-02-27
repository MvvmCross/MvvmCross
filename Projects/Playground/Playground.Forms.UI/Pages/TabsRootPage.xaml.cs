// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;

namespace Playground.Forms.UI.Pages
{
    [MvxTabbedPagePresentation(TabbedPosition.Root, NoHistory = false)]
    public partial class TabsRootPage : MvxTabbedPage<TabsRootViewModel>
    {
        public TabsRootPage()
        {
            InitializeComponent();
        }

        private bool _firstTime = true;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_firstTime)
            {
                //ViewModel.ShowInitialViewModelsCommand.Execute();
                ViewModel.ShowInitialViewModelsCommand.ExecuteAsync(null);
                _firstTime = false;
            }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
        }
    }
}
