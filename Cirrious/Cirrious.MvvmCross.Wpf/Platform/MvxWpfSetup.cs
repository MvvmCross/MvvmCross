// MvxBaseWpfSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Windows.Threading;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Wpf.Interfaces;
using Cirrious.MvvmCross.Wpf.Views;

namespace Cirrious.MvvmCross.Wpf.Platform
{
    public abstract class MvxWpfSetup
        : MvxSetup
          , IMvxServiceProducer
    {
        private readonly Dispatcher _uiThreadDispatcher;
        private readonly IMvxWpfViewPresenter _presenter;

        protected MvxWpfSetup(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
        {
            _uiThreadDispatcher = uiThreadDispatcher;
            _presenter = presenter;
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override MvxViewsContainer CreateViewsContainer()
        {
            return CreateAndRegisterSimpleWpfViewContainer();
        }

        private MvxViewsContainer CreateAndRegisterSimpleWpfViewContainer()
        {
            var toReturn = new MvxWpfViewsContainer();
            this.RegisterServiceInstance<IMvxSimpleWpfViewLoader>(toReturn);
            return toReturn;
        }

        protected override IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return new MvxWpfDispatcherProvider(_uiThreadDispatcher, _presenter);
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            return new MvxFileBasedPluginManager("Wpf", string.Empty);
        }

        protected override IDictionary<System.Type, System.Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof (IMvxWpfView));
        }
    }
}