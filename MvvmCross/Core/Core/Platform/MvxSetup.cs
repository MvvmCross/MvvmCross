// MvxSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Platform
{
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
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.Plugins;

    public abstract class MvxSetup
    {
        protected abstract IMvxTrace CreateDebugTrace();

        protected abstract IMvxApplication CreateApp();

        protected abstract IMvxViewsContainer CreateViewsContainer();

        protected abstract IMvxViewDispatcher CreateViewDispatcher();

        public virtual void Initialize()
        {
            this.InitializePrimary();
            this.InitializeSecondary();
        }

        public virtual void InitializePrimary()
        {
            if (this.State != MvxSetupState.Uninitialized)
            {
                throw new MvxException("Cannot start primary - as state already {0}", this.State);
            }
            this.State = MvxSetupState.InitializingPrimary;
            MvxTrace.Trace("Setup: Primary start");
            this.InitializeIoC();
            this.State = MvxSetupState.InitializedPrimary;
            if (this.State != MvxSetupState.InitializedPrimary)
            {
                throw new MvxException("Cannot start seconday - as state is currently {0}", this.State);
            }
            this.State = MvxSetupState.InitializingSecondary;
            MvxTrace.Trace("Setup: FirstChance start");
            this.InitializeFirstChance();
            MvxTrace.Trace("Setup: DebugServices start");
            this.InitializeDebugServices();
            MvxTrace.Trace("Setup: PlatformServices start");
            this.InitializePlatformServices();
            MvxTrace.Trace("Setup: MvvmCross settings start");
            this.InitializeSettings();
            MvxTrace.Trace("Setup: Singleton Cache start");
            this.InitializeSingletonCache();
        }

        public virtual void InitializeSecondary()
        {
            MvxTrace.Trace("Setup: Bootstrap actions");
            this.PerformBootstrapActions();
            MvxTrace.Trace("Setup: StringToTypeParser start");
            this.InitializeStringToTypeParser();
            MvxTrace.Trace("Setup: CommandHelper start");
            this.InitializeCommandHelper();
            MvxTrace.Trace("Setup: ViewModelFramework start");
            this.InitializeViewModelFramework();
            MvxTrace.Trace("Setup: PluginManagerFramework start");
            var pluginManager = this.InitializePluginFramework();
            MvxTrace.Trace("Setup: App start");
            this.InitializeApp(pluginManager);
            MvxTrace.Trace("Setup: ViewModelTypeFinder start");
            this.InitializeViewModelTypeFinder();
            MvxTrace.Trace("Setup: ViewsContainer start");
            this.InitializeViewsContainer();
            MvxTrace.Trace("Setup: ViewDispatcher start");
            this.InitializeViewDispatcher();
            MvxTrace.Trace("Setup: Views start");
            this.InitializeViewLookup();
            MvxTrace.Trace("Setup: CommandCollectionBuilder start");
            this.InitializeCommandCollectionBuilder();
            MvxTrace.Trace("Setup: NavigationSerializer start");
            this.InitializeNavigationSerializer();
            MvxTrace.Trace("Setup: InpcInterception start");
            this.InitializeInpcInterception();
            MvxTrace.Trace("Setup: LastChance start");
            this.InitializeLastChance();
            MvxTrace.Trace("Setup: Secondary end");
            this.State = MvxSetupState.Initialized;
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

        protected virtual void InitializeSettings()
        {
            Mvx.RegisterSingleton<IMvxSettings>(this.CreateSettings());
        }

        protected virtual IMvxSettings CreateSettings()
        {
            return new MvxSettings();
        }

        protected virtual void InitializeStringToTypeParser()
        {
            var parser = this.CreateStringToTypeParser();
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
            foreach (var assembly in this.GetBootstrapOwningAssemblies())
            {
                bootstrapRunner.Run(assembly);
            }
        }

        protected virtual void InitializeNavigationSerializer()
        {
            var serializer = this.CreateNavigationSerializer();
            Mvx.RegisterSingleton(serializer);
        }

        protected virtual IMvxNavigationSerializer CreateNavigationSerializer()
        {
            return new MvxStringDictionaryNavigationSerializer();
        }

        protected virtual void InitializeCommandCollectionBuilder()
        {
            Mvx.RegisterSingleton(this.CreateCommandCollectionBuilder);
        }

        protected virtual IMvxCommandCollectionBuilder CreateCommandCollectionBuilder()
        {
            return new MvxCommandCollectionBuilder();
        }

        protected virtual void InitializeIoC()
        {
            // initialize the IoC registry, then add it to itself
            var iocProvider = this.CreateIocProvider();
            Mvx.RegisterSingleton(iocProvider);
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions();
        }

        protected virtual IMvxIoCProvider CreateIocProvider()
        {
            return MvxSimpleIoCContainer.Initialize(this.CreateIocOptions());
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

        protected virtual void InitializeDebugServices()
        {
            var debugTrace = this.CreateDebugTrace();
            Mvx.RegisterSingleton<IMvxTrace>(debugTrace);
            MvxTrace.Initialize();
        }

        protected virtual void InitializeViewModelFramework()
        {
            Mvx.RegisterSingleton<IMvxViewModelLoader>(this.CreateViewModelLoader());
        }

        protected virtual IMvxViewModelLoader CreateViewModelLoader()
        {
            return new MvxViewModelLoader();
        }

        protected virtual IMvxPluginManager InitializePluginFramework()
        {
            var pluginManager = this.CreatePluginManager();
            AddPluginsLoaders (pluginManager.Registry);
            pluginManager.ConfigurationSource = this.GetPluginConfiguration;
            Mvx.RegisterSingleton(pluginManager);
            this.LoadPlugins(pluginManager);
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

        protected virtual void InitializeApp(IMvxPluginManager pluginManager)
        {
            var app = this.CreateAndInitializeApp(pluginManager);
            Mvx.RegisterSingleton(app);
            Mvx.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
        }

        protected virtual IMvxApplication CreateAndInitializeApp(IMvxPluginManager pluginManager)
        {
            var app = this.CreateApp();
            app.LoadPlugins(pluginManager);
            app.Initialize();
            return app;
        }

        protected virtual void InitializeViewsContainer()
        {
            var container = this.CreateViewsContainer();
            Mvx.RegisterSingleton<IMvxViewsContainer>(container);
        }

        protected virtual void InitializeViewDispatcher()
        {
            var dispatcher = this.CreateViewDispatcher();
            Mvx.RegisterSingleton(dispatcher);
            Mvx.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
            InitializeNavigationService(dispatcher);
        }

        protected virtual IMvxNavigationService InitializeNavigationService(IMvxViewDispatcher dispatcher)
        {
            var navigationService = new MvxNavigationService(dispatcher);
            Mvx.RegisterSingleton<IMvxNavigationService>(navigationService);
            MvxNavigationService.LoadRoutes(GetViewModelAssemblies());
            return navigationService;
        }

        protected virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
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
            assemblies.AddRange(this.GetViewAssemblies());
            //ideally we would also add ViewModelAssemblies here too :/
            //assemblies.AddRange(GetViewModelAssemblies());
            return assemblies.Distinct().ToArray();
        }

        protected abstract IMvxNameMapping CreateViewToViewModelNaming();

        protected virtual void InitializeViewModelTypeFinder()
        {
            var viewModelByNameLookup = new MvxViewModelByNameLookup();

            var viewModelAssemblies = this.GetViewModelAssemblies();
            foreach (var assembly in viewModelAssemblies)
            {
                viewModelByNameLookup.AddAll(assembly);
            }

            Mvx.RegisterSingleton<IMvxViewModelByNameLookup>(viewModelByNameLookup);
            Mvx.RegisterSingleton<IMvxViewModelByNameRegistry>(viewModelByNameLookup);

            var nameMappingStrategy = this.CreateViewToViewModelNaming();
            var finder = new MvxViewModelViewTypeFinder(viewModelByNameLookup, nameMappingStrategy);
            Mvx.RegisterSingleton<IMvxViewModelTypeFinder>(finder);
        }

        protected virtual void InitializeViewLookup()
        {
            var viewAssemblies = this.GetViewAssemblies();
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
            return this.CreatableTypes(this.GetType().GetTypeInfo().Assembly);
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
                this.SetupState = setupState;
            }

            public MvxSetupState SetupState { get; private set; }
        }

        public event EventHandler<MvxSetupStateEventArgs> StateChanged;

        private MvxSetupState _state;

        public MvxSetupState State
        {
            get { return this._state; }
            private set
            {
                this._state = value;
                this.FireStateChange(value);
            }
        }

        private void FireStateChange(MvxSetupState state)
        {
            StateChanged?.Invoke(this, new MvxSetupStateEventArgs(state));
        }

        public virtual void EnsureInitialized(Type requiredBy)
        {
            switch (this.State)
            {
                case MvxSetupState.Uninitialized:
                    this.Initialize();
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