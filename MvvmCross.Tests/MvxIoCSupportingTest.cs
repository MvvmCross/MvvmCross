// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Core;
using MvvmCross.Hosting;

namespace MvvmCross.Tests
{
    /// <summary>
    /// Base class for unit tests that need a configured MvvmCross DI container.
    /// Replaces the old <c>MvxIoCProvider</c>-based setup with Microsoft.Extensions.DependencyInjection.
    /// </summary>
    public class MvxIoCSupportingTest
    {
        private ServiceCollection _serviceCollection = new();
        private IServiceProvider? _serviceProvider;

        /// <summary>
        /// The built service provider. Built lazily on first access after <see cref="ClearAll"/> or <see cref="Setup"/>.
        /// </summary>
        public IServiceProvider Services
        {
            get
            {
                if (_serviceProvider == null)
                    BuildServices();
                return _serviceProvider!;
            }
        }

        /// <summary>
        /// The mutable service collection. Add registrations here before any service is resolved.
        /// </summary>
        public IServiceCollection ServiceCollection => _serviceCollection;

        /// <summary>
        /// Compatibility shim — allows test code written against the old <c>IMvxIoCProvider</c> API
        /// to continue working by translating registrations into the underlying <see cref="ServiceCollection"/>.
        /// </summary>
        public MvxTestServiceLocator Ioc { get; private set; } = null!;

        public void Setup()
        {
            ClearAll();
        }

        public void Reset()
        {
            (_serviceProvider as IDisposable)?.Dispose();
            _serviceProvider = null;
            _serviceCollection = new ServiceCollection();
            Ioc = new MvxTestServiceLocator(this);
            MvxHost.ResetForTesting();
        }

        public virtual void ClearAll()
        {
            Reset();
            InitializeMvxSettings();
            AdditionalSetup();
        }

        /// <summary>
        /// Explicitly builds the <see cref="Services"/> provider and sets <see cref="MvxHost.Current"/>.
        /// Called automatically on first access to <see cref="Services"/> if not called manually.
        /// </summary>
        public void BuildServices()
        {
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            MvxHost.InitializeForTesting(_serviceProvider);
        }

        protected virtual void InitializeMvxSettings()
        {
            _serviceCollection.TryAddSingleton<IMvxSettings, MvxSettings>();
        }

        protected virtual void AdditionalSetup()
        {
        }

        public void SetInvariantCulture()
        {
            var invariantCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;
        }
    }

    /// <summary>
    /// Compatibility shim that translates old <c>IMvxIoCProvider</c>-style registration calls
    /// into <see cref="IServiceCollection"/> registrations.
    /// This allows existing tests to migrate incrementally.
    /// </summary>
    public sealed class MvxTestServiceLocator
    {
        private readonly MvxIoCSupportingTest _host;

        internal MvxTestServiceLocator(MvxIoCSupportingTest host)
        {
            _host = host;
        }

        /// <summary>Registers a singleton instance.</summary>
        public void RegisterSingleton<TInterface>(TInterface instance)
            where TInterface : class
        {
            _host.ServiceCollection.AddSingleton(instance);
        }

        /// <summary>Registers a singleton factory.</summary>
        public void RegisterSingleton<TInterface>(Func<TInterface> factory)
            where TInterface : class
        {
            _host.ServiceCollection.AddSingleton<TInterface>(_ => factory());
        }

        /// <summary>Registers a transient type mapping.</summary>
        public void RegisterType<TInterface, TImpl>()
            where TInterface : class
            where TImpl : class, TInterface
        {
            _host.ServiceCollection.AddTransient<TInterface, TImpl>();
        }

        /// <summary>Resolves a service from the built container.</summary>
        public TInterface Resolve<TInterface>()
            where TInterface : class
        {
            return _host.Services.GetRequiredService<TInterface>();
        }

        /// <summary>Tries to resolve a service. Returns false and sets <paramref name="service"/> to null if not found.</summary>
        public bool TryResolve<TInterface>(out TInterface? service)
            where TInterface : class
        {
            service = _host.Services.GetService<TInterface>();
            return service != null;
        }
    }
}
