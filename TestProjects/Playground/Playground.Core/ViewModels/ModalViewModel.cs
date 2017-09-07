using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ModalViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public ModalViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));

            ShowNestedModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<NestedModalViewModel>());
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

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowNestedModalCommand { get; private set; }
    }
}
