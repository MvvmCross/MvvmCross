﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppKit;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Converters;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Mac.Binding;
using MvvmCross.Platforms.Mac.Presenters;
using MvvmCross.Platforms.Mac.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Mac.Core
{
    public abstract class MvxMacSetup
        : MvxSetup, IMvxMacSetup
    {
        private IMvxApplicationDelegate _applicationDelegate;
        private NSWindow _window;
        private IMvxMacViewPresenter _customPresenter;

        public void PlatformInitialize(IMvxApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        }

        public void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, NSWindow window)
        {
            PlatformInitialize(applicationDelegate);
            _window = window;
        }

        public void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
            PlatformInitialize(applicationDelegate);
            _customPresenter = presenter;
        }

        protected NSWindow Window
        {
            get { return _window; }
        }

        protected IMvxApplicationDelegate ApplicationDelegate
        {
            get { return _applicationDelegate; }
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }

        protected override void RegisterViewPresenter()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxMacViewPresenter>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewPresenter>(presenter => Mvx.IoCProvider.RegisterSingleton((IMvxMacViewPresenter)presenter));
        }

        protected override void RegisterViewsContainer()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewsContainer, MvxMacViewsContainer>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewsContainer>(container =>
            {
                Mvx.IoCProvider.RegisterSingleton((IMvxMacViewCreator)container);
                Mvx.IoCProvider.RegisterSingleton((IMvxCurrentRequest)container);
            });
        }

        protected override void RegisterViewDispatcher()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewDispatcher, MvxMacViewDispatcher>();
        }

        protected override void InitializePlatformServices()
        {
            RegisterLifetime();
            base.InitializePlatformServices();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxMacViewPresenter MacPresenter
        {
            get
            {
                return base.ViewPresenter as IMvxMacViewPresenter;
            }
        }

        protected override void InitializeLastChance()
        {
            InitialiseBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitialiseBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(this.FillValueConverters);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(this.FillTargetFactories);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(this.FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            var bindingBuilder = new MvxMacBindingBuilder();
            return bindingBuilder;
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual IEnumerable<Type> ValueConverterHolders
        {
            get { return null; }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }

    public class MvxMacSetup<TApplication> : MvxMacSetup
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
