using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Bindings
{
    public class FluentBindingViewModel : BaseViewModel
    {
        bool _bindingsEnabled = true;

        public FluentBindingViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ClearBindingsCommand = new MvxCommand(ClearBindings);
        }

        public IMvxCommand ClearBindingsCommand { get; }

        public MvxInteraction<bool> ClearBindingInteraction { get; } = new MvxInteraction<bool>();

        string _name;
        public string TextValue
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        void ClearBindings()
        {
            _bindingsEnabled = !_bindingsEnabled;
            ClearBindingInteraction.Raise(_bindingsEnabled);
        }
    }
}
