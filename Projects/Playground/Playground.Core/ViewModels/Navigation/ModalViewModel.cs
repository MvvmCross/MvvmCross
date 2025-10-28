// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ModalViewModel : MvxNavigationViewModel
    {
        private int _count;

        public ModalViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowTabsCommand = new MvxAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());

            CloseCommand = new MvxAsyncCommand(() => NavigationService.Close(this));

            ShowNestedModalCommand = new MvxAsyncCommand(async () =>
            {
                await NavigationService.Navigate<ModalViewModel>();
                Count++;
            });
            ShowWindowCommand = new MvxAsyncCommand(() => NavigationService.Navigate<WindowViewModel>());
        }

        public IMvxAsyncCommand ShowTabsCommand { get; }

        public IMvxAsyncCommand CloseCommand { get; }

        public IMvxAsyncCommand ShowNestedModalCommand { get; }
        public IMvxAsyncCommand ShowWindowCommand { get; }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
    }
}
