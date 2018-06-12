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
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Core
{
    public abstract class MvxSetup : IMvxSetup
    {
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

        protected abstract IMvxApplication CreateApp();

        protected abstract IMvxViewsContainer CreateViewsContainer();

        protected abstract IMvxViewDispatcher CreateViewDispatcher();

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
            SetupLog.Trace("Setup: PlatformServices start");
            InitializePlatformServices();
            SetupLog.Trace("Setup: MvvmCross settings start");
            InitializeSettings();
            SetupLog.Trace("Setup: Singleton Cache start");
            InitializeSingletonCache();
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
            SetupLog.Trace("Setup: CommandHelper start");
            InitializeCommandHelper();
            SetupLog.Trace("Setup: PluginManagerFramework start");
            var pluginManager = InitializePluginFramework();
            SetupLog.Trace("Setup: Create App");
            var app = CreateApp();
            Mvx.RegisterSingleton(app);
            SetupLog.Trace("Setup: NavigationService");
            InitializeNavigationService(app);
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

        protected virtual void InitializeCommandHelper()
        {
            Mvx.RegisterType<IMvxCommandHelper, MvxWeakCommandHelper>();
        }

        protected virtual void InitializeSingletonCache()
        {
            MvxSingletonCache.Initialize();
        }

        protected virtual void InitializeInpcInterception()
        {
            // by default no Inpc calls are intercepted
        }

        protected virtual void InitializeViewModelCache()
        {
            Mvx.RegisterSingleton<IMvxChildViewModelCache>(new MvxChildViewModelCache());
        }

        protected virtual void InitializeSettings()
        {
            Mvx.RegisterSingleton<IMvxSettings>(CreateSettings());
        }

        protected virtual IMvxSettings CreateSettings()
        {
            return new MvxSettings();
        }

        protected virtual void InitializeStringToTypeParser()
        {
            var parser = CreateStringToTypeParser();
            Mvx.RegisterSingleton<IMvxStringToTypeParser>(parser);
            Mvx.RegisterSingleton<IMvxFillableStringToTypeParser>(parser);
        }

        protected virtual MvxStringToTypeParser CreateStringToTypeParser()
        {
            return new MvxStringToTypeParser();
        }

        protected virtual void PerformBootstrapActions()
        {
            var bootstrapRunner = new MvxBootstrapRunner();
            foreach (var assembly in GetBootstrapOwningAssemblies())
            {
                bootstrapRunner.Run(assembly);
            }
        }

        protected virtual void InitializeNavigationSerializer()
        {
            var serializer = CreateNavigationSerializer();
            Mvx.RegisterSingleton(serializer);
        }

        protected virtual IMvxNavigationSerializer CreateNavigationSerializer()
        {
            return new MvxStringDictionaryNavigationSerializer();
        }

        protected virtual void InitializeCommandCollectionBuilder()
        {
            Mvx.RegisterSingleton(CreateCommandCollectionBuilder);
        }

        protected virtual IMvxCommandCollectionBuilder CreateCommandCollectionBuilder()
        {
            return new MvxCommandCollectionBuilder();
        }

        protected virtual void InitializeIoC()
        {
            // initialize the IoC registry, then add it to itself
            var iocProvider = CreateIocProvider();
            Mvx.RegisterSingleton(iocProvider);
            Mvx.RegisterSingleton<IMvxSetup>(this);
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions();
        }

        protected virtual IMvxIoCProvider CreateIocProvider()
        {
            return MvxIoCProvider.Initialize(CreateIocOptions());
        }

        protected virtual void InitializeFirstChance()
        {
            // always the very first thing to get initialized - after IoC and base platfom
            // base class implementation is empty by default
        }

        protected virtual void InitializePlatformServices()
        {
            // do nothing by default
        }

        protected virtual void InitializeLoggingServices()
        {
            var logProvider = CreateLogProvider();
            if (logProvider != null)
            {
                Mvx.RegisterSingleton(logProvider);
                SetupLog = logProvider.GetLogFor<MvxSetup>();
                var globalLog = logProvider.GetLogFor<MvxLog>();
                MvxLog.Instance = globalLog;
                Mvx.RegisterSingleton(globalLog);
            }
        }

        public virtual MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Console;

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

        protected virtual IMvxViewModelLoader CreateViewModelLoader(IMvxViewModelLocatorCollection collection)
        {
            return new MvxViewModelLoader(collection);
        }

        protected virtual IMvxPluginManager InitializePluginFramework()
        {
            var pluginManager = CreatePluginManager();
            Mvx.RegisterSingleton(pluginManager);
            LoadPlugins(pluginManager);
            return pluginManager;
        }

        protected virtual IMvxPluginManager CreatePluginManager()
            => new MvxPluginManager(GetPluginConfiguration);

        protected virtual IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            return null;
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
            catch(Exception ex)
            {
                return true;
            }
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

        protected virtual void InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            app.LoadPlugins(pluginManager);
            SetupLog.Trace("Setup: Application Initialize - On background thread");
            app.Initialize();
            Mvx.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
        }

        protected virtual void InitializeViewsContainer()
        {
            var container = CreateViewsContainer();
            Mvx.RegisterSingleton<IMvxViewsContainer>(container);
        }

        protected virtual void InitializeViewDispatcher()
        {
            var dispatcher = CreateViewDispatcher();
            Mvx.RegisterSingleton(dispatcher);
            Mvx.RegisterSingleton<IMvxMainThreadAsyncDispatcher>(dispatcher);
            Mvx.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
        }

        protected virtual IMvxNavigationService InitializeNavigationService(IMvxViewModelLocatorCollection collection)
        {
            var loader = CreateViewModelLoader(collection);
            Mvx.RegisterSingleton<IMvxViewModelLoader>(loader);
            var navigationService = new MvxNavigationService(null, loader);
            Mvx.RegisterSingleton<IMvxNavigationService>(navigationService);
            return navigationService;
        }

        protected virtual void LoadNavigationServiceRoutes()
        {
            MvxNavigationService.LoadRoutes(GetViewModelAssemblies());
        }

        public virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            var assemblies = ViewAssemblies ?? new[] { GetType().GetTypeInfo().Assembly };
            return assemblies;
        }

        public virtual IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var app = Mvx.Resolve<IMvxApplication>();
            var assembly = app.GetType().GetTypeInfo().Assembly;
            return new[] { assembly };
        }

        protected virtual IEnumerable<Assembly> GetBootstrapOwningAssemblies()
        {
            var assemblies = new List<Assembly>();
            assemblies.AddRange(GetViewAssemblies());
            //ideally we would also add ViewModelAssemblies here too :/
            //assemblies.AddRange(GetViewModelAssemblies());
            return assemblies.Distinct().ToArray();
        }

        protected abstract IMvxNameMapping CreateViewToViewModelNaming();

        private MvxViewModelByNameLookup _viewModelNameLookup;
        private MvxViewModelByNameLookup ViewModelNameLookup => _viewModelNameLookup ?? (_viewModelNameLookup = new MvxViewModelByNameLookup());

        protected virtual IMvxViewModelByNameLookup CreateViewModelByNameLookup()
        {
            return ViewModelNameLookup;
        }
        protected virtual IMvxViewModelByNameRegistry CreateViewModelByNameRegistry()
        {
            return ViewModelNameLookup;
        }

        protected virtual void RegisterViewTypeFinder()
        {
            Mvx.LazyConstructAndRegisterSingleton<IMvxViewModelTypeFinder, MvxViewModelViewTypeFinder>();
        }

        protected virtual void InitializeViewModelTypeFinder()
        {
            var viewModelByNameLookup = CreateViewModelByNameLookup();
            Mvx.RegisterSingleton(viewModelByNameLookup);

            var viewModelByNameRegistry = CreateViewModelByNameRegistry();
            Mvx.RegisterSingleton(viewModelByNameRegistry);

            var viewModelAssemblies = GetViewModelAssemblies();
            foreach (var assembly in viewModelAssemblies)
            {
                viewModelByNameRegistry.AddAll(assembly);
            }

            var nameMappingStrategy = CreateViewToViewModelNaming();
            Mvx.RegisterSingleton(nameMappingStrategy);

            RegisterViewTypeFinder();
        }

        protected virtual void InitializeViewLookup()
        {
            var viewAssemblies = GetViewAssemblies();
            var builder = new MvxViewModelViewLookupBuilder();
            var viewModelViewLookup = builder.Build(viewAssemblies);
            if (viewModelViewLookup == null)
                return;

            var container = Mvx.Resolve<IMvxViewsContainer>();
            container.AddAll(viewModelViewLookup);
        }

        protected virtual void InitializeLastChance()
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
        protected override IMvxApplication CreateApp() => Mvx.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
