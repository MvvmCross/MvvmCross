// MvxStoreSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Uwp.Views;
using MvvmCross.Uwp.Views.Suspension;
using MvvmCross.Platform.Converters;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding;
using MvvmCross.Binding.Uwp;
using MvvmCross.Binding.Binders;
using System;
using System.Reflection;

namespace MvvmCross.Uwp.Platform
{
    public abstract class MvxWindowsSetup
        : MvxSetup
    {
        private readonly IMvxWindowsFrame _rootFrame;
        private readonly string _suspensionManagerSessionStateKey;

        protected MvxWindowsSetup(Frame rootFrame, string suspensionManagerSessionStateKey = null)
            : this(new MvxWrappedFrame(rootFrame))
        {
            _suspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
        }

        protected MvxWindowsSetup(IMvxWindowsFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override void InitializePlatformServices()
        {
            InitializeSuspensionManager();
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

        protected override IMvxPluginManager CreatePluginManager()
        {
            return new MvxFilePluginManager(new List<string>() { ".Uwp", ".WindowsCommon" });
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

        protected virtual IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            return new MvxWindowsViewPresenter(rootFrame);
        }

        protected virtual MvxWindowsViewDispatcher CreateViewDispatcher(IMvxWindowsFrame rootFrame)
        {
            var presenter = CreateViewPresenter(_rootFrame);
            return new MvxWindowsViewDispatcher(presenter, rootFrame);
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
