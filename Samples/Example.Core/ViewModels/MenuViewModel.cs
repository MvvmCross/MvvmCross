using Cirrious.MvvmCross.ViewModels;

namespace Example.Core.ViewModels
{
    public class MenuViewModel
        : MvxViewModel
    {
        public IMvxCommand HomeCommand { get; private set; }
        public IMvxCommand SettingsCommand { get; private set; }
        public IMvxCommand HelpCommand { get; private set; }

        public MenuViewModel ()
        {
            HomeCommand = new MvxCommand(() => ShowViewModel<ExamplesViewModel>());
            SettingsCommand = new MvxCommand(() => ShowViewModel<SettingsViewModel>());
            HelpCommand = new MvxCommand(() => ShowViewModel<SettingsViewModel>());
        }        
    }
}