// MvxSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Logging.LogProviders;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Core.Platform
{
    public abstract class MvxSetup
    {
        protected abstract IMvxTrace CreateDebugTrace();

        protected abstract IMvxApplication CreateApp();

        protected abstract IMvxViewsContainer CreateViewsContainer();

        protected abstract IMvxViewDispatcher CreateViewDispatcher();

        protected IMvxLog SetupLog { get; private set; }

        public virtual void Initialize()
        {
            InitializePrimary();
            InitializeSecondary();
        }

        public virtual void InitializePrimary()
        {
            if (State != MvxSetupState.Uninitialized)
            {
                throw new MvxException("Cannot start primary - as state already {0}", State);
            }
            State = MvxSetupState.InitializingPrimary;
            MvxTrace.Trace("Setup: Primary start");
            InitializeIoC();
            State = MvxSetupState.InitializedPrimary;
            if (State != MvxSetupState.InitializedPrimary)
            {
                throw new MvxException("Cannot start seconday - as state is currently {0}", State);
            }
            State = MvxSetupState.InitializingSecondary;
            InitializeLoggingServices();
            MvxTrace.Trace("Setup: FirstChance start");
            InitializeFirstChance();
            MvxTrace.Trace("Setup: DebugServices start");
            InitializeDebugServices();
            MvxTrace.Trace("Setup: PlatformServices start");
            InitializePlatformServices();
            MvxTrace.Trace("Setup: MvvmCross settings start");
            InitializeSettings();
            MvxTrace.Trace("Setup: Singleton Cache start");
            InitializeSingletonCache();
        }

        public virtual void InitializeSecondary()
        {
            MvxTrace.Trace("Setup: Bootstrap actions");
            PerformBootstrapActions();
            MvxTrace.Trace("Setup: StringToTypeParser start");
            InitializeStringToTypeParser();
            MvxTrace.Trace("Setup: CommandHelper start");
            InitializeCommandHelper();
            MvxTrace.Trace("Setup: PluginManagerFramework start");
            var pluginManager = InitializePluginFramework();
            MvxTrace.Trace("Setup: Create App");
            var app = CreateApp();
            Mvx.RegisterSingleton(app);
            MvxTrace.Trace("Setup: NavigationService");
            InitializeNavigationService(app);
            MvxTrace.Trace("Setup: Load navigation routes");
            LoadNavigationServiceRoutes();
            MvxTrace.Trace("Setup: App start");
            InitializeApp(pluginManager, app);
            MvxTrace.Trace("Setup: ViewModelTypeFinder start");
            InitializeViewModelTypeFinder();
            MvxTrace.Trace("Setup: ViewsContainer start");
            InitializeViewsContainer();
            MvxTrace.Trace("Setup: ViewDispatcher start");
            InitializeViewDispatcher();
            MvxTrace.Trace("Setup: Views start");
            InitializeViewLookup();
            MvxTrace.Trace("Setup: CommandCollectionBuilder start");
            InitializeCommandCollectionBuilder();
            MvxTrace.Trace("Setup: NavigationSerializer start");
            InitializeNavigationSerializer();
            MvxTrace.Trace("Setup: InpcInterception start");
            InitializeInpcInterception();
            MvxTrace.Trace("Setup: InpcInterception start");
            InitializeViewModelCache();
            MvxTrace.Trace("Setup: LastChance start");
            InitializeLastChance();
            MvxTrace.Trace("Setup: Secondary end");
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
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions();
        }

        protected virtual IMvxIoCProvider CreateIocProvider()
        {
            return MvxSimpleIoCContainer.Initialize(CreateIocOptions());
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
                Mvx.RegisterSingleton(globalLog);
            }
        }

        protected virtual MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Console;
       
        protected virtual IMvxLogProvider CreateLogProvider()
        {
            switch(GetDefaultLogProviderType())
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

        [Obsolete("IMvxTrace is replaced by IMvxLogProvider and IMvxLog")]
        protected virtual void InitializeDebugServices()
        {
            var debugTrace = CreateDebugTrace();
            Mvx.RegisterSingleton(debugTrace);
            MvxTrace.Initialize();
        }

        protected virtual IMvxViewModelLoader CreateViewModelLoader(IMvxViewModelLocatorCollection collection)
        {
            return new MvxViewModelLoader(collection);
        }

        protected virtual IMvxPluginManager InitializePluginFramework()
        {
            var pluginManager = CreatePluginManager();
            AddPluginsLoaders (pluginManager.Registry);
            pluginManager.ConfigurationSource = GetPluginConfiguration;
            Mvx.RegisterSingleton(pluginManager);
            LoadPlugins(pluginManager);
            return pluginManager;
        }

        protected virtual void AddPluginsLoaders (MvxLoaderPluginRegistry registry)
        {
        }

        protected abstract IMvxPluginManager CreatePluginManager();

        protected virtual IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            return null;
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
        }

        protected virtual void InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            app.LoadPlugins(pluginManager);
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

        protected virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            return new[] { assembly };
        }

        protected virtual IEnumerable<Assembly> GetViewModelAssemblies()
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

        protected virtual void InitializeViewModelTypeFinder()
        {
            var viewModelByNameLookup = new MvxViewModelByNameLookup();

            var viewModelAssemblies = GetViewModelAssemblies();
            foreach (var assembly in viewModelAssemblies)
            {
                viewModelByNameLookup.AddAll(assembly);
            }

            Mvx.RegisterSingleton<IMvxViewModelByNameLookup>(viewModelByNameLookup);
            Mvx.RegisterSingleton<IMvxViewModelByNameRegistry>(viewModelByNameLookup);

            var nameMappingStrategy = CreateViewToViewModelNaming();
            var finder = new MvxViewModelViewTypeFinder(viewModelByNameLookup, nameMappingStrategy);
            Mvx.RegisterSingleton<IMvxViewModelTypeFinder>(finder);
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

        protected IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        protected IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }

        #region Setup state lifecycle

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

        public virtual void EnsureInitialized(Type requiredBy)
        {
            switch (State)
            {
                case MvxSetupState.Uninitialized:
                    Initialize();
                    break;

                case MvxSetupState.InitializingPrimary:
                case MvxSetupState.InitializedPrimary:
                case MvxSetupState.InitializingSecondary:
                    throw new MvxException("The default EnsureInitialized method does not handle partial initialization");
                case MvxSetupState.Initialized:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion Setup state lifecycle
    }
}
