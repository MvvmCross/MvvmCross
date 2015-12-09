// MvxWpfSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Platform
{
    using System.Windows.Threading;

    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;
    using MvvmCross.Wpf.Views;

    public abstract class MvxWpfSetup
        : MvxSetup
    {
        private readonly Dispatcher _uiThreadDispatcher;
        private readonly IMvxWpfViewPresenter _presenter;

        protected MvxWpfSetup(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
        {
            this._uiThreadDispatcher = uiThreadDispatcher;
            this._presenter = presenter;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxTraceTrace();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var toReturn = this.CreateWpfViewsContainer();
            Mvx.RegisterSingleton<IMvxSimpleWpfViewLoader>(toReturn);
            return toReturn;
        }

        protected virtual IMvxWpfViewsContainer CreateWpfViewsContainer()
        {
            return new MvxWpfViewsContainer();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxWpfViewDispatcher(this._uiThreadDispatcher, this._presenter);
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            return new MvxFilePluginManager(".Wpf", string.Empty);
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Control");
        }
    }
}