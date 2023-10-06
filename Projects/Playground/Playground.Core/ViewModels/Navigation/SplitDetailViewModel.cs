// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitDetailViewModel : MvxNavigationViewModel
    {
        public SplitDetailViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowChildCommand = new MvxAsyncCommand(() => NavigationService.Navigate<SplitDetailNavViewModel>());
            ShowTabsCommand = new MvxAsyncCommand(() => NavigationService.Navigate<TabsRootBViewModel>());
            ShowTabbedChildCommand = new MvxAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());
        }

        public IMvxAsyncCommand ShowChildCommand { get; }
        public IMvxAsyncCommand ShowTabsCommand { get; }
        public IMvxAsyncCommand ShowTabbedChildCommand { get; }

        public string ContentText => "Text for the Content Area";
    }
}
