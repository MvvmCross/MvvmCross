// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class PagesRootViewModel : MvxNavigationViewModel
    {
        public PagesRootViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<Page1ViewModel>());
            tasks.Add(NavigationService.Navigate<Page2ViewModel>());
            tasks.Add(NavigationService.Navigate<Page3ViewModel>());
            return Task.WhenAll(tasks);
        }
    }
}
