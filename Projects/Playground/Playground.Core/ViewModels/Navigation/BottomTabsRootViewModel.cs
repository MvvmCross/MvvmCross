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

            NavigateCommand = new MvxAsyncCommand<Type>(async (viewModelType) =>
            {
                if (viewModelType == typeof(BottomTab1ViewModel))
                {
                    await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(BottomTab1ViewModel)));
                    return;
                }
                if (viewModelType == typeof(BottomTab2ViewModel))
                {
                    await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(BottomTab2ViewModel)));
                    return;
                }
                if (viewModelType == typeof(BottomTab3ViewModel))
                {
                    await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(BottomTab3ViewModel)));
                    return;
                }
            });
        }

        public IMvxAsyncCommand<Type> NavigateCommand { get; private set; }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<BottomTab1ViewModel>());
            tasks.Add(NavigationService.Navigate<BottomTab2ViewModel>());
            tasks.Add(NavigationService.Navigate<BottomTab3ViewModel>());
            await Task.WhenAll(tasks);
        }
    }
}
