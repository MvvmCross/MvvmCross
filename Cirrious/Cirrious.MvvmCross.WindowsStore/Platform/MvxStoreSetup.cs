// MvxStoreSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsStore.Views;
using Cirrious.MvvmCross.WindowsStore.Views.Suspension;
using Windows.UI.Xaml.Controls;

namespace Cirrious.MvvmCross.WindowsStore.Platform
{
    public abstract class MvxStoreSetup
        : MvxSetup
    {
        private readonly Frame _rootFrame;

        protected MvxStoreSetup(Frame rootFrame)
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
            return new MvxFilePluginManager(".WindowsStore");
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            return new MvxStoreViewsContainer();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return CreateViewDispatcher(_rootFrame);
        }

        protected virtual IMvxStoreViewPresenter CreateViewPresenter(Frame rootFrame)
        {
            return new MvxStoreViewPresenter(rootFrame);
        }

        protected virtual MvxStoreViewDispatcher CreateViewDispatcher(Frame rootFrame)
        {
            var presenter = CreateViewPresenter(_rootFrame);
            return new MvxStoreViewDispatcher(presenter, rootFrame);
        }
    }
}