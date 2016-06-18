// MvxAndroidSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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

namespace MvvmCross.Droid.Platform
{
    public abstract class MvxAndroidSetup
        : MvxSetup, IMvxAndroidGlobals
    {
        protected MvxAndroidSetup(Context applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        #region IMvxAndroidGlobals Members

        public virtual string ExecutableNamespace => GetType().Namespace;

        public virtual Assembly ExecutableAssembly => GetType().Assembly;

		public Context ApplicationContext { get; }

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
            InitializeLifetimeMonitor();

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
            var lifetimeMonitor = CreateLifetimeMonitor();
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
            var converter = CreateSavedStateConverter();
            Mvx.RegisterSingleton(converter);
        }

        protected virtual IMvxSavedStateConverter CreateSavedStateConverter()
        {
            return new MvxSavedStateConverter();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(ApplicationContext);
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
            var presenter = CreateViewPresenter();
            Mvx.RegisterSingleton(presenter);
            return new MvxAndroidViewDispatcher(presenter);
        }

        protected override void InitializeLastChance()
        {
            InitializeSavedStateConverter();

            Mvx.RegisterSingleton<IMvxChildViewModelCache>(new MvxChildViewModelCache());
            InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual IMvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new MvxAndroidViewsContainer(applicationContext);
        }

        protected virtual void InitializeBindingBuilder()
        {
            var bindingBuilder = CreateBindingBuilder();
            RegisterBindingBuilderCallbacks();
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
            foreach (var assembly in AndroidViewAssemblies)
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
            foreach (var kvp in ViewNamespaceAbbreviations)
            {
                viewTypeResolver.ViewNamespaceAbbreviations[kvp.Key] = kvp.Value;
            }
        }

        protected virtual void FillNamespaceListViewTypeResolver(IMvxNamespaceListViewTypeResolver viewTypeResolver)
        {
            foreach (var viewNamespace in ViewNamespaces)
            {
                viewTypeResolver.Add(viewNamespace);
            }
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
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
            typeof (View).Assembly,
            typeof (MvvmCross.Binding.Droid.Views.MvxDatePicker).Assembly,
            GetType().Assembly,
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