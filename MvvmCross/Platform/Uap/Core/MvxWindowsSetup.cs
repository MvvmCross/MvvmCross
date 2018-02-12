// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Converters;
using MvvmCross.Exceptions;
using MvvmCross.Plugin;
using MvvmCross.Core;
using MvvmCross.Platform.Uap.Binding;
using MvvmCross.Platform.Uap.Presenters;
using MvvmCross.Platform.Uap.Views;
using MvvmCross.Platform.Uap.Views.Suspension;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Binders;
using MvvmCross.Presenters;

namespace MvvmCross.Platform.Uap.Core
{
    public abstract class MvxWindowsSetup
        : MvxSetup
    {
        private readonly IMvxWindowsFrame _rootFrame;
        private readonly string _suspensionManagerSessionStateKey;
        private IMvxWindowsViewPresenter _presenter;

        protected MvxWindowsSetup(Frame rootFrame, IActivatedEventArgs activatedEventArgs,
            string suspensionManagerSessionStateKey = null) : this(rootFrame, suspensionManagerSessionStateKey)
        {
            ActivationArguments = activatedEventArgs;
        }

        protected MvxWindowsSetup(Frame rootFrame, string suspensionManagerSessionStateKey = null)
            : this(new MvxWrappedFrame(rootFrame))
        {
            _suspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
        }

        protected MvxWindowsSetup(IMvxWindowsFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override void InitializePlatformServices()
        {
            InitializeSuspensionManager();
            RegisterPresenter();
            base.InitializePlatformServices();
        }

        protected virtual void InitializeSuspensionManager()
        {
            var suspensionManager = CreateSuspensionManager();
            Mvx.RegisterSingleton(suspensionManager);

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
            Mvx.RegisterSingleton<IMvxWindowsViewModelRequestTranslator>(container);
            Mvx.RegisterSingleton<IMvxWindowsViewModelLoader>(container);
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
            return CreateViewDispatcher(_rootFrame);
        }
        
        protected IMvxWindowsViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreateViewPresenter(_rootFrame);
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

        protected IActivatedEventArgs ActivationArguments { get; }

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
}
