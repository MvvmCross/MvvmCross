// MvxStoreSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
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
        private readonly string _suspensionManagerSessionStateKey;

        protected MvxStoreSetup(Frame rootFrame, string suspensionManagerSessionStateKey = null)
        {
            _rootFrame = rootFrame;
            _suspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
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
            return new MvxFilePluginManager(".WindowsStore");
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            return CreateStoreViewsContainer();
        }

        protected virtual IMvxStoreViewsContainer CreateStoreViewsContainer()
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

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }
    }
}