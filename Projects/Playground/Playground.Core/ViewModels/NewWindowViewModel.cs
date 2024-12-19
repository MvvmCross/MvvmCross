using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels.Navigation;

namespace Playground.Core.ViewModels
{
    public class NewWindowViewModel : MvxNavigationViewModel
    {
        private string _welcomeText = "Default welcome";

        public NewWindowViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : base(logFactory, navigationService)
        {
        }

        public IMvxAsyncCommand ShowRegionCommand =>
            new MvxAsyncCommand(() => this.NavigationService.Navigate<RegionViewModel>(this));

        public string WelcomeText
        {
            get => _welcomeText;
            set
            {
                ShouldLogInpc(true);
                SetProperty(ref _welcomeText, value);
                ShouldLogInpc(false);
            }
        }
    }
}
