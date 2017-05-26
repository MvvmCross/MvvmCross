using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{

    public class MainViewModel : MvxViewModel
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
            await _navigationService.Navigate<ViewModelA>();
        }

        private IMvxCommand _showACommand;

        public IMvxCommand ShowACommand
        {
            get
            {
                return _showACommand ?? (_showACommand = new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Navigate<TestAViewModel, User>(new User("MvvmCross", "Test"));

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
    }
}
