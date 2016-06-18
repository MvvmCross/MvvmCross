using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Core
{
	public abstract class MvxAsyncSetup
	{
		protected abstract IMvxTrace CreateDebugTrace();

		protected abstract IMvxApplication CreateApp();

		protected abstract IMvxViewsContainer CreateViewsContainer();

		protected abstract IMvxViewDispatcher CreateViewDispatcher();

		public virtual async Task InitializeAsync()
		{
			await InitializePrimaryAsync().ConfigureAwait(false);
			await InitializeSecondaryAsync().ConfigureAwait(false);
		}

		public virtual async Task InitializePrimaryAsync()
		{
			if (State != MvxSetupState.Uninitialized)
				throw new MvxException($"Cannot start primary - as state already {State}");

			State = MvxSetupState.InitializingPrimary;
			MvxTrace.Trace("Setup: Primary start");
			InitializeIoC();
			State = MvxSetupState.InitializedPrimary;
			if (State != MvxSetupState.InitializedPrimary)
				throw new MvxException($"Cannot start seconday - as state is currently {State}");

			State = MvxSetupState.InitializingSecondary;

			InitializeFirstChance();

			var tasks = new[] {
				Task.Run(() => InitializeDebugServices()),
			    Task.Run(() => InitializePlatformServices()),
			    Task.Run(() => InitializeSettings()),
			    Task.Run(() => InitializeSingletonCache()),
			};

			await Task.WhenAll(tasks).ConfigureAwait(false);
		}

		public virtual async Task InitializeSecondaryAsync()
		{
			var tasks = new[] {
				PerformBootstrapActionsAsync(),
				Task.Run(() => InitializeStringToTypeParser()),
				Task.Run(() => InitializeCommandHelper()),
				Task.Run(() => InitializeViewModelFramework()),
				Task.Run(() => {
					var pluginManager = InitializePluginFramework();
					InitializeApp(pluginManager);
				}),
			    Task.Run(() => InitializeViewModelTypeFinder()),
			    Task.Run(() => InitializeViewsContainer()),
			    Task.Run(() => InitializeViewDispatcher()),
			    Task.Run(() => InitializeViewLookup()),
			    Task.Run(() => InitializeCommandCollectionBuilder()),
			    Task.Run(() => InitializeNavigationSerializer()),
			    Task.Run(() => InitializeInpcInterception()),
			};

			await Task.WhenAll(tasks).ConfigureAwait(false);

			InitializeLastChance();
			MvxTrace.Trace("Setup: Secondary end");
			State = MvxSetupState.Initialized;
		}

		protected virtual void InitializeCommandHelper()
		{
			MvxTrace.Trace("Setup: CommandHelper start");

			Mvx.RegisterType<IMvxCommandHelper, MvxWeakCommandHelper>();
		}

		protected virtual void InitializeSingletonCache()
		{
			MvxTrace.Trace("Setup: Singleton Cache start");

			MvxSingletonCache.Initialize();
		}

		protected virtual void InitializeInpcInterception()
		{
			MvxTrace.Trace("Setup: InpcInterception start");

			// by default no Inpc calls are intercepted
		}

		protected virtual void InitializeSettings()
		{
			MvxTrace.Trace("Setup: MvvmCross settings start");

			Mvx.RegisterSingleton<IMvxSettings>(CreateSettings());
		}

		protected virtual IMvxSettings CreateSettings()
		{
			return new MvxSettings();
		}

		protected virtual void InitializeStringToTypeParser()
		{
			MvxTrace.Trace("Setup: StringToTypeParser start");

			var parser = CreateStringToTypeParser();
			Mvx.RegisterSingleton<IMvxStringToTypeParser>(parser);
			Mvx.RegisterSingleton<IMvxFillableStringToTypeParser>(parser);
		}

		protected virtual MvxStringToTypeParser CreateStringToTypeParser()
		{
			return new MvxStringToTypeParser();
		}

		protected virtual Task PerformBootstrapActionsAsync()
		{
			MvxTrace.Trace("Setup: Bootstrap actions");

			var bootstrapRunner = new MvxBootstrapRunner();

			var tasks = GetBootstrapOwningAssemblies()
				.Select(ass => bootstrapRunner.RunAsync(ass));

			return Task.WhenAll(tasks);
		}

		protected virtual void InitializeNavigationSerializer()
		{
			MvxTrace.Trace("Setup: NavigationSerializer start");

			var serializer = CreateNavigationSerializer();
			Mvx.RegisterSingleton(serializer);
		}

		protected virtual IMvxNavigationSerializer CreateNavigationSerializer()
		{
			return new MvxStringDictionaryNavigationSerializer();
		}

		protected virtual void InitializeCommandCollectionBuilder()
		{
			MvxTrace.Trace("Setup: CommandCollectionBuilder start");

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
			MvxTrace.Trace("Setup: FirstChance start");

			// always the very first thing to get initialized - after IoC and base platfom
			// base class implementation is empty by default
		}

		protected virtual void InitializePlatformServices()
		{
			MvxTrace.Trace("Setup: PlatformServices start");

			// do nothing by default
		}

		protected virtual void InitializeDebugServices()
		{
			MvxTrace.Trace("Setup: DebugServices start");

			var debugTrace = CreateDebugTrace();
			Mvx.RegisterSingleton<IMvxTrace>(debugTrace);
			MvxTrace.Initialize();
		}

		protected virtual void InitializeViewModelFramework()
		{
			MvxTrace.Trace("Setup: ViewModelFramework start");

			Mvx.RegisterSingleton<IMvxViewModelLoader>(CreateViewModelLoader());
		}

		protected virtual IMvxViewModelLoader CreateViewModelLoader()
		{
			return new MvxViewModelLoader();
		}

		protected virtual IMvxPluginManager InitializePluginFramework()
		{
			MvxTrace.Trace("Setup: PluginManagerFramework start");

			var pluginManager = CreatePluginManager();
			AddPluginsLoaders(pluginManager.Registry);
			pluginManager.ConfigurationSource = GetPluginConfiguration;
			Mvx.RegisterSingleton(pluginManager);
			LoadPlugins(pluginManager);
			return pluginManager;
		}

		protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry registry)
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
			MvxTrace.Trace("Setup: App start");

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
			MvxTrace.Trace("Setup: ViewsContainer start");

			var container = CreateViewsContainer();
			Mvx.RegisterSingleton<IMvxViewsContainer>(container);
		}

		protected virtual void InitializeViewDispatcher()
		{
			MvxTrace.Trace("Setup: ViewDispatcher start");

			var dispatcher = CreateViewDispatcher();
			Mvx.RegisterSingleton(dispatcher);
			Mvx.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
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
			return assemblies.Distinct();
		}

		protected abstract IMvxNameMapping CreateViewToViewModelNaming();

		protected virtual void InitializeViewModelTypeFinder()
		{
			MvxTrace.Trace("Setup: ViewModelTypeFinder start");

			var viewModelByNameLookup = new MvxViewModelByNameLookup();

			var viewModelAssemblies = GetViewModelAssemblies();
			foreach (var assembly in viewModelAssemblies)
				viewModelByNameLookup.AddAll(assembly);

			Mvx.RegisterSingleton<IMvxViewModelByNameLookup>(viewModelByNameLookup);
			Mvx.RegisterSingleton<IMvxViewModelByNameRegistry>(viewModelByNameLookup);

			var nameMappingStrategy = CreateViewToViewModelNaming();
			var finder = new MvxViewModelViewTypeFinder(viewModelByNameLookup, nameMappingStrategy);
			Mvx.RegisterSingleton<IMvxViewModelTypeFinder>(finder);
		}

		protected virtual void InitializeViewLookup()
		{
			MvxTrace.Trace("Setup: Views start");

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
			MvxTrace.Trace("Setup: LastChance start");

			// always the very last thing to get initialized
			// base class implementation is empty by default
		}

		protected IEnumerable<Type> CreatableTypes()
		{
			return CreatableTypes(GetType().GetTypeInfo().Assembly);
		}

		protected static IEnumerable<Type> CreatableTypes(Assembly assembly)
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
			StateChanged?.Invoke(this, new MvxSetupStateEventArgs(state));
		}

		public virtual async Task EnsureInitializedAsync(Type requiredBy)
		{
			switch (State)
			{
				case MvxSetupState.Uninitialized:
					await InitializeAsync().ConfigureAwait(false);
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

