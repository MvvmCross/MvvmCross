using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.MvvmCross.ViewModels;
using Tutorial.Core.ApplicationObjects;


namespace Tutorial.Core
{
    public class App
        : MvxApplication
        
    {
        public App()
        {
            var startApplicationObject = new AppStart();
            Mvx.RegisterSingleton<IMvxAppStart>(startApplicationObject);
        }
    }
}

