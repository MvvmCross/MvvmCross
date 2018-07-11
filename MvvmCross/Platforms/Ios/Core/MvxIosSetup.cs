// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Converters;
using MvvmCross.Plugin;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core;
using MvvmCross.Platforms.Ios;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;
using MvvmCross.Presenters;
using MvvmCross.IoC;

namespace MvvmCross.Platforms.Ios.Core
{
    public abstract class MvxIosSetup
        : MvxSetup, IMvxIosSetup
    {
        protected IMvxApplicationDelegate ApplicationDelegate { get; private set; }
        protected UIWindow Window { get; private set; }

        private IMvxIosViewPresenter _customPresenter;

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            Window = window;
            ApplicationDelegate = applicationDelegate;
        }

        public virtual void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
        {
            ApplicationDelegate = applicationDelegate;
            _customPresenter = presenter;
        }

        protected override void RegisterViewPresenter()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxIosViewPresenter>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewPresenter>(presenter => Mvx.IoCProvider.RegisterSingleton((IMvxIosViewPresenter)presenter));
        }

        protected override void RegisterViewsContainer()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewsContainer, MvxIosViewsContainer>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewsContainer>(container =>
            {
                Mvx.IoCProvider.RegisterSingleton((IMvxIosViewCreator)container);
                Mvx.IoCProvider.RegisterSingleton((IMvxCurrentRequest)container);
            });
        }

        protected override void RegisterViewDispatcher()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewDispatcher, MvxIosViewDispatcher>();
        }

        protected override void InitializePlatformServices()
        {
            RegisterPlatformProperties();
            RegisterLifetime();
            base.InitializePlatformServices();
        }

        protected virtual void RegisterPlatformProperties()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxIosSystem>(CreateIosSystemProperties());
        }

        protected virtual MvxIosSystem CreateIosSystemProperties()
        {
            return new MvxIosSystem();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxLifetime>(ApplicationDelegate);
        }

        protected IMvxIosViewPresenter IosPresenter
        {
            get
            {
                return base.ViewPresenter as IMvxIosViewPresenter;
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
            return new MvxIosBindingBuilder();
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

    public class MvxIosSetup<TApplication> : MvxIosSetup
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
