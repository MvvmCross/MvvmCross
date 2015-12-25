namespace MvvmCross.Mac.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using AppKit;

    using global::MvvmCross.Binding;
    using global::MvvmCross.Binding.Binders;
    using global::MvvmCross.Binding.BindingContext;
    using global::MvvmCross.Binding.Bindings.Target.Construction;
    using global::MvvmCross.Core.Platform;
    using global::MvvmCross.Core.ViewModels;
    using global::MvvmCross.Core.Views;
    using global::MvvmCross.Platform;
    using global::MvvmCross.Platform.Converters;
    using global::MvvmCross.Platform.Platform;
    using global::MvvmCross.Platform.Plugins;

    using MvvmCross.Binding.Mac;
    using MvvmCross.Mac.Views;
    using MvvmCross.Mac.Views.Presenters;

    public abstract class MvxMacSetup
        : MvxSetup
    {
        private readonly MvxApplicationDelegate _applicationDelegate;
        private readonly NSWindow _window;

        private IMvxMacViewPresenter _presenter;

        protected MvxMacSetup(MvxApplicationDelegate applicationDelegate, NSWindow window)
        {
            this._window = window;
            this._applicationDelegate = applicationDelegate;
        }

        protected MvxMacSetup(MvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
            this._presenter = presenter;
            this._applicationDelegate = applicationDelegate;
        }

        protected NSWindow Window
        {
            get { return this._window; }
        }

        protected MvxApplicationDelegate ApplicationDelegate
        {
            get { return this._applicationDelegate; }
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override void InitializeDebugServices()
        {
            Mvx.RegisterSingleton<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderPluginManager();
            var registry = new MvxLoaderPluginRegistry(".Mac", toReturn.Finders);
            this.AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = this.CreateMacViewsContainer();
            this.RegisterMacViewCreator(container);
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
            return new MvxMacViewDispatcher(this._presenter);
        }

        protected override void InitializePlatformServices()
        {
            this.RegisterPresenter();
            this.RegisterLifetime();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(this._applicationDelegate);
        }

        protected IMvxMacViewPresenter Presenter
        {
            get
            {
                this._presenter = this._presenter ?? this.CreatePresenter();
                return this._presenter;
            }
        }

        protected virtual IMvxMacViewPresenter CreatePresenter()
        {
            return new MvxMacViewPresenter(this._applicationDelegate, this._window);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = this.Presenter;
            Mvx.RegisterSingleton(presenter);
        }

        protected override void InitializeLastChance()
        {
            this.InitialiseBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitialiseBindingBuilder()
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
            var bindingBuilder = new MvxMacBindingBuilder();
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

        protected virtual List<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(this.GetViewModelAssemblies());
                toReturn.AddRange(this.GetViewAssemblies());
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
}