// MvxWinRTSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Cirrious.MvvmCross.WinRT.Views;
using Cirrious.MvvmCross.WinRT.Views.Suspension;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Platform
{
    public abstract class MvxWinRTSetup
        : MvxSetup

    {
        private readonly Frame _rootFrame;

        protected MvxWinRTSetup(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override void InitializeDebugServices()
        {
            Mvx.RegisterSingleton<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
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
        }

        protected virtual IMvxSuspensionManager CreateSuspensionManager()
        {
            return new MvxSuspensionManager();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderBasedPluginManager();
            var registry = new MvxLoaderPluginRegistry(".WinRT", toReturn.Loaders);
            AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            return new MvxWinRTViewsContainer();
        }

        protected override MvvmCross.Interfaces.Views.IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return CreateViewDispatcherProvider(_rootFrame);
        }

        protected virtual IMvxWinRTViewPresenter CreateViewPresenter(Frame rootFrame)
        {
            return new MvxWinRTViewPresenter(rootFrame);
        }

        protected virtual MvxWinRTViewDispatcherProvider CreateViewDispatcherProvider(Frame rootFrame)
        {
            var presenter = CreateViewPresenter(_rootFrame);
            return new MvxWinRTViewDispatcherProvider(presenter, rootFrame);
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().GetTypeInfo().Assembly, typeof (IMvxWinRTView));
        }
    }
}