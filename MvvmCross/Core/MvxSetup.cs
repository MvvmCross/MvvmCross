﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Logging.LogProviders;
using MvvmCross.Navigation;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Core
{
#nullable enable
    public abstract class MvxSetup : IMvxSetup
    {
        public event EventHandler<MvxSetupStateEventArgs>? StateChanged;

        private MvxSetupState _state;
        private IMvxIoCProvider? _iocProvider;

        protected static Action<IMvxIoCProvider>? RegisterSetupDependencies { get; set; }

        protected static Func<IMvxSetup>? SetupCreator { get; set; }

        protected static List<Assembly> ViewAssemblies { get; } = new List<Assembly>();

        protected IMvxLog? SetupLog { get; private set; }

        public MvxSetupState State
        {
            get => _state;
            private set
            {
                _state = value;
                FireStateChange(value);
            }
        }

        public static void RegisterSetupType<TMvxSetup>(params Assembly[] assemblies) where TMvxSetup : MvxSetup, new()
        {
            ViewAssemblies.AddRange(assemblies);
            if (ViewAssemblies.Count == 0)
            {
                // fall back to all assemblies. Assembly.GetEntryAssembly() always returns
                // null on Xamarin platforms do not use it!
                ViewAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());
            }

            // Avoid creating the instance of Setup right now, instead
            // take a reference to the type in a way that we can avoid
            // using reflection to create the instance.
            SetupCreator = () => new TMvxSetup();
        }

        public static IMvxSetup Instance()
        {
            var instance = SetupCreator?.Invoke();
            if (instance == null)
            {
                instance = MvxSetupExtensions.CreateSetup<MvxSetup>();
            }
            return instance;
        }

        protected abstract IMvxApplication CreateApp();

        protected abstract IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider);

        protected abstract IMvxViewDispatcher CreateViewDispatcher();

        public virtual void InitializePrimary()
        {
            if (State != MvxSetupState.Uninitialized)
            {
                return;
            }

            State = MvxSetupState.InitializingPrimary;
            _iocProvider = InitializeIoC();

            // Register the default setup dependencies before
            // invoking the static call back.
            // Developers can either extend the MvxSetup and override
            // the RegisterDefaultSetupDependencies method, or can provide a
            // callback method by setting the RegisterSetupDependencies method
            RegisterDefaultSetupDependencies(_iocProvider);
            RegisterSetupDependencies?.Invoke(_iocProvider);
            InitializeLoggingServices(_iocProvider);
            SetupLog.Trace("Setup: Primary start");
            SetupLog.Trace("Setup: FirstChance start");
            InitializeFirstChance(_iocProvider);
            SetupLog.Trace("Setup: MvvmCross settings start");
            InitializeSettings(_iocProvider);
            SetupLog.Trace("Setup: Singleton Cache start");
            InitializeSingletonCache();
            SetupLog.Trace("Setup: ViewDispatcher start");
            InitializeViewDispatcher(_iocProvider);
            State = MvxSetupState.InitializedPrimary;
        }

        public virtual void InitializeSecondary()
        {
            if (State != MvxSetupState.InitializedPrimary)
            {
                return;
            }

            if (_iocProvider == null)
            {
                throw new InvalidOperationException("Cannot continue setup with null IoCProvider");
            }

            State = MvxSetupState.InitializingSecondary;
            SetupLog.Trace("Setup: Bootstrap actions");
            PerformBootstrapActions();
            SetupLog.Trace("Setup: StringToTypeParser start");
            InitializeStringToTypeParser(_iocProvider);
            SetupLog.Trace("Setup: FillableStringToTypeParser start");
            InitializeFillableStringToTypeParser(_iocProvider);
            SetupLog.Trace("Setup: PluginManagerFramework start");
            var pluginManager = InitializePluginFramework(_iocProvider);
            SetupLog.Trace("Setup: Create App");
            var app = InitializeMvxApplication(_iocProvider);
            SetupLog.Trace("Setup: NavigationService");
            InitializeNavigationService(_iocProvider);
            SetupLog.Trace("Setup: App start");
            InitializeApp(pluginManager, app);
            SetupLog.Trace("Setup: ViewModelTypeFinder start");
            InitializeViewModelTypeFinder(_iocProvider);
            SetupLog.Trace("Setup: ViewsContainer start");
            InitializeViewsContainer(_iocProvider);
            SetupLog.Trace("Setup: Lookup Dictionary start");
            var lookup = InitializeLookupDictionary(_iocProvider);
            SetupLog.Trace("Setup: Views start");
            InitializeViewLookup(lookup, _iocProvider);
            SetupLog.Trace("Setup: CommandCollectionBuilder start");
            InitializeCommandCollectionBuilder(_iocProvider);
            SetupLog.Trace("Setup: NavigationSerializer start");
            InitializeNavigationSerializer(_iocProvider);
            SetupLog.Trace("Setup: InpcInterception start");
            InitializeInpcInterception(_iocProvider);
            SetupLog.Trace("Setup: InpcInterception start");
            InitializeViewModelCache(_iocProvider);
            SetupLog.Trace("Setup: LastChance start");
            InitializeLastChance(_iocProvider);
            SetupLog.Trace("Setup: Secondary end");
            State = MvxSetupState.Initialized;
        }

        protected virtual void InitializeSingletonCache()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            MvxSingletonCache.Initialize();
#pragma warning restore CA2000 // Dispose objects before losing scope
        }

        protected virtual void InitializeInpcInterception(IMvxIoCProvider iocProvider)
        {
            // by default no Inpc calls are intercepted
        }

        protected virtual IMvxChildViewModelCache InitializeViewModelCache(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var cache = CreateViewModelCache();
            return cache;
        }

        protected virtual IMvxChildViewModelCache CreateViewModelCache()
        {
            return Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
        }

        protected virtual IMvxSettings InitializeSettings(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var settings = CreateSettings(iocProvider);
            return settings;
        }

        protected virtual IMvxSettings CreateSettings(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxSettings>();
        }

        protected virtual IMvxStringToTypeParser InitializeStringToTypeParser(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateStringToTypeParser(iocProvider);
        }

        protected virtual IMvxStringToTypeParser CreateStringToTypeParser(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxStringToTypeParser>();
        }

        protected virtual IMvxFillableStringToTypeParser? InitializeFillableStringToTypeParser(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var parser = CreateFillableStringToTypeParser(iocProvider);
            iocProvider.RegisterSingleton(parser);
            return parser;
        }

        protected virtual IMvxFillableStringToTypeParser? CreateFillableStringToTypeParser(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxStringToTypeParser>() as IMvxFillableStringToTypeParser;
        }

        protected virtual void PerformBootstrapActions()
        {
            var bootstrapRunner = new MvxBootstrapRunner();
            foreach (var assembly in GetBootstrapOwningAssemblies())
            {
                bootstrapRunner.Run(assembly);
            }
        }

        protected virtual IMvxNavigationSerializer InitializeNavigationSerializer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateNavigationSerializer(iocProvider);
        }

        protected virtual IMvxNavigationSerializer CreateNavigationSerializer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxNavigationSerializer>();
        }

        protected virtual IMvxCommandCollectionBuilder InitializeCommandCollectionBuilder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateCommandCollectionBuilder(iocProvider);
        }

        protected virtual IMvxCommandCollectionBuilder CreateCommandCollectionBuilder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxCommandCollectionBuilder>();
        }

        protected virtual IMvxIoCProvider InitializeIoC()
        {
            // initialize the IoC registry, then add it to itself
            var iocProvider = CreateIocProvider();
            iocProvider.RegisterSingleton(iocProvider);
            iocProvider.RegisterSingleton<IMvxSetup>(this);
            return iocProvider;
        }

        protected virtual void RegisterDefaultSetupDependencies(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            RegisterLogProvider(iocProvider);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxSettings, MvxSettings>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxStringToTypeParser, MvxStringToTypeParser>();
            iocProvider.RegisterSingleton<IMvxPluginManager>(() => new MvxPluginManager(GetPluginConfiguration));
            iocProvider.RegisterSingleton(CreateApp);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelLoader, MvxViewModelLoader>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxNavigationService, IMvxViewModelLoader, IMvxViewDispatcher>((loader, dispatcher) =>
                new MvxNavigationService(loader, dispatcher));
            iocProvider.RegisterSingleton(() => new MvxViewModelByNameLookup());
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelByNameLookup, MvxViewModelByNameLookup>(nameLookup => nameLookup);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelByNameRegistry, MvxViewModelByNameLookup>(nameLookup => nameLookup);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelTypeFinder, MvxViewModelViewTypeFinder>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxTypeToTypeLookupBuilder, MvxViewModelViewLookupBuilder>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxCommandCollectionBuilder, MvxCommandCollectionBuilder>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxNavigationSerializer, MvxStringDictionaryNavigationSerializer>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxChildViewModelCache, MvxChildViewModelCache>();

            iocProvider.RegisterType<IMvxCommandHelper, MvxWeakCommandHelper>();
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions();
        }

        protected virtual IMvxIoCProvider CreateIocProvider()
        {
            return MvxIoCProvider.Initialize(CreateIocOptions());
        }

        protected virtual void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            // always the very first thing to get initialized - after IoC and base platform
            // base class implementation is empty by default
        }

        protected virtual void InitializeLoggingServices(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var logProvider = CreateLogProvider(iocProvider);
            SetupLog = logProvider.GetLogFor<MvxSetup>();
            var globalLog = logProvider.GetLogFor<MvxLog>();
            MvxLog.Instance = globalLog;
            iocProvider.RegisterSingleton(globalLog);
        }

        public virtual MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Console;

        protected virtual void RegisterLogProvider(IMvxIoCProvider iocProvider)
        {
            if (iocProvider == null)
                throw new ArgumentNullException(nameof(iocProvider));

            Func<IMvxLogProvider>? logProviderCreator = GetDefaultLogProviderType() switch
            {
                MvxLogProviderType.Console => () => new ConsoleLogProvider(),
                MvxLogProviderType.EntLib => () => new EntLibLogProvider(),
                MvxLogProviderType.Log4Net => () => new Log4NetLogProvider(),
                MvxLogProviderType.Loupe => () => new LoupeLogProvider(),
                MvxLogProviderType.NLog => () => new NLogLogProvider(),
                MvxLogProviderType.Serilog => () => new SerilogLogProvider(),
                _ => null,
            };

            if (logProviderCreator != null)
                iocProvider.RegisterSingleton(logProviderCreator);
        }

        protected virtual IMvxLogProvider CreateLogProvider(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxLogProvider>();
        }

        protected virtual IMvxViewModelLoader CreateViewModelLoader(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxViewModelLoader>();
        }

        protected virtual IMvxNavigationService CreateNavigationService(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxNavigationService>();
        }

        protected virtual IMvxPluginManager InitializePluginFramework(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var pluginManager = CreatePluginManager(iocProvider);
            LoadPlugins(pluginManager);
            return pluginManager;
        }

        protected virtual IMvxPluginManager CreatePluginManager(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxPluginManager>();
        }

        protected virtual IMvxPluginConfiguration? GetPluginConfiguration(Type plugin)
        {
            return null;
        }

        public virtual IEnumerable<Assembly> GetPluginAssemblies()
        {
            var mvvmCrossAssemblyName = typeof(MvxPluginAttribute).Assembly.GetName().Name;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies
                .AsParallel()
                .Where(asmb => AssemblyReferencesMvvmCross(asmb, mvvmCrossAssemblyName));
        }

        private static bool AssemblyReferencesMvvmCross(Assembly assembly, string mvvmCrossAssemblyName)
        {
            try
            {
                return assembly.GetReferencedAssemblies().Any(a => a.Name == mvvmCrossAssemblyName);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return false;
            }
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
            if (pluginManager == null)
                throw new ArgumentNullException(nameof(pluginManager));

            var pluginAttribute = typeof(MvxPluginAttribute);
            var pluginAssemblies = GetPluginAssemblies();

            //Search Assemblies for Plugins
            foreach (var pluginAssembly in pluginAssemblies)
            {
                var assemblyTypes = pluginAssembly.ExceptionSafeGetTypes();

                //Search Types for Valid Plugin
                foreach (var type in assemblyTypes)
                {
                    if (TypeContainsPluginAttribute(type))
                    {
                        //Ensure Plugin has been loaded
                        pluginManager.EnsurePluginLoaded(type);
                    }
                }
            }

            bool TypeContainsPluginAttribute(Type type) =>
                (type.GetCustomAttributes(pluginAttribute, false)?.Length ?? 0) > 0;
        }

        protected virtual IMvxApplication CreateMvxApplication(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxApplication>();
        }

        protected virtual IMvxApplication InitializeMvxApplication(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var app = CreateMvxApplication(iocProvider);
            iocProvider.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
            return app;
        }

        protected virtual void InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.LoadPlugins(pluginManager);
            SetupLog.Trace("Setup: Application Initialize - On background thread");
            app.Initialize();
        }

        protected virtual IMvxViewsContainer InitializeViewsContainer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var container = CreateViewsContainer(iocProvider);
            iocProvider.RegisterSingleton(container);
            return container;
        }

        protected virtual void InitializeViewDispatcher(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var dispatcher = CreateViewDispatcher();
            iocProvider.RegisterSingleton(dispatcher);
            iocProvider.RegisterSingleton<IMvxMainThreadAsyncDispatcher>(dispatcher);
            iocProvider.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
        }

        protected virtual IMvxNavigationService InitializeNavigationService(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            CreateViewModelLoader(iocProvider);
            var navigationService = CreateNavigationService(iocProvider);
            SetupLog.Trace("Setup: Load navigation routes");
            LoadNavigationServiceRoutes(navigationService, iocProvider);
            return navigationService;
        }

        protected virtual void LoadNavigationServiceRoutes(IMvxNavigationService navigationService, IMvxIoCProvider iocProvider)
        {
            if (navigationService == null)
                throw new ArgumentNullException(nameof(navigationService));

            ValidateArguments(iocProvider);

            navigationService.LoadRoutes(GetViewModelAssemblies());
        }

        public virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            if (ViewAssemblies.Count == 0)
                ViewAssemblies.Add(GetType().GetTypeInfo().Assembly);

            return ViewAssemblies;
        }

        public virtual IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var app = Mvx.IoCProvider.Resolve<IMvxApplication>();
            var assembly = app.GetType().GetTypeInfo().Assembly;
            return new[] { assembly };
        }

        protected virtual IEnumerable<Assembly> GetBootstrapOwningAssemblies()
        {
            return GetViewAssemblies().Distinct();
        }

        protected abstract IMvxNameMapping CreateViewToViewModelNaming();

        protected virtual IMvxViewModelByNameLookup CreateViewModelByNameLookup(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxViewModelByNameLookup>();
        }

        protected virtual IMvxViewModelByNameRegistry CreateViewModelByNameRegistry(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxViewModelByNameRegistry>();
        }

        protected virtual IMvxNameMapping InitializeViewModelTypeFinder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            CreateViewModelByNameLookup(iocProvider);
            var viewModelByNameRegistry = CreateViewModelByNameRegistry(iocProvider);

            var viewModelAssemblies = GetViewModelAssemblies();
            foreach (var assembly in viewModelAssemblies)
            {
                viewModelByNameRegistry.AddAll(assembly);
            }

            var nameMappingStrategy = CreateViewToViewModelNaming();
            iocProvider.RegisterSingleton(nameMappingStrategy);
            return nameMappingStrategy;
        }

        protected virtual IDictionary<Type, Type> InitializeLookupDictionary(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var viewAssemblies = GetViewAssemblies();
            var builder = iocProvider.Resolve<IMvxTypeToTypeLookupBuilder>();
            return builder.Build(viewAssemblies);
        }

        protected virtual IMvxViewsContainer? InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup,
            IMvxIoCProvider iocProvider)
        {
            if (viewModelViewLookup == null)
                return null;

            ValidateArguments(iocProvider);

            var container = iocProvider.Resolve<IMvxViewsContainer>();
            container.AddAll(viewModelViewLookup);
            return container;
        }

        protected virtual void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            // always the very last thing to get initialized
            // base class implementation is empty by default
        }

        public IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        public IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }

        private void FireStateChange(MvxSetupState state)
        {
            StateChanged?.Invoke(this, new MvxSetupStateEventArgs(state));
        }

        protected static void ValidateArguments(IMvxIoCProvider iocProvider)
        {
            if (iocProvider == null)
                throw new ArgumentNullException(nameof(iocProvider));
        }
    }

    public abstract class MvxSetup<TApplication> : MvxSetup
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
