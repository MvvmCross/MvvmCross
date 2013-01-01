// MvxBaseWindowsPhoneSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Cirrious.MvvmCross.WindowsPhone.Platform.Lifetime;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    public abstract class MvxBaseWindowsPhoneSetup
        : MvxBaseSetup
          , IMvxServiceProducer
    {
        private readonly PhoneApplicationFrame _rootFrame;

        protected MvxBaseWindowsPhoneSetup(PhoneApplicationFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_rootFrame);
            this.RegisterServiceInstance<IMvxWindowsPhoneViewModelRequestTranslator>(container);
            return container;
        }

        protected override IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return CreateViewDispatcherProvider(_rootFrame);
        }

        protected virtual IMvxViewDispatcherProvider CreateViewDispatcherProvider(PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewDispatcherProvider(rootFrame);
        }

        protected virtual MvxPhoneViewsContainer CreateViewsContainer(PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewsContainer(rootFrame);
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof (IMvxWindowsPhoneView));
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderBasedPluginManager();
            var registry = new MvxLoaderPluginRegistry(".WindowsPhone", toReturn.Loaders);
            AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected override void InitializePlatformServices()
        {
            this.RegisterServiceInstance<IMvxLifetime>(new MvxWindowsPhoneLifetimeMonitor());
        }
    }
}