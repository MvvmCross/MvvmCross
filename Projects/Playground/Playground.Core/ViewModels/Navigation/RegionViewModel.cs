using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Navigation
{
    public class RegionViewModel : MvxNavigationViewModel
    {
        public RegionViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : base(logFactory, navigationService)
        {
        }

        public IMvxAsyncCommand CloseRegionCommand =>
            new MvxAsyncCommand(() => this.NavigationService.Close(this));
    }
}
