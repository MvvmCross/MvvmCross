using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using MyApplication.Core.ApplicationObjects;
using MyApplication.Core.Interfaces.Errors;
using MyApplication.Core.Interfaces.First;
using MyApplication.Core.Services;

namespace MyApplication.Core
{
    public class App
        : MvxApplication
        , IMvxProducer
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
            this.RegisterServiceInstance<IErrorReporter>(errorService);
            this.RegisterServiceInstance<IErrorSource>(errorService);
        }

        private void InitaliseServices()
        {
            this.RegisterServiceInstance<IFirstService>(new FirstService());
        }

        private void InitialiseStartNavigation()
        {
            var startApplicationObject = new StartNavigation();
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
        }

        private void InitialisePlugins()
        {
            // initialise any plugins where are required at app startup
            // e.g. Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
        }
    }
}
