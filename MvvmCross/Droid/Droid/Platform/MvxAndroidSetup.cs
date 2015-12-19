// MvxAndroidSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Android.Content;
    using Android.Views;

    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Binding.Droid;
    using MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;
    using MvvmCross.Core.Platform;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Droid.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Droid;
    using MvvmCross.Platform.Droid.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.IoC;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;

    public abstract class MvxAndroidSetup
        : MvxSetup
          , IMvxAndroidGlobals
    {
        private readonly Context _applicationContext;

        protected MvxAndroidSetup(Context applicationContext)
        {
            this._applicationContext = applicationContext;
        }

        #region IMvxAndroidGlobals Members

        public virtual string ExecutableNamespace => this.GetType().Namespace;

        public virtual Assembly ExecutableAssembly => this.GetType().Assembly;

        public Context ApplicationContext => this._applicationContext;

        #endregion IMvxAndroidGlobals Members

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
            this.InitializeLifetimeMonitor();

            Mvx.RegisterSingleton<IMvxAndroidGlobals>(this);

            var intentResultRouter = new MvxIntentResultSink();
            Mvx.RegisterSingleton<IMvxIntentResultSink>(intentResultRouter);
            Mvx.RegisterSingleton<IMvxIntentResultSource>(intentResultRouter);

            var viewModelTemporaryCache = new MvxSingleViewModelCache();
            Mvx.RegisterSingleton<IMvxSingleViewModelCache>(viewModelTemporaryCache);

            var viewModelMultiTemporaryCache = new MvxMultipleViewModelCache();
            Mvx.RegisterSingleton<IMvxMultipleViewModelCache>(viewModelMultiTemporaryCache);
        }

        protected virtual void InitializeLifetimeMonitor()
        {
            var lifetimeMonitor = this.CreateLifetimeMonitor();
            Mvx.RegisterSingleton<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            Mvx.RegisterSingleton<IMvxAndroidCurrentTopActivity>(lifetimeMonitor);
            Mvx.RegisterSingleton<IMvxLifetime>(lifetimeMonitor);
        }

        protected virtual MvxAndroidLifetimeMonitor CreateLifetimeMonitor()
        {
            return new MvxAndroidLifetimeMonitor();
        }

        protected virtual void InitializeSavedStateConverter()
        {
            var converter = this.CreateSavedStateConverter();
            Mvx.RegisterSingleton(converter);
        }

        protected virtual IMvxSavedStateConverter CreateSavedStateConverter()
        {
            return new MvxSavedStateConverter();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = this.CreateViewsContainer(this._applicationContext);
            Mvx.RegisterSingleton<IMvxAndroidViewModelRequestTranslator>(container);
            Mvx.RegisterSingleton<IMvxAndroidViewModelLoader>(container);
            var viewsContainer = container as MvxViewsContainer;
            if (viewsContainer == null)
                throw new MvxException("CreateViewsContainer must return an MvxViewsContainer");
            return viewsContainer;
        }

        protected virtual IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAndroidViewPresenter();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            var presenter = this.CreateViewPresenter();
            return new MvxAndroidViewDispatcher(presenter);
        }

        protected override void InitializeLastChance()
        {
            this.InitializeSavedStateConverter();

            Mvx.RegisterSingleton<IMvxChildViewModelCache>(new MvxChildViewModelCache());
            this.InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual IMvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new MvxAndroidViewsContainer(applicationContext);
        }

        protected virtual void InitializeBindingBuilder()
        {
            var bindingBuilder = this.CreateBindingBuilder();
            this.RegisterBindingBuilderCallbacks();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
            Mvx.CallbackWhenRegistered<IMvxTypeCache<View>>(FillViewTypes);
            Mvx.CallbackWhenRegistered<IMvxAxmlNameViewTypeResolver>(FillAxmlViewTypeResolver);
            Mvx.CallbackWhenRegistered<IMvxNamespaceListViewTypeResolver>(FillNamespaceListViewTypeResolver);
        }

        protected virtual MvxAndroidBindingBuilder CreateBindingBuilder()
        {
            var bindingBuilder = new MvxAndroidBindingBuilder();
            return bindingBuilder;
        }

        protected virtual void FillViewTypes(IMvxTypeCache<View> cache)
        {
            foreach (var assembly in this.AndroidViewAssemblies)
            {
                cache.AddAssembly(assembly);
            }
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillAxmlViewTypeResolver(IMvxAxmlNameViewTypeResolver viewTypeResolver)
        {
            foreach (var kvp in this.ViewNamespaceAbbreviations)
            {
                viewTypeResolver.ViewNamespaceAbbreviations[kvp.Key] = kvp.Value;
            }
        }

        protected virtual void FillNamespaceListViewTypeResolver(IMvxNamespaceListViewTypeResolver viewTypeResolver)
        {
            foreach (var viewNamespace in this.ViewNamespaces)
            {
                viewTypeResolver.Add(viewNamespace);
            }
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(this.ValueConverterAssemblies);
            registry.Fill(this.ValueConverterHolders);
        }

        protected virtual IEnumerable<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual IDictionary<string, string> ViewNamespaceAbbreviations => new Dictionary<string, string>
        {
            {"Mvx", "MvvmCross.Binding.Droid.Views"}
        };

        protected virtual IEnumerable<string> ViewNamespaces => new List<string>
        {
            "Android.Views",
            "Android.Widget",
            "Android.Webkit",
            "MvvmCross.Binding.Droid.Views",
        };

        protected virtual IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>()
        {
            typeof (Android.Views.View).Assembly,
            typeof (MvvmCross.Binding.Droid.Views.MvxDatePicker).Assembly,
            this.GetType().Assembly,
        };

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // nothing to do in this base class
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Activity");
        }
    }
}