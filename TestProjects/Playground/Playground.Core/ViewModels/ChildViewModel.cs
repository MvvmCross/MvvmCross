using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ChildViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public ChildViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));

            ShowSecondChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SecondChildViewModel>());

            ShowRootCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<RootViewModel>());
        }

        public override System.Threading.Tasks.Task Initialize()
        {
            return base.Initialize();
        }

        public void Init()
        {
        }

        public override void Start()
        {
            base.Start();
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowSecondChildCommand { get; private set; }

        public IMvxAsyncCommand ShowRootCommand { get; private set; }
    }
}
