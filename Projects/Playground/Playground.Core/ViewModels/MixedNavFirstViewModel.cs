// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavFirstViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        
        public MixedNavFirstViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public IMvxAsyncCommand LoginCommand => new MvxAsyncCommand(GotoMasterDetailPage, CanLogin);

        private bool CanLogin()
        {
            return true;
        }

        private async Task GotoMasterDetailPage()
        {
            await _navigationService.Navigate<MixedNavMasterDetailViewModel>();
            await _navigationService.Navigate<MixedNavMasterRootContentViewModel>();
        }
    }
}
