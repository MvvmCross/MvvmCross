// Licensed to the .NET Foundation under one or more agreements.
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

        private IMvxMacViewPresenter _presenter;

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
            _presenter = presenter;
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

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateMacViewsContainer();
            RegisterMacViewCreator(container);
            return container;
        }

        protected virtual IMvxMacViewsContainer CreateMacViewsContainer()
        {
            return new MvxMacViewsContainer();
        }

        protected void RegisterMacViewCreator(IMvxMacViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxMacViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxMacViewDispatcher(_presenter);
        }

        protected override void InitializePlatformServices()
        {
            RegisterPresenter();
            RegisterLifetime();
            base.InitializePlatformServices();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxMacViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxMacViewPresenter CreateViewPresenter()
        {
            return new MvxMacViewPresenter(_applicationDelegate);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = this.Presenter;
            Mvx.RegisterSingleton(presenter);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);
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
            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(this.FillValueConverters);
            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(this.FillTargetFactories);
            Mvx.CallbackWhenRegistered<IMvxBindingNameRegistry>(this.FillBindingNames);
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
        protected override IMvxApplication CreateApp() => Mvx.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
