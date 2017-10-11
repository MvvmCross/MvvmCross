using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavMasterDetailViewModel : MvxMasterDetailViewModel<MixedNavMasterRootContentViewModel>
    {
        private readonly IMvxNavigationService _navigationService;
        private MenuItem _menuItem;
        private IMvxCommand _onSelectedChangedCommand;

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
        }

        public IEnumerable<MenuItem> Menu { get; set; }

        public MenuItem SelectedMenu {
            get => _menuItem;
            set {
                if (SetProperty(ref _menuItem, value))
                    OnSelectedChangedCommand.Execute(value);
            }
        }

        private IMvxCommand OnSelectedChangedCommand {
            get {
                return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new MvxCommand<MenuItem>(item =>
                {
                    if (item == null)
                        return;

                    var vmType = item.ViewModelType;

                    _navigationService.Navigate(vmType);
                }));
            }
        }
    }
}
