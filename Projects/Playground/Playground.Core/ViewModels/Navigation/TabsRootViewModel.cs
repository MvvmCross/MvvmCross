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
    public class TabsRootViewModel : MvxNavigationViewModel
    {
        public TabsRootViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
            ShowTabsRootBCommand = new MvxAsyncCommand(() => NavigationService.Navigate<TabsRootBViewModel>());
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; }

        public IMvxAsyncCommand ShowTabsRootBCommand { get; }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<Tab1ViewModel, string>("test"));
            tasks.Add(NavigationService.Navigate<Tab2ViewModel>());
            tasks.Add(NavigationService.Navigate<Tab3ViewModel>());
            return Task.WhenAll(tasks);
        }

        private int _itemIndex;

        public int ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                if (_itemIndex == value) return;
                _itemIndex = value;
                Log.LogTrace("Tab item changed to {ItemIndex}", _itemIndex);
                RaisePropertyChanged(() => ItemIndex);
            }
        }
    }
}
