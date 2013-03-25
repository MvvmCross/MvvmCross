// MvxSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Platform
{
    public abstract class MvxSetup
    {
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
            MvxTrace.Trace("Setup: ViewModelFramework start");
            InitializeViewModelFramework();
            MvxTrace.Trace("Setup: PluginManagerFramework start");
            InitializePluginFramework();
            MvxTrace.Trace("Setup: App start");
            InitializeApp();
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
            MvxTrace.Trace("Setup: LastChance start");
            InitializeLastChance();
            MvxTrace.Trace("Setup: Secondary end");
            State = MvxSetupState.Initialized;
        }

        protected virtual void InitialiseCommandCollectionBuilder()
        {
            Mvx.RegisterSingleton<IMvxCommandCollectionBuilder>(() => CreateCommandCollectionBuilder());
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
            MvxTrace.Initialize();
        }

        protected virtual void InitializeViewModelFramework()
        {
            Mvx.RegisterType<IMvxViewModelLoader, MvxViewModelLoader>();
        }

        protected virtual void InitializePluginFramework()
        {
            Mvx.RegisterSingleton(CreatePluginManager());
        }

        protected abstract IMvxPluginManager CreatePluginManager();

        protected virtual void InitializeApp()
        {
            var app = CreateAndInitializeApp();
            Mvx.RegisterSingleton<IMvxApplication>(app);
            Mvx.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
        }

        protected virtual IMvxApplication CreateAndInitializeApp()
        {
            var app = CreateApp();
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

        protected virtual void InitialiseViewModelTypeFinder()
        {
            var viewModelAssemblies = GetViewModelAssemblies();
            var viewModelByNameLookup = new MvxViewModelByNameLookup(viewModelAssemblies);
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