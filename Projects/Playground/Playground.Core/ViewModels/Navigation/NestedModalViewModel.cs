// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class NestedModalViewModel : MvxNavigationViewModel
    {
        public NestedModalViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService)
            : base(logFactory, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(() => NavigationService.Close(this));

            ShowTabsCommand = new MvxAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());
        }

        public IMvxAsyncCommand ShowTabsCommand { get; }

        public IMvxAsyncCommand CloseCommand { get; }
    }
}
