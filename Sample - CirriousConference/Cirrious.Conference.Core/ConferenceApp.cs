using Cirrious.Conference.Core.ApplicationObjects;
using Cirrious.Conference.Core.Interfaces;
using Cirrious.Conference.Core.Models;
using Cirrious.Conference.Core.Models.Twitter;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.Conference.Core
{
    public abstract class BaseConferenceApp 
        : MvxApplication
        , IMvxServiceProducer<IMvxStartNavigation>
        , IMvxServiceProducer<IMvxTextProvider>
        , IMvxServiceProducer<IConferenceService>
        , IMvxServiceProducer<ITwitterSearchProvider>
        , IMvxServiceProducer<IErrorReporter>
        , IMvxServiceProducer<IErrorSource>
    {
        protected BaseConferenceApp()
        {
            InitialiseText();
            InitaliseServices();
            InitaliseErrorSystem();
            InitialiseStartNavigation();
        }

        private void InitaliseErrorSystem()
        {
            var errorHub = new ErrorApplicationObject();
            this.RegisterServiceInstance<IErrorReporter>(errorHub);
            this.RegisterServiceInstance<IErrorSource>(errorHub);
        }

        private void InitaliseServices()
        {
            var repository = new ConferenceService();
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
        protected override void InitialiseStartNavigation()
        {
            var startApplicationObject = new StartApplicationObject(true);
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
        }
    }

    public class NoSplashScreenConferenceApp
        : BaseConferenceApp
    {
        protected override void InitialiseStartNavigation()
        {
            var startApplicationObject = new StartApplicationObject(false);
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
        }
    }
}
