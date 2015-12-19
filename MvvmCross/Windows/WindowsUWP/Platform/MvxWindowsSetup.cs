// MvxStoreSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsUWP.Platform
{
    using System.Collections.Generic;

    using Windows.UI.Xaml.Controls;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;
    using MvvmCross.WindowsUWP.Views;
    using MvvmCross.WindowsUWP.Views.Suspension;

    public abstract class MvxWindowsSetup
        : MvxSetup
    {
        private readonly IMvxWindowsFrame _rootFrame;
        private readonly string _suspensionManagerSessionStateKey;

        protected MvxWindowsSetup(Frame rootFrame, string suspensionManagerSessionStateKey = null)
            : this(new MvxWrappedFrame(rootFrame))
        {
            this._suspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
        }

        protected MvxWindowsSetup(IMvxWindowsFrame rootFrame)
        {
            this._rootFrame = rootFrame;
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
            return new MvxFilePluginManager(new List<string>() { ".WindowsUWP", ".WindowsCommon" });
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            return this.CreateStoreViewsContainer();
        }

        protected virtual IMvxStoreViewsContainer CreateStoreViewsContainer()
        {
            return new MvxWindowsViewsContainer();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return this.CreateViewDispatcher(this._rootFrame);
        }

        protected virtual IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            return new MvxWindowsViewPresenter(rootFrame);
        }

        protected virtual MvxWindowsViewDispatcher CreateViewDispatcher(IMvxWindowsFrame rootFrame)
        {
            var presenter = this.CreateViewPresenter(this._rootFrame);
            return new MvxWindowsViewDispatcher(presenter, rootFrame);
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Page");
        }
    }
}
