using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Plugins.Location;
using Tutorial.Core.ApplicationObjects;


namespace Tutorial.Core
{
    public class App
        : MvxApplication
        
    {
        public App()
        {
            var startApplicationObject = new StartApplicationObject();
            Mvx.RegisterSingleton<IMvxStartNavigation>(startApplicationObject);
        }
    }
}

