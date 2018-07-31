// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitDetailNavViewModel : MvxNavigationViewModel
    {
        public SplitDetailNavViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            MainMenuCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<MixedNavFirstViewModel>());
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));
        }

        public IMvxAsyncCommand MainMenuCommand { get; private set; }
        public IMvxAsyncCommand CloseCommand { get; private set; }

    }
}
