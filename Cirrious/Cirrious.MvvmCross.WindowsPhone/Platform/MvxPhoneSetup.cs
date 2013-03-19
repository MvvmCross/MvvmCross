// MvxPhoneSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Lifetime;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsPhone.Platform.Lifetime;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    public abstract class MvxPhoneSetup
        : MvxSetup
    {
        private readonly PhoneApplicationFrame _rootFrame;

        protected MvxPhoneSetup(PhoneApplicationFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override void InitializeDebugServices()
        {
            Mvx.RegisterSingleton<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_rootFrame);
            Mvx.RegisterSingleton<IMvxPhoneViewModelRequestTranslator>(container);
            return container;
        }

        protected override IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return CreateViewDispatcherProvider(_rootFrame);
        }

        protected virtual IMvxPhoneViewPresenter CreateViewPresenter(PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewPresenter(rootFrame);
        }

        protected virtual IMvxViewDispatcherProvider CreateViewDispatcherProvider(PhoneApplicationFrame rootFrame)
        {
            var presenter = CreateViewPresenter(rootFrame);
            return CreateViewDispatcherProvider(presenter, rootFrame);
        }

        protected virtual MvxPhoneViewDispatcherProvider CreateViewDispatcherProvider(IMvxPhoneViewPresenter presenter,
                                                                                      PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewDispatcherProvider(presenter, rootFrame);
        }

        protected virtual MvxPhoneViewsContainer CreateViewsContainer(PhoneApplicationFrame rootFrame)
        {
            return new MvxPhoneViewsContainer();
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof (IMvxPhoneView));
        }

        protected virtual void InitializeNavigationRequestSerializer()
        {
            Mvx.RegisterSingleton(CreateNavigationRequestSerializer());
        }

        protected abstract IMvxNavigationRequestSerializer CreateNavigationRequestSerializer();

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxFilePluginManager(".WindowsPhone");
            return toReturn;
        }

        protected override void InitializePlatformServices()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(new MvxPhoneLifetimeMonitor());
            InitializeNavigationRequestSerializer();
        }
    }
}