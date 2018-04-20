// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SplitRootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowInitialMenuCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitMasterViewModel>());
            ShowDetailCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitDetailViewModel>());
        }

        public IMvxAsyncCommand ShowInitialMenuCommand { get; private set; }

        public IMvxAsyncCommand ShowDetailCommand { get; private set; }
    }
}
