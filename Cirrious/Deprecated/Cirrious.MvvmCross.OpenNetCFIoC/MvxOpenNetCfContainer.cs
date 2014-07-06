// MvxOpenNetCfContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region Credit - OpenNetCf

// This file is based on the OpenNetCf IoC container - used under free license -see http://ioc.codeplex.com
// Note that this is not the standard OpenNetCf IoC container - we've removed their instance caching

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;

#endregion

namespace Cirrious.MvvmCross.OpenNetCfIoC
{
    public sealed class MvxOpenNetCfContainer
        : MvxSingleton<MvxOpenNetCfContainer>
    {
        private readonly Dictionary<string, object> _items = new Dictionary<string, object>();
        private readonly object _syncRoot = new object();
        private readonly Dictionary<Type, Type> _toResolve = new Dictionary<Type, Type>();

        /// <summary>
        ///   Gets the current instance of the IOC container.
        /// </summary>
        /// <value>The current.</value>
        public static MvxOpenNetCfContainer Current
        {
            get { return Instance ?? new MvxOpenNetCfContainer(); }
        }

        /// <summary>
        ///   Registers the type.
        /// </summary>
        /// <typeparam name = "TFrom">The type of from.</typeparam>
        /// <typeparam name = "TTo">The type of to.</typeparam>
        public void RegisterServiceType<TFrom, TTo>()
        {
            if (_toResolve.ContainsKey(typeof (TFrom)))
                throw new MvxException("Type already register");

            Type type = typeof (TTo);

            _toResolve.Add(typeof (TFrom), type);

            //// Adrian Sudbury extended code 31st Jan 2013

            //// Here we are looking for constructors decorated with the attribute MvxOpenNetCfInjectionAttribute.

            IEnumerable<ConstructorInfo> constructors = (type.GetConstructors(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                             .Where(
                                                                 c =>
                                                                 c.IsPublic &&
                                                                 c.GetCustomAttributes(
                                                                     typeof (MvxOpenNetCfInjectionAttribute), true)
                                                                  .Any()));

            if (constructors.Any())
            {
                //// If we find decorated constructor build type now.
                //// This was the only way i could get it to work - 
                //// i couldnt see how to do deffered object construction.
                MvxOpenNetCfObjectBuilder.CreateObject(type);
            }

            //// End of extended code.
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="typeFrom">The type from.</param>
        /// <param name="typeTo">The type to.</param>
        public void RegisterServiceType(Type typeFrom, Type typeTo)
        {
            if (_toResolve.ContainsKey(typeFrom))
                throw new MvxException("Type already register");
            _toResolve.Add(typeFrom, typeTo);
        }

        public bool CanResolve<TToResolve>() where TToResolve : class
        {
            var typeToBuild = typeof (TToResolve);
            if (_items.ContainsKey(typeToBuild.FullName))
                return true;
            return CanCreateInstance(typeToBuild);
        }

        public TToResolve Resolve<TToResolve>() where TToResolve : class
        {
            TToResolve toReturn;
            if (!TryResolve(out toReturn))
            {
                throw new MvxException("Unable to Resolve IoC type {0}", typeof (TToResolve));
            }

            return toReturn;
        }

        public bool TryResolve<TToResolve>(out TToResolve instance) where TToResolve : class
        {
            var typeToBuild = typeof (TToResolve);

            object objectReference;
            if (_items.TryGetValue(typeToBuild.FullName, out objectReference))
            {
                instance = objectReference as TToResolve;
                return true;
            }

            if (!CanCreateInstance(typeof (TToResolve)))
            {
                instance = null;
                return false;
            }

            instance = CreateInstance<TToResolve>(typeToBuild);
            return true;
        }

        private bool CanCreateInstance(Type typeToBuild)
        {
#if NETFX_CORE
            if (typeToBuild.GetTypeInfo().IsInterface)
                return _toResolve.ContainsKey(typeToBuild);
#else
            if (typeToBuild.IsInterface)
                return _toResolve.ContainsKey(typeToBuild);
#endif

            // note - the original opennetCf container supported direct type creation - but Mvx supports interfaces only
            throw new MvxException("Unexpected non interface type in call to CanCreateInstance " + typeToBuild.Name);
        }

        private TToResolve CreateInstance<TToResolve>(Type typeToBuild) where TToResolve : class
        {
#if NETFX_CORE
            if (typeToBuild.GetTypeInfo().IsInterface)
                typeToBuild = _toResolve[typeToBuild];
#else
            if (typeToBuild.IsInterface)
                typeToBuild = _toResolve[typeToBuild];
#endif

            var instance = MvxOpenNetCfObjectBuilder.CreateObject(typeToBuild);
            return (TToResolve) instance;
        }

        /// <summary>
        ///   Create a new instance at each time
        /// </summary>
        /// <returns></returns>
        public object GetInstance(Type typeToBuild)
        {
            var instance = MvxOpenNetCfObjectBuilder.CreateObject(typeToBuild);
            return instance;
        }

        /// <summary>
        ///   Create a new instance at each time
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <returns></returns>
        public T GetInstance<T>()
        {
            var typeToBuild = typeof (T);

            return (T) GetInstance(typeToBuild);
        }

        /// <summary>
        /// Create an instance of the required type
        /// </summary>
        /// <param name="typeToBuild">type</param>
        /// <returns></returns>
        public object Resolve(Type typeToBuild)
        {
            if (_items.ContainsKey(typeToBuild.FullName))
                return _items[typeToBuild.FullName];

            return CreateInstance<object>(typeToBuild);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterServiceInstance<TInterface>(TInterface instance)
        {
            if (_items.ContainsKey(typeof (TInterface).FullName))
                return;
            Add(instance, typeof (TInterface).FullName);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="interfaceToRegister">The interface to register.</param>
        /// <param name="instance">The instance.</param>
        public void RegisterServiceInstance(Type interfaceToRegister, object instance)
        {
            if (_items.ContainsKey(interfaceToRegister.FullName))
                _items.Remove(interfaceToRegister.FullName);
            Add(instance, interfaceToRegister.FullName);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="id">The id.</param>
        internal void Add(object item, string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (item == null)
                throw new ArgumentNullException("item");

            lock (_syncRoot)
            {
                _items.Add(id, item);
            }
        }

        /// <summary>
        /// Finds the type of the by.
        /// </summary>
        /// <typeparam name="TSearchType">The type of the search type.</typeparam>
        /// <returns></returns>
        public IEnumerable<TSearchType> FindByType<TSearchType>() where TSearchType : class
        {
            return (_items.Where(i => i.Value is TSearchType).Select(i => i.Value as TSearchType));
        }

        /// <summary>
        /// Finds the type of the by.
        /// </summary>
        /// <param name="searchType">Type of the search.</param>
        /// <returns></returns>
        public IEnumerable<object> FindByType(Type searchType)
        {
#if NETFX_CORE
            if (searchType.GetTypeInfo().IsValueType)
                throw new ArgumentException("searchType must be a reference type");

            if (searchType.GetTypeInfo().IsInterface)
            {
                return (_items.Where(i => i.Value.GetType().GetTypeInfo().ImplementedInterfaces.Contains(searchType)).Select(i => i.Value));
            }
            return (_items.Where(i => i.Value.GetType().Equals(searchType)).Select(i => i.Value));
#else
            if (searchType.IsValueType)
                throw new ArgumentException("searchType must be a reference type");

            if (searchType.IsInterface)
            {
                return (_items.Where(i => i.Value.GetType().GetInterfaces().Contains(searchType)).Select(i => i.Value));
            }
            return (_items.Where(i => i.Value.GetType().Equals(searchType)).Select(i => i.Value));
#endif
        }

        /// <summary>
        /// Gets the type of the resolved.
        /// </summary>
        /// <param name="toResolve">To resolve.</param>
        /// <returns></returns>
        internal object GetResolvedType(Type toResolve)
        {
            if (!_toResolve.ContainsKey(toResolve))
                throw new MvxException("Type is not register");

            if (!_items.ContainsKey(toResolve.FullName))
                return Resolve(toResolve);

            return _toResolve[toResolve];
        }
    }
}