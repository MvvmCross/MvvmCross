using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.Conference.Core.Interfaces;

namespace Cirrious.Conference.Core.ApplicationObjects
{
    public class StartApplicationObject
        : MvxNavigatingObject
        , IMvxStartNavigation
		
    {
        private readonly bool _showSplashScreen;
        public StartApplicationObject(bool showSplashScreen)
        {
            _showSplashScreen = showSplashScreen;
        }

        public void Start()
        {
            var confService = Mvx.Resolve<IConferenceService>();
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
