// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavMasterDetailViewModel : MvxNavigationViewModel
    {
        private MenuItem _menuItem;
        private IMvxAsyncCommand<MenuItem> _onSelectedChangedCommand;

        public class MenuItem
        {
            public string Title { get; set; }

            public string Description { get; set; }
            public Type ViewModelType { get; set; }
        }

        public MixedNavMasterDetailViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            Menu = new[] {
                new MenuItem { Title = "Root", Description = "The root page", ViewModelType = typeof(MixedNavMasterRootContentViewModel) },
                new MenuItem { Title = "Tabs", Description = "Tabbed detail page", ViewModelType = typeof(MixedNavTabsViewModel)},
                new MenuItem { Title = "Result", Description = "Open detail page with result", ViewModelType = typeof(MixedNavResultDetailViewModel)},
            };
        }

        public IEnumerable<MenuItem> Menu { get; set; }

        public MenuItem SelectedMenu {
            get => _menuItem;
            set {
                if (SetProperty(ref _menuItem, value))
                    OnSelectedChangedCommand.Execute(value);
            }
        }

        private IMvxAsyncCommand<MenuItem> OnSelectedChangedCommand {
            get {
                return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new MvxAsyncCommand<MenuItem>(async (item) => 
                {
                    if (item == null)
                        return;

                    var vmType = item.ViewModelType;
                    await NavigationService.Navigate(vmType);
                }));
            }
        }
    }
}
