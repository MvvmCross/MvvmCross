using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class SecondHostViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _routingService;

        public SecondHostViewModel(IMvxNavigationService routingService)
        {
            _routingService = routingService;
        }

        private IMvxCommand _showACommand;

        public IMvxCommand ShowACommand
        {
            get
            {
                return _showACommand ?? (_showACommand = new MvxAsyncCommand(async () =>
                {
                    await _routingService.Navigate<TestAViewModel, User>(new User("MvvmCross", "Test"));

                    //await _routingService.Navigate("mvx://test/a");
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
                    //var result = await _routingService.Navigate<User, User>("mvx://test/?id=" + Guid.NewGuid().ToString("N"), new User("MvvmCross2", "Test2"));
                    var result = await _routingService.Navigate<TestBViewModel, User, User>(new User("MvvmCross", "Test"));
                    var test = result?.FirstName;
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
                    await _routingService.Navigate("mvx://random");
                }));
            }
        }
    }
}
