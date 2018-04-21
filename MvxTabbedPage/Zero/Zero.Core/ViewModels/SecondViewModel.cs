// ---------------------------------------------------------------
// <author>Paul Datsyuk</author>
// <url>https://www.linkedin.com/in/pauldatsyuk/</url>
// ---------------------------------------------------------------

using Acr.UserDialogs;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using Zero.Core.Services;
using Zero.Core.Helpers;

namespace Zero.Core.ViewModels
{
    public class SecondViewModel : MvxViewModel<Dictionary<string, string>>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly Services.IAppSettings _settings;
        private readonly IUserDialogs _userDialogs;

        private Dictionary<string, string> _parameter;
        private readonly ILocalizeService _localizeService;

        public SecondViewModel(IMvxNavigationService navigationService, Services.IAppSettings settings, IUserDialogs userDialogs, ILocalizeService localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;

            MainPageButtonText = "test";
        }

        public string MainPageButtonText { get; set; }

        public IMvxAsyncCommand BackCommand => new MvxAsyncCommand(async () =>
        {
            var localizedText = _localizeService.Translate("SecondPage_ByeBye_Localization");

            await _userDialogs.AlertAsync(localizedText);
            await _navigationService.Close(this);
        });

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            if (_parameter != null && _parameter.ContainsKey("ButtonText"))
                MainPageButtonText = "ButtonText";
        }

        public int SuperNumber
        {
            get { return _settings.SuperNumber; }
            set { _settings.SuperNumber = value; }
        }
    }
}