// MvxTouchSetup.cs

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
    using MvvmCross.Binding.Touch;
    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;
    using MvvmCross.Platform.Touch.Platform;
    using MvvmCross.Platform.Touch.Views;
    using iOS.Views;
    using iOS.Views.Presenters;

    using UIKit;

    public abstract class MvxTouchSetup
        : MvxSetup
    {
        private readonly IMvxApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        private IMvxTouchViewPresenter _presenter;

        protected MvxTouchSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            this._window = window;
            this._applicationDelegate = applicationDelegate;
        }

        protected MvxTouchSetup(IMvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
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
            var container = this.CreateTouchViewsContainer();
            this.RegisterTouchViewCreator(container);
            return container;
        }

        protected virtual IMvxTouchViewsContainer CreateTouchViewsContainer()
        {
            return new MvxTouchViewsContainer();
        }

        protected virtual void RegisterTouchViewCreator(IMvxTouchViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxTouchViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxTouchViewDispatcher(this.Presenter);
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
            Mvx.RegisterSingleton<IMvxTouchSystem>(this.CreateTouchSystemProperties());
        }

        protected virtual MvxTouchSystem CreateTouchSystemProperties()
        {
            return new MvxTouchSystem();
        }

        [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
        protected virtual void RegisterOldStylePlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxTouchPlatformProperties>(new MvxTouchPlatformProperties());
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(this._applicationDelegate);
        }

        protected IMvxTouchViewPresenter Presenter
        {
            get
            {
                this._presenter = this._presenter ?? this.CreatePresenter();
                return this._presenter;
            }
        }

        protected virtual IMvxTouchViewPresenter CreatePresenter()
        {
            return new MvxTouchViewPresenter(this._applicationDelegate, this._window);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = this.Presenter;
            Mvx.RegisterSingleton(presenter);
            Mvx.RegisterSingleton<IMvxTouchModalHost>(presenter);
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
            var bindingBuilder = new MvxTouchBindingBuilder();
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