// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class BottomTab1ViewModel : MvxNavigationViewModel
    {
        public BottomTab1ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            OpenBottomTab2Command = new MvxAsyncCommand(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(BottomTab2ViewModel))));

            OpenBottomTab3Command = new MvxAsyncCommand(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(BottomTab3ViewModel))));
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }

        public IMvxAsyncCommand OpenBottomTab2Command { get; private set; }

        public IMvxAsyncCommand OpenBottomTab3Command { get; private set; }
    }
}
