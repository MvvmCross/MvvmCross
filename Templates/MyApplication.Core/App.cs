using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ViewModels;
using MyApplication.Core.ApplicationObjects;
using MyApplication.Core.Interfaces.Errors;
using MyApplication.Core.Interfaces.First;
using MyApplication.Core.Services;

namespace MyApplication.Core
{
    public class App
        : MvxApplication
        
    {
        public App()
        {
            InitaliseErrorReporting();
            InitialisePlugins();
            InitaliseServices();
            InitialiseStartNavigation();
        }

        private void InitaliseErrorReporting()
        {
            var errorService = new ErrorApplicationObject();
            Mvx.RegisterSingleton<IErrorReporter>(errorService);
            Mvx.RegisterSingleton<IErrorSource>(errorService);
        }

        private void InitaliseServices()
        {
            Mvx.RegisterSingleton<IFirstService>(new FirstService());
        }

        private void InitialiseStartNavigation()
        {
            var startApplicationObject = new StartNavigation();
            Mvx.RegisterSingleton<IMvxStartNavigation>(startApplicationObject);
        }

        private void InitialisePlugins()
        {
            // initialise any plugins where are required at app startup
            // e.g. Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
        }
    }
}
