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
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.IoC;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Views.Attributes;

namespace Cirrious.MvvmCross.Platform
{
    public abstract class MvxSetup
        : IDisposable
    {
        #region some cleanup code - especially for test harness use

        ~MvxSetup()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                MvxSingleton.ClearAllSingletons();
            }
        }

        #endregion Some cleanup code - especially for test harness use

        protected bool UsePrefixConventions { get; set; }

        protected string BaseTypeKeyword { get; set; }

        protected MvxSetup()
        {
            UsePrefixConventions = true;
            BaseTypeKeyword = "Base";
        }


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
            MvxTrace.Trace("Setup: ViewsContainer start");
            InitializeViewsContainer();
            MvxTrace.Trace("Setup: ViewDispatcherProvider start");
            InitiaiseViewDispatcherProvider();
            MvxTrace.Trace("Setup: Views start");
            InitializeViews();
            MvxTrace.Trace("Setup: LastChance start");
            InitializeLastChance();
            MvxTrace.Trace("Setup: Secondary end");
            State = MvxSetupState.Initialized;
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
            var app = CreateApp();
            Mvx.RegisterSingleton<IMvxViewModelLocatorFinder>(app);
        }

        protected abstract MvxApplication CreateApp();

        protected virtual void InitializeViewsContainer()
        {
            var container = CreateViewsContainer();
            Mvx.RegisterSingleton<IMvxViewsContainer>(container);
        }

        protected virtual void InitiaiseViewDispatcherProvider()
        {
            var provider = CreateViewDispatcherProvider();
            Mvx.RegisterSingleton(provider);
            var wrapped = new MvxViewBasedMainThreadDispatcherProvider(provider);
            Mvx.RegisterSingleton<IMvxMainThreadDispatcherProvider>(wrapped);
        }

        protected abstract MvxViewsContainer CreateViewsContainer();

        protected virtual void InitializeViews()
        {
            var container = Mvx.Resolve<IMvxViewsContainer>();

            foreach (var pair in GetViewModelViewLookup())
            {
                Add(container, pair.Key, pair.Value);
            }
        }

        protected abstract IDictionary<Type, Type> GetViewModelViewLookup();
        protected abstract IMvxViewDispatcherProvider CreateViewDispatcherProvider();

        protected virtual IDictionary<Type, Type> GetViewModelViewLookup(Assembly assembly, Type expectedInterfaceType)
        {
            var views = from type in assembly.GetTypes()
                        let viewModelType = GetViewModelTypeMappingIfPresent(type, expectedInterfaceType)
                        where viewModelType != null
                        select new {type, viewModelType};

            try
            {
                return views.ToDictionary(x => x.viewModelType, x => x.type);
            }
            catch (ArgumentException exception)
            {
                var overSizedCounts = views.GroupBy(x => x.viewModelType)
                                           .Select(x => new {x.Key.Name, Count = x.Count()})
                                           .Where(x => x.Count > 1)
                                           .ToList();

                if (overSizedCounts.Count == 0)
                {
                    // no idea what the error is - so throw the original
                    throw;
                }
                else
                {
                    var overSizedText = string.Join(",", overSizedCounts);
                    throw exception.MvxWrap(
                        "Problem seen creating View-ViewModel lookup table - you have more than one View registered for the ViewModels: {0}",
                        overSizedText);
                }
            }
        }

        protected virtual Type GetViewModelTypeMappingIfPresent(Type candidateType, Type expectedInterfaceType)
        {
            if (candidateType == null)
                return null;

            if (candidateType.IsAbstract)
                return null;

            if (!expectedInterfaceType.IsAssignableFrom(candidateType))
                return null;

            if (TestTypeForBaseKeyword(candidateType))
                return null;

            var unconventionalAttributes = candidateType.GetCustomAttributes(typeof (MvxUnconventionalViewAttribute),
                                                                             true);
            if (unconventionalAttributes.Length > 0)
                return null;

            var conditionalAttributes =
                candidateType.GetCustomAttributes(typeof (MvxConditionalConventionalViewAttribute), true);
            foreach (MvxConditionalConventionalViewAttribute conditional in conditionalAttributes)
            {
                var result = conditional.IsConventional;
                if (!result)
                    return null;
            }

            var viewModelPropertyInfo = candidateType
                .GetProperties()
                .FirstOrDefault(x => x.Name == "ViewModel"
                                     && !x.PropertyType.IsInterface
                                     && !TestTypeForBaseKeyword(x.PropertyType));

            if (viewModelPropertyInfo == null)
                return null;

            return viewModelPropertyInfo.PropertyType;
        }

        private bool TestTypeForBaseKeyword(Type candidateType)
        {
            if (UsePrefixConventions)
            {
                if (candidateType.Name.StartsWith(BaseTypeKeyword))
                    return true;
            }
            else
            {
                if (candidateType.Name.EndsWith(BaseTypeKeyword))
                    return true;
            }
            return false;
        }

        protected virtual void InitializeLastChance()
        {
            // always the very last thing to get initialized
            // base class implementation is empty by default
        }

        protected void Add<TViewModel, TView>(IMvxViewsContainer container)
            where TViewModel : IMvxViewModel
        {
            container.Add(typeof (TViewModel), typeof (TView));
        }

        protected void Add(IMvxViewsContainer container, Type viewModelType, Type viewType)
        {
            container.Add(viewModelType, viewType);
        }

        #region Lifecycle

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