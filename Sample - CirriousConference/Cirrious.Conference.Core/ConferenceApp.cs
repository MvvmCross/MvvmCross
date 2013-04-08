using Cirrious.Conference.Core.ApplicationObjects;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.Conference.Core.Models;
using Cirrious.Conference.Core.Models.Twitter;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core
{
    public abstract class BaseConferenceApp 
        : MvxApplication        
    {
        protected BaseConferenceApp()
        {
            InitialisePlugins();
            InitialiseText();
            InitialiseServices();
            InitaliseErrorSystem();
        }

        private void InitialisePlugins()
        {
			Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();

            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.JsonLocalisation.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.ResourceLoader.PluginLoader.Instance.EnsureLoaded();
			Cirrious.MvvmCross.Plugins.Messenger.PluginLoader.Instance.EnsureLoaded();

            // these don't really need to be loaded on startup, but it's convenient for now
            Cirrious.MvvmCross.Plugins.Email.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.PhoneCall.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.Share.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.WebBrowser.PluginLoader.Instance.EnsureLoaded();
        }

        private void InitaliseErrorSystem()
        {
            var errorHub = new ErrorApplicationObject();
            Mvx.RegisterSingleton<IErrorReporter>(errorHub);
            Mvx.RegisterSingleton<IErrorSource>(errorHub);
        }

        private void InitialiseServices()
        {
            var repository = new ConferenceService();
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Mvx.RegisterSingleton<IConferenceService>(repository);
            Mvx.RegisterSingleton<ITwitterSearchProvider>(new TwitterSearchProvider());
        }

        private void InitialiseText()
        {
            var builder = new TextProviderBuilder();
            // TODO - could choose a language here: builder.LoadResources(whichLanguage);
            Mvx.RegisterSingleton<IMvxTextProvider>(builder.TextProvider);
        }

        protected abstract void InitialiseStartNavigation();
    }

    public class ConferenceApp
        : BaseConferenceApp
    {
        public ConferenceApp()
        {
            InitialiseStartNavigation();
        }

        protected sealed override void InitialiseStartNavigation()
        {
            var startApplicationObject = new AppStart(true);
            Mvx.RegisterSingleton<IMvxAppStart>(startApplicationObject);
        }
    }

    public class NoSplashScreenConferenceApp
        : BaseConferenceApp
    {
        public NoSplashScreenConferenceApp()
        {
            InitialiseStartNavigation();
        }

        protected sealed override void InitialiseStartNavigation()
        {
            var startApplicationObject = new AppStart(false);
            Mvx.RegisterSingleton<IMvxAppStart>(startApplicationObject);
        }
    }
}
