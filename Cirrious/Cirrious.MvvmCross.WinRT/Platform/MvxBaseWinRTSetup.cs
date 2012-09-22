#region Copyright
// <copyright file="MvxBaseWinRTSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WinRT.Interfaces;
using Cirrious.MvvmCross.WinRT.Views;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WinRT.Platform
{

    public abstract class MvxBaseWinRTSetup 
        : MvxBaseSetup
        , IMvxServiceProducer<IMvxTrace>
    {
        private readonly Frame _rootFrame;

        protected MvxBaseWinRTSetup(Frame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override void InitializeDefaultTextSerializer()
        {
            Cirrious.MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
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
            return GetViewModelViewLookup(GetType().GetTypeInfo().Assembly, typeof(IMvxWinRTView));
        }
    }
}