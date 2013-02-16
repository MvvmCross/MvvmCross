using Cirrious.Conference.Core.ApplicationObjects;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.Conference.Core.Models;
using Cirrious.Conference.Core.Models.Twitter;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Localization.Interfaces;

namespace Cirrious.Conference.Core
{
    public abstract class BaseConferenceApp 
        : MvxApplication
        , IMvxServiceProducer
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
            this.RegisterServiceInstance<IErrorReporter>(errorHub);
            this.RegisterServiceInstance<IErrorSource>(errorHub);
        }

        private void InitialiseServices()
        {
            var repository = new ConferenceService();
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            this.RegisterServiceInstance<IConferenceService>(repository);
            this.RegisterServiceInstance<ITwitterSearchProvider>(new TwitterSearchProvider());
        }

        private void InitialiseText()
        {
            var builder = new TextProviderBuilder();
            // TODO - could choose a language here: builder.LoadResources(whichLanguage);
            this.RegisterServiceInstance<IMvxTextProvider>(builder.TextProvider);
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
            var startApplicationObject = new StartApplicationObject(true);
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
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
            var startApplicationObject = new StartApplicationObject(false);
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
        }
    }
}
