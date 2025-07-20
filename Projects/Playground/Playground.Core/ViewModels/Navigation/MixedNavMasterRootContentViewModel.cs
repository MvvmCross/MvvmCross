// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.Models;

namespace Playground.Core.ViewModels
{
    public class MixedNavMasterRootContentViewModel : MvxNavigationViewModel
    {
        public MixedNavMasterRootContentViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowModalCommand = new MvxAsyncCommand(() => NavigationService.Navigate<ModalNavViewModel>());
            ShowChildCommand = new MvxAsyncCommand(() => NavigationService.Navigate<ChildViewModel, SampleModel>(new SampleModel("Hey", 1.23m)));
        }

        public IMvxAsyncCommand ShowModalCommand { get; }
        public IMvxAsyncCommand ShowChildCommand { get; }
    }
}
