// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Threading;
using MvvmCross.Core;
using MvvmCross.Platform.Wpf.Presenters;
using MvvmCross.Platform.Wpf.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platform.Wpf.Core
{

    public abstract class MvxWpfSetup
        : MvxSetup, IMvxWpfSetup
    {
        private Dispatcher _uiThreadDispatcher;
        private ContentControl _root;
        private IMvxWpfViewPresenter _presenter;

        public void PlatformInitialize(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
        {
            _uiThreadDispatcher = uiThreadDispatcher;
            _presenter = presenter;
        }

        public void PlatformInitialize(Dispatcher uiThreadDispatcher, ContentControl root)
        {
            _uiThreadDispatcher = uiThreadDispatcher;
            _root = root;
        }

        protected override void InitializePlatformServices()
        {
            RegisterPresenter();
            base.InitializePlatformServices();
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            return base.GetViewAssemblies().Union(new[] { Assembly.GetEntryAssembly() });
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

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Control");
        }
    }

    public class MvxWpfSetup<TApplication> : MvxWpfSetup
        where TApplication : IMvxApplication, new()
    {
        protected override IMvxApplication CreateApp() => Mvx.IocConstruct<TApplication>();

        protected override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
