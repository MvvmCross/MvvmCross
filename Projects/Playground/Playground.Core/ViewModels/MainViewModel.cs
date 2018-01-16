using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace Playground.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private int _counter = 2;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ChildViewModel>());

            ShowModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalViewModel>());

            ShowModalNavCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalNavViewModel>());

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());

            ShowSplitCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitRootViewModel>());

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<OverrideAttributeViewModel>());

            ShowSheetCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SheetViewModel>());

            ShowWindowCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<WindowViewModel>());

            ShowMixedNavigationCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<MixedNavFirstViewModel>());

            _counter = 3;
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            base.SaveStateToBundle(bundle);

            bundle.Data["MyKey"] = _counter.ToString();
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            base.ReloadFromBundle(state);

            _counter = int.Parse(state.Data["MyKey"]);
        }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }

        public IMvxAsyncCommand ShowModalCommand { get; private set; }

        public IMvxAsyncCommand ShowModalNavCommand { get; private set; }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand ShowSplitCommand { get; private set; }

        public IMvxAsyncCommand ShowOverrideAttributeCommand { get; private set; }

        public IMvxAsyncCommand ShowSheetCommand { get; private set; }

        public IMvxAsyncCommand ShowWindowCommand { get; private set; }

        public IMvxAsyncCommand ShowMixedNavigationCommand { get; private set; }

        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder("MvxBindingsExample", "Text"); }
        }

        private string _bindableText = "I'm bound!";
        public string BindableText
        {
            get
            {
                return _bindableText;
            }
            set
            {
                if (BindableText != value)
                {
                    _bindableText = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
