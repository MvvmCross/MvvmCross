using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace Playground.Core.ViewModels
{
    public class MixedNavMasterDetailViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private MenuItem _menuItem;
        private IMvxAsyncCommand<MenuItem> _onSelectedChangedCommand;

        public class MenuItem
        {
            public string Title { get; set; }

            public string Description { get; set; }
            public Type ViewModelType { get; set; }
        }

        public MixedNavMasterDetailViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            Menu = new[] {
                new MenuItem { Title = "Root", Description = "The root page", ViewModelType = typeof(MixedNavMasterRootContentViewModel) },
                new MenuItem { Title = "Tabs", Description = "Tabbed detail page", ViewModelType = typeof(MixedNavTabsViewModel)},
            };

#if __IOS__
            if(Xamarin.Forms.Application.Current.MainPage is MasterDetailPage masterDetailPage)
            {
                masterDetailPage.IsGestureEnabled = false;
            }
            else if(Xamarin.Forms.Application.Current.MainPage is NavigationPage navigationPage && navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
            {
                nestedMasterDetail.IsGestureEnabled = false;
            }
#endif
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
                    // TODO: This logic doesn't belong here
                    //if(Xamarin.Forms.Application.Current.MainPage is MasterDetailPage masterDetailPage)
                    //{
                    //    masterDetailPage.IsPresented = false;
                    //}
                    //else if(Xamarin.Forms.Application.Current.MainPage is NavigationPage navigationPage && navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
                    //{
                    //    nestedMasterDetail.IsPresented = false;
                    //}
                    await _navigationService.Navigate(vmType);
                }));
            }
        }
    }
}
