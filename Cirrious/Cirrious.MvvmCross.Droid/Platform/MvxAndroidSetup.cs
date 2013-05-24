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
using Android.Views;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;
using Cirrious.MvvmCross.Droid.Views;
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
            return new MvxFilePluginManager(".Droid", ".dll");
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override void InitializePlatformServices()
        {
            var lifetimeMonitor = new MvxAndroidLifetimeMonitor();
            Mvx.RegisterSingleton<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            Mvx.RegisterSingleton<IMvxAndroidCurrentTopActivity>(lifetimeMonitor);
            Mvx.RegisterSingleton<IMvxLifetime>(lifetimeMonitor);

            Mvx.RegisterSingleton<IMvxAndroidGlobals>(this);

            var intentResultRouter = new MvxIntentResultSink();
            Mvx.RegisterSingleton<IMvxIntentResultSink>(intentResultRouter);
            Mvx.RegisterSingleton<IMvxIntentResultSource>(intentResultRouter);

            var viewModelTemporaryCache = new MvxSingleViewModelCache();
            Mvx.RegisterSingleton<IMvxSingleViewModelCache>(viewModelTemporaryCache);
        }

        protected virtual void InitializeSavedStateConverter()
        {
            var converter = CreateSavedStateConverter();
            Mvx.RegisterSingleton(converter);
        }

        protected virtual IMvxSavedStateConverter CreateSavedStateConverter()
        {
            return new MvxSavedStateConverter();
        }

        protected override sealed MvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_applicationContext);
            Mvx.RegisterSingleton<IMvxAndroidViewModelRequestTranslator>(container);
            Mvx.RegisterSingleton<IMvxAndroidViewModelLoader>(container);
            return container;
        }

        protected virtual IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAndroidViewPresenter();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            var presenter = CreateViewPresenter();
            return new MvxAndroidViewDispatcher(presenter);
        }

        protected override void InitializeLastChance()
        {
            InitializeSavedStateConverter();

            Mvx.RegisterSingleton<IMvxChildViewModelCache>(new MvxChildViewModelCache());
            InitialiseBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual MvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new MvxAndroidViewsContainer(applicationContext);
        }

        protected virtual void InitialiseBindingBuilder()
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual MvxAndroidBindingBuilder CreateBindingBuilder()
        {
			var bindingBuilder = new MvxAndroidBindingBuilder(
                                        FillTargetFactories, 
                                        FillValueConverters, 
                                        FillBindingNames,
                                        SetupAxmlViewTypeResolver,
                                        SetupNamespaceListViewTypeResolver,
                                        FillViewTypes);
            return bindingBuilder;
        }

        protected virtual void FillViewTypes(IMvxTypeCache<View> cache)
        {
            foreach (var assembly in AndroidViewAssemblies)
            {
                cache.AddAssembly(assembly);                
            }
        }

        protected virtual void FillBindingNames (IMvxBindingNameRegistry obj)
		{
			// this base class does nothing
		}

        protected virtual void SetupAxmlViewTypeResolver(MvxAxmlNameViewTypeResolver viewTypeResolver)
        {
            viewTypeResolver.ViewNamespaceAbbreviations = this.ViewNamespaceAbbreviations;
        }

        protected virtual void SetupNamespaceListViewTypeResolver(MvxNamespaceListViewTypeResolver viewTypeResolver)
        {
            viewTypeResolver.Namespaces = this.ViewNamespaces;
            viewTypeResolver.EnsureAllNamespacesAreLowerCaseAndEndWithPeriod();
        }        

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders
        {
            get { return new List<Type>(); }
        }

        protected virtual List<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                toReturn.Add(typeof(Cirrious.MvvmCross.Localization.MvxLanguageConverter).Assembly);
                return toReturn;
            }
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

        protected virtual IList<string> ViewNamespaces
        {
            get
            {
                return new List<string>
                    {
                        "Android.Views",
                        "Android.Widget",
                        "Android.Webkit",
                        "Cirrious.MvvmCross.Binding.Droid.Views",
                    };
            }
        }

        protected virtual IList<Assembly> AndroidViewAssemblies
        {
            get
            {
                return new List<Assembly>()
                    {
                        typeof (Android.Views.View).Assembly,
                        typeof (Cirrious.MvvmCross.Binding.Droid.Views.MvxDateChangedListener).Assembly,
                        this.GetType().Assembly,
                    };
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // nothing to do in this base class
        }
    }
}