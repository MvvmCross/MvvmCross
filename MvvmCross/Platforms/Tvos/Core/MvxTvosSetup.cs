// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Converters;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Tvos.Binding;
using MvvmCross.Platforms.Tvos.Presenters;
using MvvmCross.Platforms.Tvos.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Core
{
    public abstract class MvxTvosSetup
        : MvxSetup, IMvxTvosSetup
    {
        private IMvxApplicationDelegate _applicationDelegate;
        private UIWindow _window;

        private IMvxTvosViewPresenter _customPresenter;

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            _window = window;
            _applicationDelegate = applicationDelegate;
        }

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxTvosViewPresenter presenter)
        {
            _customPresenter = presenter;
            _applicationDelegate = applicationDelegate;
        }

        protected UIWindow Window => _window;

        protected IMvxApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected override void RegisterViewPresenter()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxTvosViewPresenter>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewPresenter>(presenter => Mvx.IoCProvider.RegisterSingleton((IMvxTvosViewPresenter)presenter));
        }

        protected override void RegisterViewsContainer()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewsContainer, MvxTvosViewsContainer>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewsContainer>(container =>
            {
                Mvx.IoCProvider.RegisterSingleton((IMvxTvosViewCreator)container);
                Mvx.IoCProvider.RegisterSingleton((IMvxCurrentRequest)container);
            });
        }

        protected override void RegisterViewDispatcher()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewDispatcher, MvxTvosViewDispatcher>();
        }

        protected override void InitializePlatformServices()
        {
            RegisterPlatformProperties();
            RegisterLifetime();
            base.InitializePlatformServices();
        }

        protected virtual void RegisterPlatformProperties()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxTvosSystem>(CreateTvosSystemProperties());
        }

        protected virtual MvxTvosSystem CreateTvosSystemProperties()
        {
            return new MvxTvosSystem();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxTvosViewPresenter Presenter
        {
            get
            {
                return base.ViewPresenter as IMvxTvosViewPresenter;
            }
        }

        protected override void InitializeLastChance()
        {
            InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitializeBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxTvosBindingBuilder();
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }
    }

    public class MvxTvosSetup<TApplication> : MvxTvosSetup
        where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterApp()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxApplication, TApplication>();
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
