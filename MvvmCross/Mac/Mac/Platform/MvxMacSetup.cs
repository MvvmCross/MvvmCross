using System;
using System.Collections.Generic;
using System.Reflection;

using AppKit;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Binding.Mac;
using MvvmCross.Mac.Views;
using MvvmCross.Mac.Views.Presenters;

namespace MvvmCross.Mac.Platform
{
    public abstract class MvxMacSetup
        : MvxSetup
    {
        private readonly MvxApplicationDelegate _applicationDelegate;
        private readonly NSWindow _window;

        private IMvxMacViewPresenter _presenter;

        protected MvxMacSetup(MvxApplicationDelegate applicationDelegate, NSWindow window)
        {
            _window = window;
            _applicationDelegate = applicationDelegate;
        }

        protected MvxMacSetup(MvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
            _presenter = presenter;
            _applicationDelegate = applicationDelegate;
        }

        protected NSWindow Window
        {
            get { return _window; }
        }

        protected MvxApplicationDelegate ApplicationDelegate
        {
            get { return _applicationDelegate; }
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
            return new MvxPluginManager();
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
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxMacViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreatePresenter();
                return _presenter;
            }
        }

        protected virtual IMvxMacViewPresenter CreatePresenter()
        {
            return new MvxMacViewPresenter(_applicationDelegate);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = this.Presenter;
            Mvx.RegisterSingleton(presenter);
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
}