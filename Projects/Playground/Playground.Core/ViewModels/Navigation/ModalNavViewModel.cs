// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ModalNavViewModel : MvxNavigationViewModel
    {
        public ModalNavViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));

            ShowChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ChildViewModel>());

            ShowNestedModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<NestedModalViewModel>());
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }

        public IMvxAsyncCommand ShowNestedModalCommand { get; private set; }
    }
}
