using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace Playground.Core.ViewModels
{
    public class BindingsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private int _counter = 2;

        public BindingsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
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
