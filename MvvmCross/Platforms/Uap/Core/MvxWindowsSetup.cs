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
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Platforms.Uap.Binding;
using MvvmCross.Platforms.Uap.Presenters;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.Platforms.Uap.Views.Suspension;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace MvvmCross.Platforms.Uap.Core
{
#nullable enable
    public abstract class MvxWindowsSetup
        : MvxSetup, IMvxWindowsSetup
    {
        private IMvxWindowsFrame? _rootFrame;
        private string? _suspensionManagerSessionStateKey;
        private IMvxWindowsViewPresenter? _presenter;

        public virtual void PlatformInitialize(Frame rootFrame, IActivatedEventArgs activatedEventArgs,
            string? suspensionManagerSessionStateKey = null)
        {
            PlatformInitialize(rootFrame, suspensionManagerSessionStateKey);
            ActivationArguments = activatedEventArgs;
        }

        public virtual void PlatformInitialize(Frame rootFrame, string? suspensionManagerSessionStateKey = null)
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

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            InitializeSuspensionManager(iocProvider);
            RegisterPresenter(iocProvider);
            base.InitializeFirstChance(iocProvider);
        }

        protected virtual void InitializeSuspensionManager(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var suspensionManager = CreateSuspensionManager();
            iocProvider.RegisterSingleton(suspensionManager);

            if (_suspensionManagerSessionStateKey != null)
                suspensionManager.RegisterFrame(_rootFrame, _suspensionManagerSessionStateKey);
        }

        protected virtual IMvxSuspensionManager CreateSuspensionManager()
        {
            return new MvxSuspensionManager();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
        {
            var container = CreateStoreViewsContainer();
            iocProvider.RegisterSingleton<IMvxWindowsViewModelRequestTranslator>(container);
            iocProvider.RegisterSingleton<IMvxWindowsViewModelLoader>(container);
            var viewsContainer = container as MvxViewsContainer;
            if (viewsContainer == null)
                throw new MvxException("CreateViewsContainer must return an MvxViewsContainer");
            return container;
        }

        protected virtual IMvxStoreViewsContainer CreateStoreViewsContainer()
        {
            return new MvxWindowsViewsContainer();
        }

        protected IMvxWindowsViewPresenter Presenter
        {
            get
            {
                if (_rootFrame == null)
                    throw new InvalidOperationException("Cannot create View Presenter with null root frame");
                _presenter ??= CreateViewPresenter(_rootFrame);
                return _presenter;
            }
        }

        protected virtual IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            return new MvxWindowsViewPresenter(rootFrame);
        }

        protected virtual MvxWindowsViewDispatcher CreateViewDispatcher(IMvxWindowsFrame rootFrame)
        {
            return new MvxWindowsViewDispatcher(Presenter, rootFrame);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            if (_rootFrame == null)
                throw new InvalidOperationException("Cannot create View Dispatcher with null root frame");
            return CreateViewDispatcher(_rootFrame);
        }

        protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var presenter = Presenter;
            iocProvider.RegisterSingleton(presenter);
            iocProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
        }

        protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            InitializeBindingBuilder(iocProvider);
            base.InitializeLastChance(iocProvider);
        }

        protected virtual void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
            RegisterBindingBuilderCallbacks(iocProvider);
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration(iocProvider);
        }

        protected virtual void RegisterBindingBuilderCallbacks(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            iocProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            iocProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            iocProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
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

        protected IActivatedEventArgs? ActivationArguments { get; private set; }

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

    public abstract class MvxWindowsSetup<TApplication> : MvxWindowsSetup
         where TApplication : class, IMvxApplication, new()
    {
        protected override IMvxApplication CreateApp(IMvxIoCProvider iocProvider) =>
            iocProvider.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
#nullable restore
}
