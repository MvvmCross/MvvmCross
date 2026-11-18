// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Commands;
using MvvmCross.Hosting;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class NavigationCloseViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public NavigationCloseViewModel(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
        }

        public IMvxAsyncCommand OpenChildThenCloseThisCommand => new MvxAsyncCommand(CloseThisAndOpenChildAsync);

        public IMvxAsyncCommand TryToCloseNewViewModelCommand => new MvxAsyncCommand(TryToCloseNewViewModelAsync);

        private async Task CloseThisAndOpenChildAsync()
        {
            await _mvxNavigationService.Navigate<SecondChildViewModel>();
            await _mvxNavigationService.Close(this);
        }

        private Task TryToCloseNewViewModelAsync()
        {
            return _mvxNavigationService.Close(MvxHost.Current!.Services.GetRequiredService<SecondChildViewModel>());
        }
    }
}
