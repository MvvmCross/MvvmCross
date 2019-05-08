// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class BottomTab3ViewModel : MvxNavigationViewModel
    {
        public BottomTab3ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            OpenBottomTab1Command = new MvxAsyncCommand(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(BottomTab1ViewModel))));

            OpenBottomTab2Command = new MvxAsyncCommand(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(BottomTab2ViewModel))));
        }

        public IMvxAsyncCommand OpenBottomTab1Command { get; private set; }

        public IMvxAsyncCommand OpenBottomTab2Command { get; private set; }
    }
}
