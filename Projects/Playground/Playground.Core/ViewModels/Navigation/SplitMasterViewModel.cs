// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitMasterViewModel : MvxNavigationViewModel
    {
        public SplitMasterViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            OpenDetailCommand = new MvxAsyncCommand(() => NavigationService.Navigate<SplitDetailViewModel>());

            OpenDetailNavCommand = new MvxAsyncCommand(() => NavigationService.Navigate<SplitDetailNavViewModel>());

            ShowRootViewModel = new MvxAsyncCommand(() => NavigationService.Navigate<RootViewModel>());
        }

        public string PaneText => "Text for the Master Pane";

        public IMvxAsyncCommand OpenDetailCommand { get; }

        public IMvxAsyncCommand OpenDetailNavCommand { get; }

        public IMvxAsyncCommand ShowRootViewModel { get; }
    }
}
