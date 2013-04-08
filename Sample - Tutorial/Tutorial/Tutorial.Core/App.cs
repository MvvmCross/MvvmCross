using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.MvvmCross.ViewModels;
using Tutorial.Core.ViewModels;


namespace Tutorial.Core
{
    public class App
        : MvxApplication        
    {
        public App()
        {
            RegisterAppStart<MainMenuViewModel>();
        }
    }
}

