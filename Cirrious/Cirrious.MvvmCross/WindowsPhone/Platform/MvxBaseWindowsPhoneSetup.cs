using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    public abstract class MvxBaseWindowsPhoneSetup 
        : MvxBaseSetup        
          , IMvxServiceProducer<IMvxApplicationTitle>
          , IMvxServiceProducer<IMvxViewModelLocatorFinder>
          , IMvxServiceProducer<IMvxViewModelLocatorStore>
          , IMvxServiceProducer<IMvxViewsContainer>
          , IMvxServiceProducer<IMvxViewDispatcherProvider>
          , IMvxServiceProducer<IMvxWindowsPhoneViewModelRequestTranslator>
          , IMvxServiceConsumer<IMvxViewsContainer>
    {
        private readonly PhoneApplicationFrame _rootFrame;

        protected MvxBaseWindowsPhoneSetup(PhoneApplicationFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected void Add<TViewModel, TView>(IMvxViewsContainer container) where TViewModel : IMvxViewModel
        {
            container.Add(typeof(TViewModel), typeof(TView));
        }

        protected void Add(IMvxViewsContainer container, Type viewModelType, Type viewType)
        {
            container.Add(viewModelType, viewType);
        }

        protected override void InitializeViewsContainer()
        {
            var container = new MvxPhoneViewsContainer(_rootFrame);

            this.RegisterServiceInstance<IMvxViewsContainer>(container);
            this.RegisterServiceInstance<IMvxViewDispatcherProvider>(container);
            this.RegisterServiceInstance<IMvxWindowsPhoneViewModelRequestTranslator>(container);
        }

        protected override void InitializeApp()
        {
            var app = CreateApp();
            this.RegisterServiceInstance<IMvxApplicationTitle>(app);
            this.RegisterServiceInstance<IMvxViewModelLocatorFinder>(app);
            this.RegisterServiceInstance<IMvxViewModelLocatorStore>(app);
        }

        protected override void  InitializeViews()
        {
            var container = this.GetService<IMvxViewsContainer>();

            foreach (var pair in GetViewModelViewLookup())
            {
                Add(container, pair.Key, pair.Value);
            }
        }

        protected abstract MvxApplication CreateApp();
        protected abstract IDictionary<Type, Type> GetViewModelViewLookup();
    }
}