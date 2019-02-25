// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Android.Views;
using MvvmCross.Converters;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Presenters;
using System.Linq;

namespace MvvmCross.Platforms.Android.Core
{
    public abstract class MvxAndroidSetup
        : MvxSetup, IMvxAndroidGlobals, IMvxAndroidSetup
    {
        private Context _applicationContext;
        private IMvxAndroidViewPresenter _presenter;

        public void PlatformInitialize(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public virtual Assembly ExecutableAssembly => ViewAssemblies?.FirstOrDefault() ?? GetType().Assembly;

        public Context ApplicationContext => _applicationContext;

        protected override void InitializePlatformServices()
        {
            InitializeLifetimeMonitor();
            InitializeAndroidCurrentTopActivity();
            RegisterPresenter();

            Mvx.IoCProvider.RegisterSingleton<IMvxAndroidGlobals>(this);

            var intentResultRouter = new MvxIntentResultSink();
            Mvx.IoCProvider.RegisterSingleton<IMvxIntentResultSink>(intentResultRouter);
            Mvx.IoCProvider.RegisterSingleton<IMvxIntentResultSource>(intentResultRouter);

            var viewModelTemporaryCache = new MvxSingleViewModelCache();
            Mvx.IoCProvider.RegisterSingleton<IMvxSingleViewModelCache>(viewModelTemporaryCache);

            var viewModelMultiTemporaryCache = new MvxMultipleViewModelCache();
            Mvx.IoCProvider.RegisterSingleton<IMvxMultipleViewModelCache>(viewModelMultiTemporaryCache);
            base.InitializePlatformServices();
        }

        protected virtual void InitializeAndroidCurrentTopActivity()
        {
            var currentTopActivity = CreateAndroidCurrentTopActivity();
            Mvx.IoCProvider.RegisterSingleton<IMvxAndroidCurrentTopActivity>(currentTopActivity);
        }

        protected virtual IMvxAndroidCurrentTopActivity CreateAndroidCurrentTopActivity()
        {
            var mvxApplication = MvxAndroidApplication.Instance;
            if (mvxApplication != null)
            {
                var activityLifecycleCallbacksManager = new MvxApplicationCallbacksCurrentTopActivity();
                mvxApplication.RegisterActivityLifecycleCallbacks(activityLifecycleCallbacksManager);
                return activityLifecycleCallbacksManager;
            }
            else
            {
                return new MvxLifecycleMonitorCurrentTopActivity(Mvx.IoCProvider.GetSingleton<IMvxAndroidActivityLifetimeListener>());
            }
        }

        protected virtual void InitializeLifetimeMonitor()
        {
            var lifetimeMonitor = CreateLifetimeMonitor();

            Mvx.IoCProvider.RegisterSingleton<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            Mvx.IoCProvider.RegisterSingleton<IMvxLifetime>(lifetimeMonitor);
        }

        protected virtual MvxAndroidLifetimeMonitor CreateLifetimeMonitor()
        {
            return new MvxAndroidLifetimeMonitor();
        }

        protected virtual void InitializeSavedStateConverter()
        {
            var converter = CreateSavedStateConverter();
            Mvx.IoCProvider.RegisterSingleton(converter);
        }

        protected virtual IMvxSavedStateConverter CreateSavedStateConverter()
        {
            return new MvxSavedStateConverter();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateViewsContainer(_applicationContext);
            Mvx.IoCProvider.RegisterSingleton<IMvxAndroidViewModelRequestTranslator>(container);
            Mvx.IoCProvider.RegisterSingleton<IMvxAndroidViewModelLoader>(container);
            var viewsContainer = container as MvxViewsContainer;
            if (viewsContainer == null)
                throw new MvxException("CreateViewsContainer must return an MvxViewsContainer");
            return viewsContainer;
        }

        protected IMvxAndroidViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAndroidViewPresenter(AndroidViewAssemblies);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxAndroidViewDispatcher(Presenter);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.IoCProvider.RegisterSingleton(presenter);
            Mvx.IoCProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
        }

        protected override void InitializeLastChance()
        {
            InitializeSavedStateConverter();

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
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTypeCache<View>>(FillViewTypes);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxAxmlNameViewTypeResolver>(FillAxmlViewTypeResolver);
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxNamespaceListViewTypeResolver>(FillNamespaceListViewTypeResolver);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
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
            { "Mvx", "MvvmCross.Platforms.Android.Views" }
        };

        protected virtual IEnumerable<string> ViewNamespaces => new List<string>
        {
            "Android.Views",
            "Android.Widget",
            "Android.Webkit",
            "MvvmCross.Platforms.Android.Views",
        };

        protected virtual IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>()
        {
            typeof(View).Assembly,
            typeof(MvxDatePicker).Assembly,
            GetType().Assembly,
        };

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // nothing to do in this base class
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "Activity", "Fragment");
        }
    }

    public class MvxAndroidSetup<TApplication> : MvxAndroidSetup
        where TApplication : class, IMvxApplication, new()
    {
        protected override IMvxApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
