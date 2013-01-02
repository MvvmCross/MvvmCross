// MvxBaseAndroidSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Droid.Platform.Tasks;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Platform
{
    public abstract class MvxBaseAndroidSetup
        : MvxBaseSetup
          , IMvxAndroidGlobals
          , IMvxServiceProducer
    {
        private readonly Context _applicationContext;

        protected MvxBaseAndroidSetup(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        #region IMvxAndroidGlobals Members

        public virtual string ExecutableNamespace
        {
            get { return GetType().Namespace; }
        }

        public virtual Assembly ExecutableAssembly
        {
            get { return GetType().Assembly; }
        }

        public Context ApplicationContext
        {
            get { return _applicationContext; }
        }

        #endregion

        protected override MvvmCross.Interfaces.Plugins.IMvxPluginManager CreatePluginManager()
        {
            return new MvxFileBasedPluginManager("Droid");
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override void InitializePlatformServices()
        {
            var lifetimeMonitor = new MvxAndroidLifetimeMonitor();
            this.RegisterServiceInstance<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            this.RegisterServiceInstance<IMvxAndroidCurrentTopActivity>(lifetimeMonitor);
            this.RegisterServiceInstance<IMvxLifetime>(lifetimeMonitor);

            this.RegisterServiceInstance<IMvxAndroidGlobals>(this);

            var intentResultRouter = new MvxIntentResultSink();
            this.RegisterServiceInstance<IMvxIntentResultSink>(intentResultRouter);
            this.RegisterServiceInstance<IMvxIntentResultSource>(intentResultRouter);
        }

        protected override sealed MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_applicationContext);
            this.RegisterServiceInstance<IMvxAndroidViewModelRequestTranslator>(container);
            this.RegisterServiceInstance<IMvxAndroidViewModelLoader>(container);
            return container;
        }

        protected virtual IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAndroidViewPresenter();
        }

        protected override MvvmCross.Interfaces.Views.IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            var presenter = CreateViewPresenter();
            return new MvxAndroidViewDispatcherProvider(presenter);
        }

        protected override void InitializeLastChance()
        {
            this.RegisterServiceInstance<IMvxAndroidSubViewModelCache>(new MvxAndroidSubViewModelCache());
            base.InitializeLastChance();
        }

        protected virtual MvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new MvxAndroidViewsContainer(applicationContext);
        }

        protected override IDictionary<System.Type, System.Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(ExecutableAssembly, typeof (IMvxAndroidView));
        }
    }
}