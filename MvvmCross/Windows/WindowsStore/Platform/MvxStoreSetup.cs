// MvxStoreSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsStore.Platform
{
    using Windows.UI.Xaml.Controls;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;
    using MvvmCross.WindowsStore.Views;
    using MvvmCross.WindowsStore.Views.Suspension;

    public abstract class MvxStoreSetup
        : MvxSetup
    {
        private readonly Frame _rootFrame;
        private readonly string _suspensionManagerSessionStateKey;

        protected MvxStoreSetup(Frame rootFrame, string suspensionManagerSessionStateKey = null)
        {
            this._rootFrame = rootFrame;
            this._suspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override void InitializePlatformServices()
        {
            this.InitializeSuspensionManager();
            base.InitializePlatformServices();
        }

        protected virtual void InitializeSuspensionManager()
        {
            var suspensionManager = this.CreateSuspensionManager();
            Mvx.RegisterSingleton(suspensionManager);

            if (this._suspensionManagerSessionStateKey != null)
                suspensionManager.RegisterFrame(this._rootFrame, this._suspensionManagerSessionStateKey);
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
            return this.CreateStoreViewsContainer();
        }

        protected virtual IMvxStoreViewsContainer CreateStoreViewsContainer()
        {
            return new MvxStoreViewsContainer();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return this.CreateViewDispatcher(this._rootFrame);
        }

        protected virtual IMvxStoreViewPresenter CreateViewPresenter(Frame rootFrame)
        {
            return new MvxStoreViewPresenter(rootFrame);
        }

        protected virtual MvxStoreViewDispatcher CreateViewDispatcher(Frame rootFrame)
        {
            var presenter = this.CreateViewPresenter(this._rootFrame);
            return new MvxStoreViewDispatcher(presenter, rootFrame);
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }
    }
}