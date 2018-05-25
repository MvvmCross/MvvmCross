// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class OverrideAttributeViewModel : MvxViewModel
    {
        public OverrideAttributeViewModel()
        {
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));

            ShowTabsCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootViewModel>());

            ShowSecondChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SecondChildViewModel>());
        }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowSecondChildCommand { get; private set; }
    }
}
