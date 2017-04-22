using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Presenter.Core;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MasterDetailExample.Core.ViewModels
{
    /// <summary>
    /// This will be the ViewModel used in the Master page of the MainPage
    /// The type used in MvxMasterDetailViewModel determines the ViewModel used in the RootContentViewModel
    /// This sample will put a simple ListView in the Master and reacts to changes in 
    /// In every platform: use the right Presenter (MvxFormsDroidMasterDetailPagePresenter, ...)
    /// </summary>

    public class MainViewModel : MvxMasterDetailViewModel<RootContentViewModel>
    {
        MenuItem _menuItem;
        public MenuItem SelectedMenu {
            get { return _menuItem; }
            set {
                if (SetProperty(ref _menuItem, value))
                    OnSelectedChangedCommand.Execute(value);
            } }

        IEnumerable<MenuItem> _menu;
        public IEnumerable<MenuItem> Menu { get { return _menu; } set { SetProperty(ref _menu, value); } }

        MvxCommand<MenuItem> _onSelectedChangedCommand;
        ICommand OnSelectedChangedCommand { get
            {
                return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new MvxCommand<MenuItem>((item) =>
                {
                    if (item == null)
                        return;

                    var vmType =  item.ViewModelType;

                    // We demand to clear the Navigation stack as we are changing the section
                    var presentationBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationMode", "ClearStack" } });

                    // Show the ViewModel in the Detail NavigationPage
                    ShowViewModel(vmType, presentationBundle: presentationBundle);
                }));
            } }

        public MainViewModel()
        {
            Menu = new[] {
                new MenuItem { Title = "Opción 1", Description = "Descripción Opción 1", ViewModelType = typeof(Option1ViewModel) },
                new MenuItem { Title = "Opción 2", Description = "Descripción Opción 2", ViewModelType = typeof(Option2ViewModel) },
                new MenuItem { Title = "Opción 3", Description = "Descripción Opción 3", ViewModelType = typeof(Option3ViewModel) }
            };
        }

        public override void RootContentPageActivated()
        {
            // When user go backs to root page in NavigationPage (using UI back or changing option in Menu)
            // we unset the SelectedItem of our list
            SelectedMenu = null;
        }
    }

    public class MenuItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Type ViewModelType { get; set; }
    }
}
