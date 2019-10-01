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

        public override async void ViewAppearing()
        {
            await ShowInitialViewModels();
            base.ViewAppearing();
        }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<MixedNavTab1ViewModel>());
            tasks.Add(NavigationService.Navigate<MixedNavTab2ViewModel>());
            //tasks.Add(NavigationService.Navigate<Tab1ViewModel, string>("test"));
            //tasks.Add(NavigationService.Navigate<Tab2ViewModel>());
            //tasks.Add(NavigationService.Navigate<Tab3ViewModel>());
            await Task.WhenAll(tasks);
        }
    }
}
