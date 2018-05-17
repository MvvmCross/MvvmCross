﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Converters;
using MvvmCross.Exceptions;
using MvvmCross.Plugin;
using MvvmCross.Core;
using MvvmCross.Platforms.Uap.Binding;
using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.Platforms.Uap.Views.Suspension;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Binders;
using MvvmCross.Presenters;
using MvvmCross.IoC;

namespace MvvmCross.Platforms.Uap.Core
{
    public abstract class MvxWindowsSetup
        : MvxSetup, IMvxWindowsSetup
    {
        private IMvxWindowsFrame _rootFrame;
        private string _suspensionManagerSessionStateKey;

        public virtual void PlatformInitialize(Frame rootFrame, IActivatedEventArgs activatedEventArgs,
            string suspensionManagerSessionStateKey = null)
        {
            PlatformInitialize(rootFrame, suspensionManagerSessionStateKey);
            ActivationArguments = activatedEventArgs;
        }

        public virtual void PlatformInitialize(Frame rootFrame, string suspensionManagerSessionStateKey = null)
        {
            PlatformInitialize(new MvxWrappedFrame(rootFrame));
            _suspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
        }

        public virtual void PlatformInitialize(IMvxWindowsFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public virtual void UpdateActivationArguments(IActivatedEventArgs e)
        {
            ActivationArguments = e;
        }

        protected override void InitializePlatformServices()
        {
            InitializeSuspensionManager();
            base.InitializePlatformServices();
        }

        protected virtual void InitializeSuspensionManager()
        {
            var suspensionManager = CreateSuspensionManager();
            Mvx.IoCProvider.RegisterSingleton(suspensionManager);

            if (_suspensionManagerSessionStateKey != null)
                suspensionManager.RegisterFrame(_rootFrame, _suspensionManagerSessionStateKey);
        }

        protected virtual IMvxSuspensionManager CreateSuspensionManager()
        {
            return new MvxSuspensionManager();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateStoreViewsContainer();
            Mvx.IoCProvider.RegisterSingleton<IMvxWindowsViewModelRequestTranslator>(container);
            Mvx.IoCProvider.RegisterSingleton<IMvxWindowsViewModelLoader>(container);
            var viewsContainer = container as MvxViewsContainer;
            if (viewsContainer == null)
                throw new MvxException("CreateViewsContainer must return an MvxViewsContainer");
            return container;
        }

        protected virtual IMvxStoreViewsContainer CreateStoreViewsContainer()
        {
            return new MvxWindowsViewsContainer();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            var dispatcher = base.CreateViewDispatcher() as MvxWindowsViewDispatcher;
            dispatcher.UiDispatcher = _rootFrame.UnderlyingControl.Dispatcher;
            return dispatcher;
        }

        protected IMvxWindowsViewPresenter Presenter
        {
            get
            {
                return base.ViewPresenter as IMvxWindowsViewPresenter;
            }
        }

        protected override IMvxViewPresenter CreateViewPresenter()
        {
            var presenter = base.CreateViewPresenter() as IMvxWindowsViewPresenter;
            presenter.RootFrame = _rootFrame;
            return presenter;
        }

        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            Mvx.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxWindowsViewPresenter>();
            Mvx.LazyConstructAndRegisterSingleton(() => Presenter);
            Mvx.LazyConstructAndRegisterSingleton<IMvxViewDispatcher, MvxWindowsViewDispatcher>();
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

        protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }

        protected IActivatedEventArgs ActivationArguments { get; private set; }

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

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxWindowsBindingBuilder();
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }
    }

    public class MvxWindowsSetup<TApplication> : MvxWindowsSetup
         where TApplication : class, IMvxApplication, new()
    {
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }

        protected override IMvxApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();
    }
}
