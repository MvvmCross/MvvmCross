using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.Conference.Core.ApplicationObjects
{
    public class StartApplicationObject
        : MvxApplicationObject
        , IMvxStartNavigation
		, IMvxServiceConsumer<IConferenceService>
    {
        private readonly bool _showSplashScreen;
        public StartApplicationObject(bool showSplashScreen)
        {
            _showSplashScreen = showSplashScreen;
        }

        public void Start()
        {
            var confService = this.GetService<IConferenceService>();
            if (_showSplashScreen)
            {
                confService.BeginAsyncLoad();
                RequestNavigate<SplashScreenViewModel>();
            }
            else
            {
                confService.DoSyncLoad();
                RequestNavigate<HomeViewModel>();
            }
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return true; }
        }
    }
}
