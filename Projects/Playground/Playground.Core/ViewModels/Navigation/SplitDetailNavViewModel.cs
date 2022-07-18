// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitDetailNavViewModel : MvxNavigationViewModel
    {
        public SplitDetailNavViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            MainMenuCommand = new MvxAsyncCommand(() => NavigationService.Navigate<MixedNavFirstViewModel>());
            CloseCommand = new MvxAsyncCommand(() => NavigationService.Close(this));
        }

        public IMvxAsyncCommand MainMenuCommand { get; }
        public IMvxAsyncCommand CloseCommand { get; }

    }
}
