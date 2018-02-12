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
using MvvmCross.Platform.Ios;
using MvvmCross.Platform.Ios.Binding;
using MvvmCross.Platform.Ios.Presenters;
using MvvmCross.Platform.Ios.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;
using MvvmCross.Presenters;

namespace MvvmCross.Platform.Ios.Core
{
    public abstract class MvxIosSetup
        : MvxSetup
    {
        private readonly IMvxApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        private IMvxIosViewPresenter _presenter;

        protected MvxIosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            _window = window;
            _applicationDelegate = applicationDelegate;
        }

        protected MvxIosSetup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
        {
            _applicationDelegate = applicationDelegate;
            _presenter = presenter;
        }

        protected UIWindow Window => _window;

        protected IMvxApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateIosViewsContainer();
            RegisterIosViewCreator(container);
            return container;
        }

        protected virtual IMvxIosViewsContainer CreateIosViewsContainer()
        {
            return new MvxIosViewsContainer();
        }

        protected virtual void RegisterIosViewCreator(IMvxIosViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxIosViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxIosViewDispatcher(Presenter);
        }

        protected override void InitializePlatformServices()
        {
            RegisterPlatformProperties();
            RegisterPresenter();
            RegisterLifetime();
            base.InitializePlatformServices();
        }

        protected virtual void RegisterPlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxIosSystem>(CreateIosSystemProperties());
        }

        protected virtual MvxIosSystem CreateIosSystemProperties()
        {
            return new MvxIosSystem();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxIosViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxIosViewPresenter CreateViewPresenter()
        {
            return new MvxIosViewPresenter(_applicationDelegate, _window);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.RegisterSingleton(presenter);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);
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
            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
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
}
