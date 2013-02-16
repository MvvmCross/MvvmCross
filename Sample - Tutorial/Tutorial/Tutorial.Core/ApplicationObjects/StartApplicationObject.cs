using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using Tutorial.Core.ViewModels;

namespace Tutorial.Core.ApplicationObjects
{
    public class StartApplicationObject 
        : MvxNavigatingObject
        , IMvxStartNavigation
    {
        public void Start()
        {
            this.RequestNavigate<MainMenuViewModel>();
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return true; }
        }
    }
}