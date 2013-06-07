// MvxSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Platform
{
    public abstract class MvxSetup
    {
        protected abstract IMvxTrace CreateDebugTrace();

        protected abstract IMvxApplication CreateApp();

        protected abstract MvxViewsContainer CreateViewsContainer();

        protected abstract IMvxViewDispatcher CreateViewDispatcher();

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
            MvxTrace.Trace("Setup: FirstChance start");
            InitializeFirstChance();
            MvxTrace.Trace("Setup: DebugServices start");
            InitializeDebugServices();
            MvxTrace.Trace("Setup: PlatformServices start");
            InitializePlatformServices();
        }

        public virtual void InitializeSecondary()
        {
            MvxTrace.Trace("Setup: Bootstrap actions");
            PerformBootstrapActions();
            MvxTrace.Trace("Setup: Singleton Cache start");
            InitializeSingletonCache();
            MvxTrace.Trace("Setup: MvvmCross settings start");
            InitializeSettings();
            MvxTrace.Trace("Setup: StringToTypeParser start");
            InitializeStringToTypeParser();
            MvxTrace.Trace("Setup: ViewModelFramework start");
            InitializeViewModelFramework();
            MvxTrace.Trace("Setup: PluginManagerFramework start");
            var pluginManager = InitializePluginFramework();
            MvxTrace.Trace("Setup: App start");
            InitializeApp(pluginManager);
            MvxTrace.Trace("Setup: ViewModelTypeFinder start");
            InitialiseViewModelTypeFinder();
            MvxTrace.Trace("Setup: ViewsContainer start");
            InitializeViewsContainer();
            MvxTrace.Trace("Setup: ViewDispatcher start");
            InitiaiseViewDispatcher();
            MvxTrace.Trace("Setup: Views start");
            InitializeViewLookup();
            MvxTrace.Trace("Setup: CommandCollectionBuilder start");
            InitialiseCommandCollectionBuilder();
            MvxTrace.Trace("Setup: NavigationSerializer start");
            InitializeNavigationSerializer();
            MvxTrace.Trace("Setup: InpcInterception start");
            InitializeInpcInterception();
            MvxTrace.Trace("Setup: LastChance start");
            InitializeLastChance();
            MvxTrace.Trace("Setup: Secondary end");
            State = MvxSetupState.Initialized;
        }

        protected virtual void InitializeSingletonCache()
        {
            MvxSingletonCache.Initialise();
        }

        protected virtual void InitializeInpcInterception()
        {
            // by default no Inpc calls are intercepted
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

        protected virtual void InitialiseCommandCollectionBuilder()
        {
            Mvx.RegisterSingleton(() => CreateCommandCollectionBuilder());
        }

        protected virtual IMvxCommandCollectionBuilder CreateCommandCollectionBuilder()
        {
            return new MvxCommandCollectionBuilder();
        }

        protected virtual void InitializeIoC()
        {
            // initialise the IoC registry, then add it to itself
            var iocProvider = CreateIocProvider();
            Mvx.RegisterSingleton(iocProvider);
        }

        protected virtual IMvxIoCProvider CreateIocProvider()
        {
            return MvxSimpleIoCContainer.Initialise();
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
            var debugTrace = CreateDebugTrace();
            Mvx.RegisterSingleton<IMvxTrace>(debugTrace);
            MvxTrace.Initialize();
        }

        protected virtual void InitializeViewModelFramework()
        {
            Mvx.RegisterType<IMvxViewModelLoader, MvxViewModelLoader>();
        }

        protected virtual IMvxPluginManager InitializePluginFramework()
        {
            var pluginManager = CreatePluginManager();
            pluginManager.ConfigurationSource = GetPluginConfiguration;
            Mvx.RegisterSingleton(pluginManager);
            LoadPlugins(pluginManager);
            return pluginManager;
        }

        protected abstract IMvxPluginManager CreatePluginManager();

        protected virtual IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            return null;
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
        }

        /*
         * this code removed - as it just would't work on enough platforms :/
         * I blame Microsoft... not supporting GetReferencedAssembliesEx() on WP and WinRT
         * means that we can't really do nice automated plugin loading
        protected void TryAutoLoadPluginsByReflection()
        {
            var assemblies = GetPluginOwningAssemblies();
            var candidatePluginNames = assemblies.SelectMany(a => a.GetReferencedAssembliesEx()).Distinct();
            var filtered = candidatePluginNames
                .Where(a => a.Name.Contains("Plugin"))
                .Where(a => !a.Name.Contains("Droid"));

            var list = filtered.ToList();
            foreach (var assemblyName in list)
            {
                var pluginTypeName = string.Format("{0}.Plugin, {0}", assemblyName.Name);
                var type = Type.GetType(pluginTypeName);
                if (type == null)
                {
                    MvxTrace.Trace("Plugin not found - will not autoload {0}");
                    continue;
                }

                var field = type.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
                if (field == null)
                {
                    MvxTrace.Trace("Plugin Instance not found - will not autoload {0}");
                    continue;
                }

                var instance = field.GetValue(null);
                if (instance == null)
                {
                    MvxTrace.Trace("Plugin Instance was empty - will not autoload {0}");
                    continue;
                }
                var pluginLoader = instance as IMvxPluginLoader;
                if (pluginLoader == null)
                {
                    MvxTrace.Trace("Plugin Instance was not a loader - will not autoload {0}");
                    continue;
                }

                EnsurePluginLoaded(pluginLoader);
            }
        }
         */

        protected virtual void InitializeApp(IMvxPluginManager pluginManager)
        {
            var app = CreateAndInitializeApp(pluginManager);
            Mvx.RegisterSingleton(app);
            Mvx.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
        }

        protected virtual IMvxApplication CreateAndInitializeApp(IMvxPluginManager pluginManager)
        {
            var app = CreateApp();
            app.LoadPlugins(pluginManager);
            app.Initialize();
            return app;
        }

        protected virtual void InitializeViewsContainer()
        {
            var container = CreateViewsContainer();
            Mvx.RegisterSingleton<IMvxViewsContainer>(container);
        }

        protected virtual void InitiaiseViewDispatcher()
        {
            var dispatcher = CreateViewDispatcher();
            Mvx.RegisterSingleton(dispatcher);
            Mvx.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
        }

        protected virtual Assembly[] GetViewAssemblies()
        {
            var assembly = GetType().Assembly;
            return new[] {assembly};
        }

        protected virtual Assembly[] GetViewModelAssemblies()
        {
            var app = Mvx.Resolve<IMvxApplication>();
            var assembly = app.GetType().Assembly;
            return new[] {assembly};
        }

        protected virtual Assembly[] GetBootstrapOwningAssemblies()
        {
            var assemblies = new List<Assembly>();
            assemblies.AddRange(GetViewAssemblies());
            //ideally we would also add ViewModelAssemblies here too :/
            //assemblies.AddRange(GetViewModelAssemblies());
            return assemblies.Distinct().ToArray();
        }

        /*
        protected virtual Assembly[] GetPluginOwningAssemblies()
        {
            var assemblies = new List<Assembly>();
            assemblies.AddRange(GetViewAssemblies());
            //ideally we would also add ViewModelAssemblies here too :/
            //assemblies.AddRange(GetViewModelAssemblies());
            return assemblies.Distinct().ToArray();
        }
         */

        protected virtual void InitialiseViewModelTypeFinder()
        {
            var viewModelAssemblies = GetViewModelAssemblies();
            var viewModelByNameLookup = new MvxViewModelByNameLookup(viewModelAssemblies);
            Mvx.RegisterSingleton<IMvxViewModelByNameLookup>(viewModelByNameLookup);
            var finder = new MvxViewModelViewTypeFinder(viewModelByNameLookup);
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
            return CreatableTypes(this.GetType().Assembly);
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
            get { return _state; }
            private set
            {
                _state = value;
                FireStateChange(value);
            }
        }

        private void FireStateChange(MvxSetupState state)
        {
            var handler = StateChanged;
            if (handler != null)
            {
                handler(this, new MvxSetupStateEventArgs(state));
            }
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

        #endregion
    }
}