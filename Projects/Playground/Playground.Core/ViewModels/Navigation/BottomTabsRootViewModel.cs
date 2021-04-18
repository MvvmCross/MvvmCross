// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class BottomTabsRootViewModel : MvxNavigationViewModel
    {
        public BottomTabsRootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
        }
        
        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>
            {
                NavigationService.Navigate<BottomTab1ViewModel>(),
                NavigationService.Navigate<BottomTab2ViewModel>(),
                NavigationService.Navigate<BottomTab3ViewModel>()
            };
            return Task.WhenAll(tasks);
        }
    }
}
