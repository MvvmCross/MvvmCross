using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace RoutingExample.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _routingService;

        private ICommand _showACommand;

        private ICommand _showBCommand;

        private ICommand _showRandomCommand;

        public MainViewModel(IMvxNavigationService routingService)
        {
            _routingService = routingService;
        }

        public ICommand ShowACommand
        {
            get
            {
                return _showACommand ?? (_showACommand = new MvxAsyncCommand(async () =>
                {
                    await _routingService.Navigate("mvx://test/a");
                }));
            }
        }

        public ICommand ShowBCommand
        {
            get
            {
                return _showBCommand ?? (_showBCommand = new MvxAsyncCommand(async () =>
                {
                    await _routingService.Navigate("mvx://test/?id=" + Guid.NewGuid().ToString("N"));
                }));
            }
        }

        public ICommand ShowRandomCommand
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