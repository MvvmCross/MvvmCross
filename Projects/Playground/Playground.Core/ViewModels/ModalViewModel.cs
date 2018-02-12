﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ModalViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public ModalViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));

            ShowNestedModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<NestedModalViewModel>());
        }

        public override System.Threading.Tasks.Task Initialize()
        {
            return base.Initialize();
        }

        public void Init()
        {
        }

        public override void Start()
        {
            base.Start();
        }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowNestedModalCommand { get; private set; }
    }
}
