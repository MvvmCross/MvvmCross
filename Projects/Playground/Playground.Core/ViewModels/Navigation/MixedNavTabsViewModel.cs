// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavTabsViewModel : MvxNavigationViewModel
    {
        public MixedNavTabsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }

        public override async ValueTask ViewAppearing()
        {
            await ShowInitialViewModels().ConfigureAwait(false);
            await base.ViewAppearing().ConfigureAwait(false);
        }

        private Task ShowInitialViewModels()
        {
            var tasks = new[]
            {
                NavigationService.Navigate<MixedNavTab1ViewModel>().AsTask(),
                NavigationService.Navigate<MixedNavTab2ViewModel>().AsTask(),
                //NavigationService.Navigate<Tab1ViewModel, string>("test").AsTask(),
                //NavigationService.Navigate<Tab2ViewModel>().AsTask(),
                //NavigationService.Navigate<Tab3ViewModel>().AsTask()
            };

            return Task.WhenAll(tasks);
        }
    }
}
