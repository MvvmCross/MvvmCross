// MvxIosSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;

namespace MvvmCross.iOS.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using Binding.iOS;
    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;
    using MvvmCross.Platform.iOS.Platform;
    using MvvmCross.Platform.iOS.Views;
    using iOS.Views;
    using iOS.Views.Presenters;

    using UIKit;

    public abstract class MvxIosSetup
        : MvxSetup
    {
        private readonly IMvxApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        private IMvxIosViewPresenter _presenter;

        protected MvxIosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            this._window = window;
            this._applicationDelegate = applicationDelegate;
        }

        protected MvxIosSetup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
        {
            this._presenter = presenter;
            this._applicationDelegate = applicationDelegate;
        }

        protected UIWindow Window => this._window;

        protected IMvxApplicationDelegate ApplicationDelegate => this._applicationDelegate;

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderPluginManager();
            var registry = new MvxLoaderPluginRegistry(".Touch", toReturn.Finders);
            this.AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = this.CreateIosViewsContainer();
            this.RegisterIosViewCreator(container);
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
            return new MvxIosViewDispatcher(this.Presenter);
        }

        protected override void InitializePlatformServices()
        {
            this.RegisterPlatformProperties();
            // for now we continue to register the old style platform properties
            this.RegisterOldStylePlatformProperties();
            this.RegisterPresenter();
            this.RegisterLifetime();
        }

        protected virtual void RegisterPlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxIosSystem>(this.CreateIosSystemProperties());
        }

        protected virtual MvxIosSystem CreateIosSystemProperties()
        {
            return new MvxIosSystem();
        }

        [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
        protected virtual void RegisterOldStylePlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxIosPlatformProperties>(new MvxIosPlatformProperties());
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(this._applicationDelegate);
        }

        protected IMvxIosViewPresenter Presenter
        {
            get
            {
                this._presenter = this._presenter ?? this.CreatePresenter();
                return this._presenter;
            }
        }

        protected virtual IMvxIosViewPresenter CreatePresenter()
        {
            return new MvxIosViewPresenter(this._applicationDelegate, this._window);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = this.Presenter;
            Mvx.RegisterSingleton(presenter);
            Mvx.RegisterSingleton<IMvxIosModalHost>(presenter);
        }

        protected override void InitializeLastChance()
        {
            this.InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitializeBindingBuilder()
        {
            this.RegisterBindingBuilderCallbacks();
            var bindingBuilder = this.CreateBindingBuilder();
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
            var bindingBuilder = new MvxIosBindingBuilder();
            return bindingBuilder;
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(this.ValueConverterAssemblies);
            registry.Fill(this.ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(this.GetViewModelAssemblies());
                toReturn.AddRange(this.GetViewAssemblies());
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