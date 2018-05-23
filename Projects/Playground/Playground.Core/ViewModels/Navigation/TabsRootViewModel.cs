﻿// Licensed to the .NET Foundation under one or more agreements.
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
    public class TabsRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IMvxLog _log;

        public TabsRootViewModel(IMvxNavigationService navigationService, IMvxLogProvider logProvider)
        {
            _log = logProvider.GetLogFor(nameof(TabsRootViewModel));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
            ShowTabsRootBCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootBViewModel>());
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        public IMvxAsyncCommand ShowTabsRootBCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(_navigationService.Navigate<Tab1ViewModel, string>("test"));
            tasks.Add(_navigationService.Navigate<Tab2ViewModel>());
            tasks.Add(_navigationService.Navigate<Tab3ViewModel>());
            await Task.WhenAll(tasks);
        }

        private int _itemIndex;

        public int ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                if (_itemIndex == value) return;
                _itemIndex = value;
                _log.Trace("Tab item changed to {0}", _itemIndex.ToString());
                RaisePropertyChanged(() => ItemIndex);
            }
        }
    }
}
