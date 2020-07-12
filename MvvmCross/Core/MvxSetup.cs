// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        protected static Action<IMvxIoCProvider> RegisterSetupDependencies { get; set; }

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

        protected IMvxLog? SetupLog { get; private set; }

        public virtual async Task InitializePrimary()
        {
            if (State != MvxSetupState.Uninitialized)
            {
                return;
            }

            State = MvxSetupState.InitializingPrimary;

            var iocProvider = InitializeIoC();

            // Register the default setup dependencies before
            // invoking the static call back.
            // Developers can either extend the MvxSetup and override
            // the RegisterDefaultSetupDependencies method, or can provide a
            // callback method by setting the RegisterSetupDependencies method
            RegisterDefaultSetupDependencies(iocProvider);
            RegisterSetupDependencies?.Invoke(iocProvider);

            InitializeLoggingServices();

            await Task.WhenAll(new[]
                {
                InitializeFirstChance(),
                InitializeSettings(),
                InitializeSingletonCache(),
                InitializeViewDispatcher(),
                InitializeApp(),
                InitializeViewModelTypeFinder()
            }).ConfigureAwait(false);

            State = MvxSetupState.InitializedPrimary;
        }

        public virtual async Task InitializeSecondary()
        {
            if (State != MvxSetupState.InitializedPrimary)
            {
                return;
            }
            State = MvxSetupState.InitializingSecondary;

            await Task.WhenAll(new[]
            {
                PerformBootstrapActions(),
                InitializeStringToTypeParser(),
                InitializeFillableStringToTypeParser(),
                InitializeNavigationService(),
                InitializeViewsContainer(),
                InitializeViewLookup(),
                InitializeCommandCollectionBuilder(),
                InitializeNavigationSerializer(),
                InitializeInpcInterception(),
                InitializeViewModelCache(),
                InitializeLastChance()
            }).ConfigureAwait(false);

            SetupLog?.Trace("Setup: Secondary end");
            State = MvxSetupState.Initialized;
        }

        protected virtual Task InitializeSingletonCache()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: Singleton Cache start");
                MvxSingletonCache.Initialize();
            });
        }

        protected virtual Task InitializeInpcInterception()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: InpcInterception start");
                // by default no Inpc calls are intercepted
            });
        }

        protected virtual Task InitializeViewModelCache()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: InpcInterception start");
                _ = CreateViewModelCache();
            });
        }

        protected virtual IMvxChildViewModelCache CreateViewModelCache()
        {
            return Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
        }

        protected virtual Task InitializeSettings()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: MvvmCross settings start");
                _ = CreateSettings();
            });
        }

        protected virtual IMvxSettings CreateSettings()
        {
            return Mvx.IoCProvider.Resolve<IMvxSettings>();
        }

        protected virtual Task<IMvxStringToTypeParser> InitializeStringToTypeParser()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: StringToTypeParser start");

                var parser = CreateStringToTypeParser();
                return parser;
            });
        }

        protected virtual IMvxStringToTypeParser CreateStringToTypeParser()
        {
            return Mvx.IoCProvider.Resolve<IMvxStringToTypeParser>();
        }

        protected virtual Task<IMvxFillableStringToTypeParser> InitializeFillableStringToTypeParser()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: FillableStringToTypeParser start");
                var parser = CreateFillableStringToTypeParser();
                Mvx.IoCProvider.RegisterSingleton(parser);
                return parser;
            });
        }

        protected virtual IMvxFillableStringToTypeParser CreateFillableStringToTypeParser()
        {
            return Mvx.IoCProvider.Resolve<IMvxStringToTypeParser>() as IMvxFillableStringToTypeParser;
        }

        protected virtual Task PerformBootstrapActions()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: Bootstrap actions");

                var bootstrapRunner = new MvxBootstrapRunner();
                foreach (var assembly in GetBootstrapOwningAssemblies())
                {
                    bootstrapRunner.Run(assembly);
                }
            });
        }

        protected virtual Task InitializeNavigationSerializer()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: NavigationSerializer start");

                _ = CreateNavigationSerializer();
            });
        }

        protected virtual IMvxNavigationSerializer CreateNavigationSerializer()
        {
            return Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
        }

        protected virtual Task InitializeCommandCollectionBuilder()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: CommandCollectionBuilder start");
                _ = CreateCommandCollectionBuilder();
            });
        }

        protected virtual IMvxCommandCollectionBuilder CreateCommandCollectionBuilder()
        {
            return Mvx.IoCProvider.Resolve<IMvxCommandCollectionBuilder>();
        }

        protected virtual IMvxIoCProvider InitializeIoC()
        {
            // initialize the IoC registry, then add it to itself
            var iocProvider = CreateIocProvider();
            Mvx.IoCProvider.RegisterSingleton(iocProvider);
            Mvx.IoCProvider.RegisterSingleton<IMvxSetup>(this);
            return iocProvider;
        }

        protected virtual void RegisterDefaultSetupDependencies(IMvxIoCProvider iocProvider)
        {
            RegisterLogProvider(iocProvider);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxSettings, MvxSettings>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxStringToTypeParser, MvxStringToTypeParser>();
            iocProvider.RegisterSingleton<IMvxPluginManager>(() => new MvxPluginManager(GetPluginConfiguration));
            iocProvider.RegisterSingleton(CreateApp);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelLoader, MvxViewModelLoader>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxNavigationService, IMvxViewModelLoader>(loader =>
                new MvxNavigationService(null, loader));
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

        protected virtual Task InitializeFirstChance()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: FirstChance start");
                // always the very first thing to get initialized - after IoC and base platform
                // base class implementation is empty by default
            });
        }

        protected virtual void InitializeLoggingServices()
        {
            //return Task.Run(() =>
            //{
            var logProvider = CreateLogProvider();
            SetupLog = logProvider.GetLogFor<MvxSetup>();
            var globalLog = logProvider.GetLogFor<MvxLog>();
            MvxLog.Instance = globalLog;
            Mvx.IoCProvider.RegisterSingleton(globalLog);
            //});
        }

        public virtual MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Console;

        protected virtual void RegisterLogProvider(IMvxIoCProvider iocProvider)
        {
            Func<IMvxLogProvider>? logProviderCreator;
            switch (GetDefaultLogProviderType())
            {
                case MvxLogProviderType.Console:
                    logProviderCreator = () => new ConsoleLogProvider();
                    break;

                case MvxLogProviderType.EntLib:
                    logProviderCreator = () => new EntLibLogProvider();
                    break;

                case MvxLogProviderType.Log4Net:
                    logProviderCreator = () => new Log4NetLogProvider();
                    break;

                case MvxLogProviderType.Loupe:
                    logProviderCreator = () => new LoupeLogProvider();
                    break;

                case MvxLogProviderType.NLog:
                    logProviderCreator = () => new NLogLogProvider();
                    break;

                case MvxLogProviderType.Serilog:
                    logProviderCreator = () => new SerilogLogProvider();
                    break;

                default:
                    logProviderCreator = null;
                    break;
            }

            if (logProviderCreator != null)
            {
                iocProvider?.RegisterSingleton(logProviderCreator);
            }
        }

        protected virtual IMvxLogProvider CreateLogProvider()
        {
            return Mvx.IoCProvider.Resolve<IMvxLogProvider>();
        }

        protected virtual IMvxViewModelLoader CreateViewModelLoader()
        {
            return Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
        }

        protected virtual IMvxNavigationService CreateNavigationService()
        {
            return Mvx.IoCProvider.Resolve<IMvxNavigationService>();
        }

        protected virtual Task<IMvxPluginManager> InitializePluginFramework()
        {
            return Task.Run(() =>
            {
                var pluginManager = CreatePluginManager();
                LoadPlugins(pluginManager);
                return pluginManager;
            });
        }

        protected virtual IMvxPluginManager CreatePluginManager()
        {
            return Mvx.IoCProvider.Resolve<IMvxPluginManager>();
        }

        protected virtual IMvxPluginConfiguration? GetPluginConfiguration(Type plugin)
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
            catch (Exception)
            {
                // TODO: Should the response here be true or false? Surely if exception we should return false?
                return true;
            }
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
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

            bool TypeContainsPluginAttribute(Type type) => (type.GetCustomAttributes(pluginAttribute, false)?.Length ?? 0) > 0;
        }

        protected virtual IMvxApplication CreateMvxApplication()
        {
            return Mvx.IoCProvider.Resolve<IMvxApplication>();
        }

        protected virtual Task<IMvxApplication> InitializeMvxApplication()
        {
            return Task.Run(() =>
            {
                var app = CreateMvxApplication();
                Mvx.IoCProvider.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
                return app;
            });
        }

        private async Task InitializeApp()
        {
            SetupLog?.Trace("Setup: PluginManagerFramework start");
            var pluginManager = await InitializePluginFramework().ConfigureAwait(false);

            SetupLog?.Trace("Setup: Create App");
            var app = await InitializeMvxApplication().ConfigureAwait(false);

            SetupLog?.Trace("Setup: App start");
            await InitializeApp(pluginManager, app).ConfigureAwait(false);
        }

        protected virtual Task InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            return Task.Run(() =>
            {
                app.LoadPlugins(pluginManager);
                SetupLog?.Trace("Setup: Application Initialize - On background thread");
                app.Initialize();
            });
        }

        protected virtual Task InitializeViewsContainer()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: ViewsContainer start");

                var container = CreateViewsContainer();
                Mvx.IoCProvider.RegisterSingleton(container);
            });
        }

        protected virtual Task InitializeViewDispatcher()
        {
            return Task.Run(() =>
            {
                var dispatcher = CreateViewDispatcher();

                SetupLog?.Trace("Setup: ViewDispatcher start");
                Mvx.IoCProvider.RegisterSingleton(dispatcher);
                Mvx.IoCProvider.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
                Mvx.IoCProvider.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
            });
        }

        protected virtual Task<IMvxNavigationService> InitializeNavigationService()
        {
            SetupLog?.Trace("Setup: NavigationService");
            var loader = CreateViewModelLoader();

            return Task.Run(() =>
            {
                var navigationService = CreateNavigationService();
                SetupLog?.Trace("Setup: Load navigation routes");
                LoadNavigationServiceRoutes(navigationService);
                return navigationService;
            });
        }

        protected virtual void LoadNavigationServiceRoutes(IMvxNavigationService navigationService)
        {
            navigationService.LoadRoutes(GetViewModelAssemblies());
        }

        public virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            var assemblies = ViewAssemblies ?? new[] { GetType().GetTypeInfo().Assembly };
            return assemblies;
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

        protected virtual IMvxViewModelByNameLookup CreateViewModelByNameLookup()
        {
            return Mvx.IoCProvider.Resolve<IMvxViewModelByNameLookup>();
        }

        protected virtual IMvxViewModelByNameRegistry CreateViewModelByNameRegistry()
        {
            return Mvx.IoCProvider.Resolve<IMvxViewModelByNameRegistry>();
        }

        protected virtual Task InitializeViewModelTypeFinder()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: ViewModelTypeFinder start");

                var viewModelByNameLookup = CreateViewModelByNameLookup();
                var viewModelByNameRegistry = CreateViewModelByNameRegistry();

                var viewModelAssemblies = GetViewModelAssemblies();
                foreach (var assembly in viewModelAssemblies)
                {
                    viewModelByNameRegistry.AddAll(assembly);
                }

                var nameMappingStrategy = CreateViewToViewModelNaming();
                Mvx.IoCProvider.RegisterSingleton(nameMappingStrategy);
            });
        }

        private async Task InitializeViewLookup()
        {
            SetupLog?.Trace("Setup: Lookup Dictionary start");
            var lookup = await InitializeLookupDictionary().ConfigureAwait(false);

            SetupLog?.Trace("Setup: Views start");
            await InitializeViewLookup(lookup).ConfigureAwait(false);
        }

        protected virtual Task<IDictionary<Type, Type>> InitializeLookupDictionary()
        {
            return Task.Run(() =>
            {
                var viewAssemblies = GetViewAssemblies();
                var builder = Mvx.IoCProvider.Resolve<IMvxTypeToTypeLookupBuilder>();
                var viewModelViewLookup = builder.Build(viewAssemblies);
                return viewModelViewLookup;
            });
        }

        protected virtual Task<IMvxViewsContainer?> InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup)
        {
            return Task.Run(() =>
            {
                if (viewModelViewLookup == null)
                    return null;

                var container = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
                container.AddAll(viewModelViewLookup);
                return container;
            });
        }

        protected virtual Task InitializeLastChance()
        {
            return Task.Run(() =>
            {
                SetupLog?.Trace("Setup: LastChance start");
                // always the very last thing to get initialized
                // base class implementation is empty by default
            });
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

            public MvxSetupState SetupState { get; }
        }

        public event EventHandler<MvxSetupStateEventArgs>? StateChanged;

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
        protected override IMvxApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
