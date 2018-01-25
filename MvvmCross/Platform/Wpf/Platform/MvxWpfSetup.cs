// MvxWpfSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows.Threading;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters;
using System.Windows.Controls;

namespace MvvmCross.Wpf.Platform
{
    public abstract class MvxWpfSetup
        : MvxSetup
    {
        private readonly Dispatcher _uiThreadDispatcher;
        private readonly ContentControl _root;
        private IMvxWpfViewPresenter _presenter;

        protected MvxWpfSetup(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
        {
            _uiThreadDispatcher = uiThreadDispatcher;
            _presenter = presenter;
        }

        protected MvxWpfSetup(Dispatcher uiThreadDispatcher, ContentControl root)
        {
            _uiThreadDispatcher = uiThreadDispatcher;
            _root = root;
        }

        protected override void InitializePlatformServices()
        {
            RegisterPresenter();
            base.InitializePlatformServices();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var toReturn = CreateWpfViewsContainer();
            Mvx.RegisterSingleton<IMvxWpfViewLoader>(toReturn);
            return toReturn;
        }

        protected virtual IMvxWpfViewsContainer CreateWpfViewsContainer()
        {
            return new MvxWpfViewsContainer();
        }

        protected IMvxWpfViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreateViewPresenter(_root);
                return _presenter;
            }
        }

        protected virtual IMvxWpfViewPresenter CreateViewPresenter(ContentControl root)
        {
            return new MvxWpfViewPresenter(root);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxWpfViewDispatcher(_uiThreadDispatcher, Presenter);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.RegisterSingleton(presenter);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);
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
