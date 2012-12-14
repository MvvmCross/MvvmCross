#region Copyright
// <copyright file="MvxBaseWpfSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using System.Windows.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Wpf.Interfaces;
using Cirrious.MvvmCross.Wpf.Views;

namespace Cirrious.MvvmCross.Wpf.Platform
{
    public abstract class MvxBaseWpfSetup 
        : MvxBaseSetup        
        , IMvxServiceProducer
    {
        private readonly Dispatcher _uiThreadDispatcher;
        private readonly IMvxWpfViewPresenter _presenter;

        protected MvxBaseWpfSetup(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter)
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
            return GetViewModelViewLookup(GetType().Assembly, typeof(IMvxWpfView));
        }
    }
}