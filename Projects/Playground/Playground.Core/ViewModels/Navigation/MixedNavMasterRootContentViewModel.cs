// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.Models;

namespace Playground.Core.ViewModels
{
    public class MixedNavMasterRootContentViewModel : MvxNavigationViewModel
    {
        public MixedNavMasterRootContentViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>());
            ShowChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ChildViewModel, SampleModel>(new SampleModel { Message = "Hey", Value = 1.23m }));
        }

        public IMvxAsyncCommand ShowModalCommand { get; private set; }
        public IMvxAsyncCommand ShowChildCommand { get; private set; }
    }
}
