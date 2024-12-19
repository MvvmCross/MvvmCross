// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;
using MvvmCross.ViewModels.Result;
using MvvmCross.Views;

namespace MvvmCross.Core;

public abstract class MvxSetup : IMvxSetup
{
    public event EventHandler<MvxSetupStateEventArgs>? StateChanged;

    private static readonly object Lock = new();
    private MvxSetupState _state;
    private IMvxIoCProvider? _iocProvider;

    protected static Action<IMvxIoCProvider>? RegisterSetupDependencies { get; set; }

    protected static Func<IMvxSetup>? SetupCreator { get; set; }

    protected static List<Assembly> ViewAssemblies { get; } = [];

    protected ILogger? SetupLog { get; private set; }

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
        // We are using double-checked locking here to avoid overhead of locking if the
        // SetupCreator is already created
        if (SetupCreator is null)
        {
            lock (Lock)
            {
                if (SetupCreator is null)
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

                    return;
                }
            }
        }

        MvxLogHost.Default?.LogInformation("Setup: RegisterSetupType already called");
    }

    public static IMvxSetup? Instance()
    {
        var instance = SetupCreator?.Invoke() ?? MvxSetupExtensions.CreateSetup<MvxSetup>();
        return instance;
    }

    protected abstract IMvxApplication CreateApp(IMvxIoCProvider iocProvider);

    protected abstract IMvxViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider);

    protected abstract IMvxViewDispatcher CreateViewDispatcher();

    public virtual void InitializePrimary()
    {
        if (State != MvxSetupState.Uninitialized)
        {
            SetupLog?.Log(LogLevel.Trace,
                "InitializePrimary() called when State is not Uninitialized. State: {State}", State);
            return;
        }

        try
        {
            State = MvxSetupState.InitializingPrimary;
            _iocProvider = InitializeIoC();

            InitializeLoggingServices(_iocProvider);

            // Register the default setup dependencies before
            // invoking the static call back.
            // Developers can either extend the MvxSetup and override
            // the RegisterDefaultSetupDependencies method, or can provide a
            // callback method by setting the RegisterSetupDependencies method
            RegisterDefaultSetupDependencies(_iocProvider);
            RegisterSetupDependencies?.Invoke(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: Primary start");
            SetupLog?.Log(LogLevel.Trace, "Setup: FirstChance start");
            InitializeFirstChance(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: MvvmCross settings start");
            InitializeSettings(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: Singleton Cache start");
            InitializeSingletonCache();
            SetupLog?.Log(LogLevel.Trace, "Setup: ViewDispatcher start");
            InitializeViewDispatcher(_iocProvider);
            State = MvxSetupState.InitializedPrimary;
        }
        catch (Exception e)
        {
            SetupLog?.Log(LogLevel.Error, e, "InitializePrimary() Failed initializing primary dependencies");
            throw;
        }
    }

    public virtual void InitializeSecondary()
    {
        if (State != MvxSetupState.InitializedPrimary)
        {
            SetupLog?.Log(LogLevel.Trace,
                "InitializeSecondary() called when State is not InitializedPrimary. State: {State}", State);
            return;
        }

        if (_iocProvider == null)
        {
            SetupLog?.Log(LogLevel.Error, "InitializeSecondary() IoC Provider is null");
            throw new InvalidOperationException("Cannot continue setup with null IoCProvider");
        }

        try
        {
            State = MvxSetupState.InitializingSecondary;
            SetupLog?.Log(LogLevel.Trace, "Setup: Bootstrap actions");
            PerformBootstrapActions();
            SetupLog?.Log(LogLevel.Trace, "Setup: StringToTypeParser start");
            InitializeStringToTypeParser(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: FillableStringToTypeParser start");
            InitializeFillableStringToTypeParser(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: Create App");
            var app = InitializeMvxApplication(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: NavigationService");
            InitializeNavigationService(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: ResultViewModelManager");
            InitializeResultViewModelManager(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: ViewModelTypeFinder start");
            InitializeViewModelTypeFinder(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: ViewsContainer start");
            InitializeViewsContainer(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: Lookup Dictionary start");
            var lookup = InitializeLookupDictionary(_iocProvider);
            if (lookup != null)
            {
                SetupLog?.Log(LogLevel.Trace, "Setup: Views start");
                InitializeViewLookup(lookup, _iocProvider);
            }
            else
            {
                SetupLog?.LogWarning("Lookup dictionary is null returning from {MethodName}",
                    nameof(InitializeLookupDictionary));
            }

            SetupLog?.Log(LogLevel.Trace, "Setup: CommandCollectionBuilder start");
            InitializeCommandCollectionBuilder(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: NavigationSerializer start");
            InitializeNavigationSerializer(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
            InitializeInpcInterception(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
            InitializeViewModelCache(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: BindingBuilder start");
            InitializeBindingBuilder(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: PluginManagerFramework start");
            var pluginManager = InitializePluginFramework(_iocProvider);
            if (pluginManager != null)
            {
                app?.LoadPlugins(pluginManager);
                SetupLog?.Log(LogLevel.Trace, "Setup: App start");
            }
            else
            {
                SetupLog?.LogWarning("PluginManager was null returning from {MethodName}",
                    nameof(InitializePluginFramework));
            }

            if (app != null)
            {
                InitializeApp(app);
            }
            else
            {
                SetupLog?.LogWarning("App instance is null returning from {MethodName}",
                    nameof(InitializeMvxApplication));
            }

            SetupLog?.Log(LogLevel.Trace, "Setup: LastChance start");
            InitializeLastChance(_iocProvider);
            SetupLog?.Log(LogLevel.Trace, "Setup: Secondary end");
            State = MvxSetupState.Initialized;
        }
        catch (Exception e)
        {
            SetupLog?.Log(LogLevel.Error, e, "InitializeSecondary() failed initializing secondary dependencies");
            throw;
        }
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

    protected virtual IMvxChildViewModelCache? InitializeViewModelCache(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var cache = CreateViewModelCache(iocProvider);
        return cache;
    }

    protected virtual IMvxChildViewModelCache? CreateViewModelCache(IMvxIoCProvider iocProvider)
    {
        return iocProvider.Resolve<IMvxChildViewModelCache>();
    }

    protected virtual IMvxSettings? InitializeSettings(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var settings = CreateSettings(iocProvider);
        return settings;
    }

    protected virtual IMvxSettings? CreateSettings(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxSettings>();
    }

    protected virtual IMvxStringToTypeParser? InitializeStringToTypeParser(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return CreateStringToTypeParser(iocProvider);
    }

    protected virtual IMvxStringToTypeParser? CreateStringToTypeParser(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxStringToTypeParser>();
    }

    protected virtual IMvxFillableStringToTypeParser? InitializeFillableStringToTypeParser(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var parser = CreateFillableStringToTypeParser(iocProvider);
        if (parser != null)
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

    protected virtual IMvxNavigationSerializer? InitializeNavigationSerializer(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return CreateNavigationSerializer(iocProvider);
    }

    protected virtual IMvxNavigationSerializer? CreateNavigationSerializer(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxNavigationSerializer>();
    }

    protected virtual IMvxCommandCollectionBuilder? InitializeCommandCollectionBuilder(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return CreateCommandCollectionBuilder(iocProvider);
    }

    protected virtual IMvxCommandCollectionBuilder? CreateCommandCollectionBuilder(IMvxIoCProvider iocProvider)
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

        iocProvider.LazyConstructAndRegisterSingleton<IMvxSettings, MvxSettings>();
        iocProvider.LazyConstructAndRegisterSingleton<IMvxStringToTypeParser, MvxStringToTypeParser>();
        iocProvider.RegisterSingleton<IMvxPluginManager>(() => new MvxPluginManager(iocProvider, GetPluginConfiguration));
        iocProvider.RegisterSingleton(CreateApp(iocProvider));
        iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelLoader, MvxViewModelLoader>();
        iocProvider.LazyConstructAndRegisterSingleton<IMvxNavigationService, IMvxViewModelLoader, IMvxViewDispatcher, IMvxIoCProvider>(
            (loader, dispatcher, p) => new MvxNavigationService(loader, dispatcher, p));
        iocProvider.LazyConstructAndRegisterSingleton<IMvxResultViewModelManager, MvxResultViewModelManager>();
        iocProvider.RegisterSingleton(() => new MvxViewModelByNameLookup());
        iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelByNameLookup, MvxViewModelByNameLookup>(
            nameLookup => nameLookup);
        iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelByNameRegistry, MvxViewModelByNameLookup>(
            nameLookup => nameLookup);
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

        var logProvider = CreateLogProvider();
        var loggerFactory = CreateLogFactory();

        if (logProvider != null)
        {
            iocProvider.RegisterSingleton(logProvider);
            loggerFactory?.AddProvider(logProvider);
        }

        if (loggerFactory != null)
        {
            iocProvider.RegisterSingleton(loggerFactory);
            iocProvider.RegisterType(typeof(ILogger<>), typeof(Logger<>));
            SetupLog = loggerFactory.CreateLogger<MvxSetup>();
        }
    }

    protected abstract ILoggerProvider? CreateLogProvider();
    protected abstract ILoggerFactory? CreateLogFactory();

    protected virtual IMvxViewModelLoader? CreateViewModelLoader(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxViewModelLoader>();
    }

    protected virtual IMvxNavigationService? CreateNavigationService(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxNavigationService>();
    }

    protected virtual IMvxPluginManager? InitializePluginFramework(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var pluginManager = CreatePluginManager(iocProvider);
        if (pluginManager != null)
            LoadPlugins(pluginManager);
        return pluginManager;
    }

    protected virtual IMvxPluginManager? CreatePluginManager(IMvxIoCProvider iocProvider)
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
            .Where(assembly => AssemblyReferencesMvvmCross(assembly, mvvmCrossAssemblyName));
    }

    private static bool AssemblyReferencesMvvmCross(Assembly assembly, string? mvvmCrossAssemblyName)
    {
        if (string.IsNullOrEmpty(mvvmCrossAssemblyName))
            return false;

        try
        {
            return Array.Exists(assembly.GetReferencedAssemblies(), a => a.Name == mvvmCrossAssemblyName);
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

        // Search Assemblies for Plugins
        foreach (var pluginAssembly in pluginAssemblies)
        {
            var assemblyTypes = pluginAssembly.ExceptionSafeGetTypes();

            // Search Types for Valid Plugin
            foreach (var type in assemblyTypes.Where(TypeContainsPluginAttribute))
            {
                // Ensure Plugin has been loaded
                pluginManager.EnsurePluginLoaded(type);
            }
        }

        bool TypeContainsPluginAttribute(Type type) =>
            type.GetCustomAttributes(pluginAttribute, false).Length > 0;
    }

    protected virtual IMvxApplication? CreateMvxApplication(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxApplication>();
    }

    protected virtual IMvxApplication? InitializeMvxApplication(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var app = CreateMvxApplication(iocProvider);
        if (app != null)
            iocProvider.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
        return app;
    }

    protected virtual void InitializeApp(IMvxApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        SetupLog?.Log(LogLevel.Trace, "Setup: Application Initialize - On background thread");
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

    protected virtual IMvxNavigationService? InitializeNavigationService(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        CreateViewModelLoader(iocProvider);
        var navigationService = CreateNavigationService(iocProvider);
        if (navigationService != null)
        {
            SetupLog?.Log(LogLevel.Trace, "Setup: Load navigation routes");
            LoadNavigationServiceRoutes(navigationService, iocProvider);
        }
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
        var app = _iocProvider?.Resolve<IMvxApplication>();
        if (app == null) return [];
        var assembly = app.GetType().GetTypeInfo().Assembly;
        return [assembly];
    }

    protected virtual IEnumerable<Assembly> GetBootstrapOwningAssemblies()
    {
        return GetViewAssemblies().Distinct();
    }

    protected virtual IMvxResultViewModelManager? InitializeResultViewModelManager(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return CreateResultViewModelManager(iocProvider);
    }

    protected virtual IMvxResultViewModelManager? CreateResultViewModelManager(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxResultViewModelManager>();
    }

    protected abstract IMvxNameMapping CreateViewToViewModelNaming();

    protected virtual IMvxViewModelByNameLookup? CreateViewModelByNameLookup(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxViewModelByNameLookup>();
    }

    protected virtual IMvxViewModelByNameRegistry? CreateViewModelByNameRegistry(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        return iocProvider.Resolve<IMvxViewModelByNameRegistry>();
    }

    protected virtual IMvxNameMapping InitializeViewModelTypeFinder(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        CreateViewModelByNameLookup(iocProvider);
        var viewModelByNameRegistry = CreateViewModelByNameRegistry(iocProvider);
        if (viewModelByNameRegistry != null)
        {
            var viewModelAssemblies = GetViewModelAssemblies();
            foreach (var assembly in viewModelAssemblies)
            {
                viewModelByNameRegistry.AddAll(assembly);
            }
        }

        var nameMappingStrategy = CreateViewToViewModelNaming();
        iocProvider.RegisterSingleton(nameMappingStrategy);
        return nameMappingStrategy;
    }

    protected virtual IDictionary<Type, Type>? InitializeLookupDictionary(IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var viewAssemblies = GetViewAssemblies();
        var builder = iocProvider.Resolve<IMvxTypeToTypeLookupBuilder>();
        return builder?.Build(viewAssemblies);
    }

    protected virtual IMvxViewsContainer? InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup,
        IMvxIoCProvider iocProvider)
    {
        ValidateArguments(iocProvider);

        var container = iocProvider.Resolve<IMvxViewsContainer>();
        container?.AddAll(viewModelViewLookup);
        return container;
    }

    protected virtual void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
    {
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
        ArgumentNullException.ThrowIfNull(iocProvider);
    }
}
