// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SharedElementRootChildViewModel : BaseViewModel
    {
        public override Task Initialize()
        {
            Items = new MvxObservableCollection<ListItemViewModel>
            {
                new ListItemViewModel { Id = 1, Title = "title one Fragment" },
                new ListItemViewModel { Id = 2, Title = "title two Activity" },
                new ListItemViewModel { Id = 3, Title = "title three Fragment" },
                new ListItemViewModel { Id = 4, Title = "title four Activity" },
                new ListItemViewModel { Id = 5, Title = "title five Fragment" }
            };

            return base.Initialize();
        }

        private MvxObservableCollection<ListItemViewModel> _items;
        public MvxObservableCollection<ListItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private ListItemViewModel _selectedItem;

        public SharedElementRootChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }

        public ListItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public void SelectItemExecution(ListItemViewModel item)
        {
            SelectedItem = item;

            if (item.Id % 2 == 0)
            {
                NavigationService.Navigate<SharedElementSecondViewModel>();
            }
            else
            {
                NavigationService.Navigate<SharedElementSecondChildViewModel>();
            }
        }
    }
}
