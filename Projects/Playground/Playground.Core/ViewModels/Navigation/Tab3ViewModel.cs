// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab3ViewModel : MvxNavigationViewModel
    {
        public Tab3ViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowRootViewModelCommand = new MvxAsyncCommand(() => NavigationService.Navigate<RootViewModel>());

            CloseViewModelCommand = new MvxAsyncCommand(() => NavigationService.Close(this));

            ShowPageOneCommand = new MvxCommand(() => NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab1ViewModel))));
        }

        public IMvxAsyncCommand ShowRootViewModelCommand { get; }

        public IMvxAsyncCommand CloseViewModelCommand { get; }

        public IMvxCommand ShowPageOneCommand { get; }
    }
}
