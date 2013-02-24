using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.Conference.Core.Interfaces;

namespace Cirrious.Conference.Core.ApplicationObjects
{
    public class StartApplicationObject
        : MvxNavigatingObject
        , IMvxStartNavigation
		, IMvxConsumer
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
