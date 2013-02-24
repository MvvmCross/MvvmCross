// MvxAndroidSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Cirrious.CrossCore.Droid.Interfaces;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Droid.Platform.Tasks;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Platform
{
    public abstract class MvxAndroidSetup
        : MvxSetup
          , IMvxAndroidGlobals
    {
        private readonly Context _applicationContext;

        protected MvxAndroidSetup(Context applicationContext)
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

        protected override IMvxPluginManager CreatePluginManager()
        {
            return new MvxFileBasedPluginManager("Droid");
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterSingleton<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override void InitializePlatformServices()
        {
            var lifetimeMonitor = new MvxAndroidLifetimeMonitor();
            this.RegisterSingleton<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            this.RegisterSingleton<IMvxAndroidCurrentTopActivity>(lifetimeMonitor);
            this.RegisterSingleton<IMvxLifetime>(lifetimeMonitor);

            this.RegisterSingleton<IMvxAndroidGlobals>(this);

            var intentResultRouter = new MvxIntentResultSink();
            this.RegisterSingleton<IMvxIntentResultSink>(intentResultRouter);
            this.RegisterSingleton<IMvxIntentResultSource>(intentResultRouter);

            InitializeNavigationRequestSerializer();
        }

        protected override sealed MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_applicationContext);
            this.RegisterSingleton<IMvxAndroidViewModelRequestTranslator>(container);
            this.RegisterSingleton<IMvxAndroidViewModelLoader>(container);
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
            this.RegisterSingleton<IMvxAndroidSubViewModelCache>(new MvxAndroidSubViewModelCache());
            InitialiseBindingBuilder();
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

        protected virtual void InitializeNavigationRequestSerializer()
        {
            this.RegisterSingleton(CreateNavigationRequestSerializer());
        }

        protected abstract IMvxNavigationRequestSerializer CreateNavigationRequestSerializer();

        protected virtual void InitialiseBindingBuilder()
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual MvxDroidBindingBuilder CreateBindingBuilder()
        {
            var bindingBuilder = new MvxDroidBindingBuilder(FillTargetFactories, FillValueConverters,
                                                              SetupViewTypeResolver);
            return bindingBuilder;
        }

        protected virtual void SetupViewTypeResolver(MvxViewTypeResolver viewTypeResolver)
        {
            viewTypeResolver.ViewNamespaceAbbreviations = this.ViewNamespaceAbbreviations;
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            var holders = ValueConverterHolders;
            if (holders == null)
                return;

            var filler = new MvxInstanceBasedValueConverterRegistryFiller(registry);
            var staticFiller = new MvxStaticBasedValueConverterRegistryFiller(registry);
            foreach (var converterHolder in holders)
            {
                filler.AddFieldConverters(converterHolder);
                staticFiller.AddStaticFieldConverters(converterHolder);
            }
        }

        protected virtual IEnumerable<Type> ValueConverterHolders
        {
            get { return null; }
        }

        protected virtual IDictionary<string, string> ViewNamespaceAbbreviations
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        {"Mvx", "Cirrious.MvvmCross.Binding.Droid.Views"}
                    };
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // nothing to do in this base class
        }
    }
}