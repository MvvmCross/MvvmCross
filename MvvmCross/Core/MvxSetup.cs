// Licensed to the .NET Foundation under one or more agreements.
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
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Core
{
    public abstract class MvxSetup : IMvxSetup
    {
        private IMvxViewPresenter _presenter;

        protected static Func<IMvxSetup> SetupCreator { get; set; }

        protected static Assembly[] ViewAssemblies { get; set; }

        public static void RegisterSetupType<TMvxSetup>(params Assembly[] assemblies) where TMvxSetup : MvxSetup, new()
        {
            ViewAssemblies = assemblies;
            if (!(ViewAssemblies?.Any() ?? false))
            {
                // fall back to all assemblies. Assembly.GetEntryAssembly() always returns
                // null on Xamarin platforms do not use it!
                ViewAssemblies = AppDomain.CurrentDomain.GetAssemblies();
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

        protected virtual IMvxViewPresenter ViewPresenter
        {
            get
            {
                return _presenter;
            }
        }

        protected virtual void RegisterImplementations()
        {
            // Register Singletons that can be lazy loaded
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxSettings, MvxSettings>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxPluginManager, MvxPluginManager>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewModelLoader, MvxViewModelLoader>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxNavigationService, MvxNavigationService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxNavigationSerializer, MvxStringDictionaryNavigationSerializer>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxViewModelTypeFinder, MvxViewModelViewTypeFinder>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxTypeToTypeLookupBuilder, MvxViewModelViewLookupBuilder>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxCommandCollectionBuilder, MvxCommandCollectionBuilder>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxChildViewModelCache, MvxChildViewModelCache>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMvxStringToTypeParser, MvxStringToTypeParser>();

            RegisterViewDispatcher();
            RegisterApp();
            RegisterViewsContainer();
            RegisterViewPresenter();

            // Register non-singletons that will be constructed on demand
            //Mvx.RegisterType<IMvxCommandHelper, MvxWeakCommandHelper>();
        }

        protected abstract void RegisterViewDispatcher();

        protected abstract void RegisterApp();

        protected abstract void RegisterViewsContainer();

        protected abstract void RegisterViewPresenter();

        protected IMvxLog SetupLog { get; private set; }

        public virtual void InitializePrimary()
        {
            if (State != MvxSetupState.Uninitialized)
            {
                return;
            }
            State = MvxSetupState.InitializingPrimary;
            InitializeIoC();
            InitializeLoggingServices();
            SetupLog.Trace("Setup: Primary start");
            SetupLog.Trace("Setup: FirstChance start");
            InitializeFirstChance();
            SetupLog.Trace("Setup: Singleton Cache start");
            InitializeSingletonCache();
            SetupLog.Trace("Setup: Register Implementations start");
            RegisterImplementations();
            SetupLog.Trace("Setup: PlatformServices start");
            InitializePlatformServices();
            SetupLog.Trace("Setup: MvvmCross settings start");
            InitializeSettings();
            SetupLog.Trace("Setup: ViewPresenter start");
            InitializeViewPresenter();
            SetupLog.Trace("Setup: ViewDispatcher start");
            InitializeViewDispatcher();
            State = MvxSetupState.InitializedPrimary;
        }

        public virtual void InitializeSecondary()
        {
            if (State != MvxSetupState.InitializedPrimary)
            {
                return;
            }
            State = MvxSetupState.InitializingSecondary;
            SetupLog.Trace("Setup: Bootstrap actions");
            PerformBootstrapActions();
            SetupLog.Trace("Setup: StringToTypeParser start");
            InitializeStringToTypeParser();
            SetupLog.Trace("Setup: PluginManagerFramework start");
            var pluginManager = InitializePluginFramework();
            SetupLog.Trace("Setup: Create App");
            var app = CreateApp();
            SetupLog.Trace("Setup: ViewModelLoader");
            var loader = InitializeViewModelLoader(app);
            SetupLog.Trace("Setup: NavigationService");
            InitializeNavigationService(loader);
            SetupLog.Trace("Setup: Load navigation routes");
            LoadNavigationServiceRoutes();
            SetupLog.Trace("Setup: App start");
            InitializeApp(pluginManager, app);
            SetupLog.Trace("Setup: ViewModelTypeFinder start");
            InitializeViewModelTypeFinder();
            SetupLog.Trace("Setup: ViewsContainer start");
            InitializeViewsContainer();
            SetupLog.Trace("Setup: Views start");
            InitializeViewLookup();
            SetupLog.Trace("Setup: CommandCollectionBuilder start");
            InitializeCommandCollectionBuilder();
            SetupLog.Trace("Setup: NavigationSerializer start");
            InitializeNavigationSerializer();
            SetupLog.Trace("Setup: InpcInterception start");
            InitializeInpcInterception();
            SetupLog.Trace("Setup: InpcInterception start");
            InitializeViewModelCache();
            SetupLog.Trace("Setup: LastChance start");
            InitializeLastChance();
            SetupLog.Trace("Setup: Secondary end");
            State = MvxSetupState.Initialized;
        }

        protected virtual IMvxIoCProvider InitializeIoC()
        {
            // initialize the IoC registry, then add it to itself
            var iocProvider = CreateIocProvider();
            Mvx.IoCProvider.RegisterSingleton(iocProvider);
            Mvx.IoCProvider.RegisterSingleton<IMvxSetup>(this);
            return iocProvider;
        }

        protected virtual IMvxIoCProvider CreateIocProvider()
        {
            return MvxIoCProvider.Initialize(CreateIocOptions());
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions();
        }

        protected virtual IMvxLogProvider InitializeLoggingServices()
        {
            var logProvider = CreateLogProvider();
            if (logProvider != null)
            {
                Mvx.IoCProvider.RegisterSingleton(logProvider);
                SetupLog = logProvider.GetLogFor<MvxSetup>();
                var globalLog = logProvider.GetLogFor<MvxLog>();
                MvxLog.Instance = globalLog;
                Mvx.IoCProvider.RegisterSingleton(globalLog);
            }

            return logProvider;
        }

        protected virtual IMvxLogProvider CreateLogProvider()
        {
            switch (GetDefaultLogProviderType())
            {
                case MvxLogProviderType.Console:
                    return new ConsoleLogProvider();
                case MvxLogProviderType.EntLib:
                    return new EntLibLogProvider();
                case MvxLogProviderType.Log4Net:
                    return new Log4NetLogProvider();
                case MvxLogProviderType.Loupe:
                    return new LoupeLogProvider();
                case MvxLogProviderType.NLog:
                    return new NLogLogProvider();
                case MvxLogProviderType.Serilog:
                    return new SerilogLogProvider();
                default:
                    return null;
            }
        }

        public virtual MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Console;

        protected virtual void InitializeFirstChance()
        {
            // always the very first thing to get initialized - after IoC, logging service and base platfom
            // base class implementation is empty by default
        }

        protected virtual void InitializeSingletonCache()
        {
            MvxSingletonCache.Initialize();
        }

        protected virtual void InitializePlatformServices()
        {
            // do nothing by default
        }

        protected virtual IMvxSettings InitializeSettings()
        {
            return Mvx.IoCProvider.Resolve<IMvxSettings>();
        }

        protected virtual IMvxViewPresenter InitializeViewPresenter()
        {
            _presenter = Mvx.IoCProvider.Resolve<IMvxViewPresenter>();
            return _presenter;
        }

        protected virtual IMvxViewDispatcher InitializeViewDispatcher()
        {
            var dispatcher = Mvx.IoCProvider.Resolve<IMvxViewDispatcher>();
            dispatcher.Presenter = ViewPresenter;
            Mvx.IoCProvider.RegisterSingleton<IMvxMainThreadAsyncDispatcher>(dispatcher);
            Mvx.IoCProvider.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
            return dispatcher;
        }

        protected virtual void PerformBootstrapActions()
        {
            var bootstrapRunner = new MvxBootstrapRunner();
            foreach (var assembly in GetBootstrapOwningAssemblies())
            {
                bootstrapRunner.Run(assembly);
            }
        }

        protected virtual IEnumerable<Assembly> GetBootstrapOwningAssemblies()
        {
            var assemblies = new List<Assembly>();
            assemblies.AddRange(GetViewAssemblies());
            //ideally we would also add ViewModelAssemblies here too :/
            //assemblies.AddRange(GetViewModelAssemblies());
            return assemblies.Distinct().ToArray();
        }

        public virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            var assemblies = ViewAssemblies ?? new[] { GetType().GetTypeInfo().Assembly };
            return assemblies;
        }

        protected virtual IMvxStringToTypeParser InitializeStringToTypeParser()
        {
            var parser = Mvx.IoCProvider.Resolve<IMvxStringToTypeParser>();
            Mvx.IoCProvider.RegisterSingleton((IMvxFillableStringToTypeParser)parser);
            return parser;
        }

        protected virtual IMvxPluginManager InitializePluginFramework()
        {
            var pluginManager = Mvx.IoCProvider.Resolve<IMvxPluginManager>();
            pluginManager.ConfigurationSource = GetPluginConfiguration;
            LoadPlugins(pluginManager);
            return pluginManager;
        }

        protected virtual IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            return null;
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
            var pluginAttribute = typeof(MvxPluginAttribute);

            var pluginTypes =
                GetPluginAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(TypeContainsPluginAttribute);

            foreach (var pluginType in pluginTypes)
            {
                pluginManager.EnsurePluginLoaded(pluginType);
            }

            bool TypeContainsPluginAttribute(Type type)
                => (type.GetCustomAttributes(pluginAttribute, false)?.Length ?? 0) > 0;
        }

        public virtual IEnumerable<Assembly> GetPluginAssemblies()
        {
            var mvvmCrossAssemblyName = typeof(MvxPluginAttribute).Assembly.GetName().Name;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var pluginAssemblies =
                assemblies
                    .AsParallel()
                    .Where(asmb => AssemblyReferencesMvvmCross(asmb, mvvmCrossAssemblyName));

            return pluginAssemblies;
        }

        private bool AssemblyReferencesMvvmCross(Assembly assembly, string mvvmCrossAssemblyName)
        {
            try
            {
                return assembly.GetReferencedAssemblies().Any(a => a.Name == mvvmCrossAssemblyName);
            }
            catch (Exception)
            {
                // TODO: Should the response here be true or false? Surely if exception we should return false?
                return true;
            }
        }

        protected virtual IMvxApplication CreateApp()
        {
            return Mvx.IoCProvider.Resolve<IMvxApplication>();
        }

        protected virtual IMvxViewModelLoader InitializeViewModelLoader(IMvxViewModelLocatorCollection collection)
        {
            var loader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            loader.LocatorCollection = collection;
            return loader;
        }

        protected virtual IMvxNavigationService InitializeNavigationService(IMvxViewModelLoader loader)
        {
            var navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            navigationService.ViewModelLoader = loader;
            return navigationService;
        }

        protected virtual void LoadNavigationServiceRoutes()
        {
            MvxNavigationService.LoadRoutes(GetViewModelAssemblies());
        }

        public virtual IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var app = Mvx.IoCProvider.Resolve<IMvxApplication>();
            var assembly = app.GetType().GetTypeInfo().Assembly;
            return new[] { assembly };
        }

        protected virtual void InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            app.LoadPlugins(pluginManager);
            SetupLog.Trace("Setup: Application Initialize - On background thread");
            app.Initialize();
            Mvx.IoCProvider.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
        }

        protected virtual void InitializeViewModelTypeFinder()
        {
            var viewModelByNameLookup = CreateViewModelByNameLookup();
            Mvx.IoCProvider.RegisterSingleton(viewModelByNameLookup);

            var viewModelByNameRegistry = CreateViewModelByNameRegistry();
            Mvx.IoCProvider.RegisterSingleton(viewModelByNameRegistry);

            var viewModelAssemblies = GetViewModelAssemblies();
            foreach (var assembly in viewModelAssemblies)
            {
                viewModelByNameRegistry.AddAll(assembly);
            }

            var nameMappingStrategy = CreateViewToViewModelNaming();
            Mvx.IoCProvider.RegisterSingleton(nameMappingStrategy);
        }

        protected abstract IMvxNameMapping CreateViewToViewModelNaming();

        private MvxViewModelByNameLookup _viewModelNameLookup;
        protected virtual MvxViewModelByNameLookup ViewModelNameLookup => _viewModelNameLookup ?? (_viewModelNameLookup = new MvxViewModelByNameLookup());

        protected virtual IMvxViewModelByNameLookup CreateViewModelByNameLookup()
        {
            return ViewModelNameLookup;
        }
        protected virtual IMvxViewModelByNameRegistry CreateViewModelByNameRegistry()
        {
            return ViewModelNameLookup;
        }

        protected virtual IMvxViewsContainer InitializeViewsContainer()
        {
            return Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
        }

        protected virtual void InitializeViewLookup()
        {
            var viewAssemblies = GetViewAssemblies();
            var builder = Mvx.IoCProvider.Resolve<IMvxTypeToTypeLookupBuilder>();
            var viewModelViewLookup = builder.Build(viewAssemblies);
            if (viewModelViewLookup == null)
                return;

            var container = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
            container.AddAll(viewModelViewLookup);
        }

        protected virtual IMvxCommandCollectionBuilder InitializeCommandCollectionBuilder()
        {
            return Mvx.IoCProvider.Resolve<IMvxCommandCollectionBuilder>();
        }

        protected virtual IMvxNavigationSerializer InitializeNavigationSerializer()
        {
            return Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
        }

        protected virtual void InitializeInpcInterception()
        {
            // by default no Inpc calls are intercepted
        }

        protected virtual IMvxChildViewModelCache InitializeViewModelCache()
        {
            return Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
        }

        protected virtual void InitializeLastChance()
        {
            // always the very last thing to get initialized
            // base class implementation is empty by default
        }

        //protected virtual void InitializeCommandHelper()
        //{
        //    Mvx.IoCProvider.RegisterType<IMvxCommandHelper, MvxWeakCommandHelper>();
        //}

        //protected virtual MvxStringToTypeParser CreateStringToTypeParser()
        //{
        //    return new MvxStringToTypeParser();
        //}

        public IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        public IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }

        public enum MvxSetupState
        {
            Uninitialized,
            InitializingPrimary,
            InitializedPrimary,
            InitializingSecondary,
            Initialized
        }

        public class MvxSetupStateEventArgs : EventArgs
        {
            public MvxSetupStateEventArgs(MvxSetupState setupState)
            {
                SetupState = setupState;
            }

            public MvxSetupState SetupState { get; private set; }
        }

        public event EventHandler<MvxSetupStateEventArgs> StateChanged;

        private MvxSetupState _state;
        public MvxSetupState State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
                FireStateChange(value);
            }
        }

        private void FireStateChange(MvxSetupState state)
        {
            StateChanged?.Invoke(this, new MvxSetupStateEventArgs(state));
        }
    }

    public abstract class MvxSetup<TApplication> : MvxSetup
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
