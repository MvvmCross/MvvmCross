// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Content;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Converters;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Core
{
    public abstract class MvxAndroidSetup
        : MvxSetup, IMvxAndroidGlobals, IMvxAndroidSetup
    {
        private Context _applicationContext;

        public void PlatformInitialize(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public virtual Assembly ExecutableAssembly => ViewAssemblies.FirstOrDefault() ?? GetType().Assembly;

        public Context ApplicationContext => _applicationContext;

        protected override void RegisterImplementations()
        {
            base.RegisterImplementations();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxIntentResultSink, MvxIntentResultSink>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxIntentResultSink>(intentResultRouter => Mvx.IoCProvider.RegisterSingleton((IMvxIntentResultSource)intentResultRouter));
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxSingleViewModelCache, MvxSingleViewModelCache>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxMultipleViewModelCache, MvxMultipleViewModelCache>();
        }

        protected override void RegisterViewPresenter()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewPresenter, MvxAndroidViewPresenter>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewPresenter>(presenter =>
            {
                if (presenter is IMvxAndroidViewPresenter droidPresenter)
                {
                    droidPresenter.AndroidViewAssemblies = AndroidViewAssemblies;
                    Mvx.IoCProvider.RegisterSingleton(droidPresenter);
                }
            });
        }

        protected override void RegisterViewsContainer()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewsContainer, MvxAndroidViewsContainer>();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxViewsContainer>(container =>
            {
                Mvx.IoCProvider.RegisterSingleton((IMvxAndroidViewModelRequestTranslator)container);
                Mvx.IoCProvider.RegisterSingleton((IMvxAndroidViewModelLoader)container);
                var viewsContainer = container as MvxViewsContainer;
                if (viewsContainer == null)
                    throw new MvxException("CreateViewsContainer must return an MvxViewsContainer");
            });
        }

        protected override void RegisterViewDispatcher()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewDispatcher, MvxAndroidViewDispatcher>();
        }

        protected override void InitializePlatformServices()
        {
            InitializeLifetimeMonitor();
            InitializeAndroidCurrentTopActivity();

            Mvx.IoCProvider.RegisterSingleton<IMvxAndroidGlobals>(this);

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

        protected IMvxAndroidViewPresenter AndroidPresenter
        {
            get
            {
                return base.ViewPresenter as IMvxAndroidViewPresenter;
            }
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
            { "Mvx", "MvvmCross.Binding.Droid.Views" }
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
        protected override void RegisterApp()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxApplication, TApplication>();
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
