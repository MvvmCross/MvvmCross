using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class MainViewModel : MvxViewModelResult<User>
    {
        private readonly IMvxNavigationService _navigationService;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Start()
        {
            base.Start();
        }

        public override async Task Initialize()
        {
            //await _navigationService.Navigate<ViewModelA>();
        }

        private IMvxCommand _showACommand;

        public IMvxCommand ShowACommand
        {
            get
            {
                return _showACommand ?? (_showACommand = new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Navigate<ViewModelA, string, string>("test");
                    //await _navigationService.Navigate<TestAViewModel, User>(new User("MvvmCross", "Test"));

                    //await _navigationService.Navigate("mvx://test/a");
                }));
            }
        }

        private IMvxCommand _showBCommand;

        public IMvxCommand ShowBCommand
        {
            get
            {
                return _showBCommand ?? (_showBCommand = new MvxAsyncCommand(async () =>
                {
                    //var result = await _navigationService.Navigate<User, User>("mvx://test/?id=" + Guid.NewGuid().ToString("N"), new User("MvvmCross2", "Test2"));
                    var result = await _navigationService.Navigate<TestBViewModel, User, User>(new User("MvvmCross", "Test"));
                    var test = result?.FirstName;
                    //await _navigationService.Close(this, new User("Close parent", "Test"));
                }));
            }
        }

        private IMvxCommand _showDialogACommand;

        public IMvxCommand ShowDialogACommand
        {
            get
            {
                return _showDialogACommand ?? (_showDialogACommand = new MvxAsyncCommand(async () =>
                {
                    var result = await _navigationService.Navigate<ViewModelDialogA, IMvxViewModel, string>(this);
                }));
            }
        }

        private IMvxCommand _showRandomCommand;

        public IMvxCommand ShowRandomCommand
        {
            get
            {
                return _showRandomCommand ?? (_showRandomCommand = new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Navigate("mvx://random");
                }));
            }
        }

        private IMvxCommand _showPrePopCommand;

        public IMvxCommand ShowPrePopCommand
        {
            get
            {
                return _showPrePopCommand ?? (_showPrePopCommand = new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Navigate("mvx://prepop");
                }));
            }
        }
    }
}