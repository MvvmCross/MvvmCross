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
        private readonly IMvxNavigationService _routingService;

        public MainViewModel(IMvxNavigationService routingService)
        {
            _routingService = routingService;
        }

        private ICommand _showACommand;

        public ICommand ShowACommand
        {
            get
            {
                return _showACommand ?? (_showACommand = new MvxAsyncCommand(async () =>
                {
                    await _routingService.RouteAsync("mvx://test/a");
                }));
            }
        }

        private ICommand _showBCommand;

        public ICommand ShowBCommand
        {
            get
            {
                return _showBCommand ?? (_showBCommand = new MvxAsyncCommand(async () =>
                {
                    await _routingService.RouteAsync("mvx://test/?id=" + Guid.NewGuid().ToString("N"));
                }));
            }
        }

        private ICommand _showRandomCommand;

        public ICommand ShowRandomCommand
        {
            get
            {
                return _showRandomCommand ?? (_showRandomCommand = new MvxAsyncCommand(async () =>
                {
                    await _routingService.RouteAsync("mvx://random");
                }));
            }
        }
    }
}
