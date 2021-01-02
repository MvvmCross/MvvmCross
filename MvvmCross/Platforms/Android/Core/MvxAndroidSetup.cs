﻿// Licensed to the .NET Foundation under one or more agreements.
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
#nullable enable
    public abstract class MvxAndroidSetup
        : MvxSetup, IMvxAndroidGlobals, IMvxAndroidSetup
    {
        private Context? _applicationContext;
        private IMvxAndroidViewPresenter? _presenter;

        public void PlatformInitialize(Context applicationContext)
        {
            if (applicationContext == null)
                throw new ArgumentNullException(nameof(applicationContext));

            _applicationContext = applicationContext;
        }

        public virtual Assembly ExecutableAssembly => ViewAssemblies?.FirstOrDefault() ?? GetType().Assembly;

        public Context? ApplicationContext => _applicationContext;

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            InitializeLifetimeMonitor(iocProvider);
            InitializeAndroidCurrentTopActivity(iocProvider);
            RegisterPresenter(iocProvider);

            iocProvider.RegisterSingleton<IMvxAndroidGlobals>(this);

            var intentResultRouter = new MvxIntentResultSink();
            iocProvider.RegisterSingleton<IMvxIntentResultSink>(intentResultRouter);
            iocProvider.RegisterSingleton<IMvxIntentResultSource>(intentResultRouter);

            var viewModelTemporaryCache = new MvxSingleViewModelCache();
            iocProvider.RegisterSingleton<IMvxSingleViewModelCache>(viewModelTemporaryCache);

            var viewModelMultiTemporaryCache = new MvxMultipleViewModelCache();
            iocProvider.RegisterSingleton<IMvxMultipleViewModelCache>(viewModelMultiTemporaryCache);
            base.InitializeFirstChance(iocProvider);
        }

        protected virtual void InitializeAndroidCurrentTopActivity(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var currentTopActivity = CreateAndroidCurrentTopActivity();
            iocProvider.RegisterSingleton<IMvxAndroidCurrentTopActivity>(currentTopActivity);
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

        protected virtual void InitializeLifetimeMonitor(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var lifetimeMonitor = CreateLifetimeMonitor();

            iocProvider.RegisterSingleton<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            iocProvider.RegisterSingleton<IMvxLifetime>(lifetimeMonitor);
        }

        protected virtual MvxAndroidLifetimeMonitor CreateLifetimeMonitor()
        {
            return new MvxAndroidLifetimeMonitor();
        }

        protected virtual void InitializeSavedStateConverter(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var converter = CreateSavedStateConverter();
            iocProvider.RegisterSingleton(converter);
        }

        protected virtual IMvxSavedStateConverter CreateSavedStateConverter()
        {
            return new MvxSavedStateConverter();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            if (_applicationContext == null)
                throw new InvalidOperationException("Cannot create Views Container without ApplicationContext");

            var container = CreateViewsContainer(_applicationContext);
            iocProvider.RegisterSingleton<IMvxAndroidViewModelRequestTranslator>(container);
            iocProvider.RegisterSingleton<IMvxAndroidViewModelLoader>(container);
            var viewsContainer = container as MvxViewsContainer;
            if (viewsContainer == null)
                throw new MvxException("CreateViewsContainer must return an MvxViewsContainer");
            return viewsContainer;
        }

        protected virtual IMvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new MvxAndroidViewsContainer(applicationContext);
        }

        protected IMvxAndroidViewPresenter Presenter
        {
            get
            {
                _presenter ??= CreateViewPresenter();
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

        protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var presenter = Presenter;
            iocProvider.RegisterSingleton(presenter);
            iocProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
        }

        protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            InitializeSavedStateConverter(iocProvider);

            InitializeBindingBuilder(iocProvider);
            base.InitializeLastChance(iocProvider);
        }

        protected virtual void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var bindingBuilder = CreateBindingBuilder();
            RegisterBindingBuilderCallbacks(iocProvider);
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            iocProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            iocProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            iocProvider.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
            iocProvider.CallbackWhenRegistered<IMvxTypeCache<View>>(FillViewTypes);
            iocProvider.CallbackWhenRegistered<IMvxAxmlNameViewTypeResolver>(FillAxmlViewTypeResolver);
            iocProvider.CallbackWhenRegistered<IMvxNamespaceListViewTypeResolver>(FillNamespaceListViewTypeResolver);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxAndroidBindingBuilder();
        }

        protected virtual void FillViewTypes(IMvxTypeCache<View> cache)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

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
            if (viewTypeResolver == null)
                throw new ArgumentNullException(nameof(viewTypeResolver));

            foreach (var kvp in ViewNamespaceAbbreviations)
            {
                viewTypeResolver.ViewNamespaceAbbreviations[kvp.Key] = kvp.Value;
            }
        }

        protected virtual void FillNamespaceListViewTypeResolver(IMvxNamespaceListViewTypeResolver viewTypeResolver)
        {
            if (viewTypeResolver == null)
                throw new ArgumentNullException(nameof(viewTypeResolver));

            foreach (var viewNamespace in ViewNamespaces)
            {
                viewTypeResolver.Add(viewNamespace);
            }
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

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
            { "Mvx", "mvvmcross.platforms.android.binding.views" }
        };

        protected virtual IEnumerable<string> ViewNamespaces => new List<string>
        {
            "Android.Views",
            "Android.Widget",
            "Android.Webkit",
            "MvvmCross.Platforms.Android.Views",
            "MvvmCross.Platforms.Android.Binding.Views"
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
#nullable restore
}
