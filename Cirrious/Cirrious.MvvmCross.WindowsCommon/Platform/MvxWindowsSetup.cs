// MvxStoreSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.WindowsCommon.Views;
using Cirrious.MvvmCross.WindowsCommon.Views.Suspension;

namespace Cirrious.MvvmCross.WindowsCommon.Platform
{
    public abstract class MvxWindowsSetup
        : MvxSetup
    {
        private readonly Frame _rootFrame;

        protected MvxWindowsSetup(Frame rootFrame)
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
        }

        protected virtual IMvxSuspensionManager CreateSuspensionManager()
        {
            return new MvxSuspensionManager();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            return new MvxFilePluginManager(new List<string>() { ".WindowsCommon", ".WindowsStore" });
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            return CreateStoreViewsContainer();
        }

        protected virtual IMvxStoreViewsContainer CreateStoreViewsContainer()
        {
            return new MvxWindowsViewsContainer();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return CreateViewDispatcher(_rootFrame);
        }

        protected virtual IMvxWindowsViewPresenter CreateViewPresenter(Frame rootFrame)
        {
            return new MvxWindowsViewPresenter(rootFrame);
        }

        protected virtual MvxWindowsViewDispatcher CreateViewDispatcher(Frame rootFrame)
        {
            var presenter = CreateViewPresenter(_rootFrame);
            return new MvxWindowsViewDispatcher(presenter, rootFrame);
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }
    }
}