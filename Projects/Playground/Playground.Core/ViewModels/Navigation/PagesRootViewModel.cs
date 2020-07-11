// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class PagesRootViewModel : MvxNavigationViewModel
    {
        public PagesRootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(
                async () => await ShowInitialViewModels().ConfigureAwait(false));
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            var tasks = new[]
            {
                NavigationService.Navigate<Page1ViewModel>().AsTask(),
                NavigationService.Navigate<Page2ViewModel>().AsTask(),
                NavigationService.Navigate<Page3ViewModel>().AsTask(),
            };
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
