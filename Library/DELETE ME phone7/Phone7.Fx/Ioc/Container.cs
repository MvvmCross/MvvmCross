#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Phone7.Fx.Ioc
{
    public sealed class Container
    {
        private static Container _current;
        private readonly Dictionary<string, object> _items = new Dictionary<string, object>();
        private readonly object _syncRoot = new object();
        private readonly Dictionary<Type, Type> _toResolve = new Dictionary<Type, Type>();

        /// <summary>
        ///   Gets the current instance of the IOC container.
        /// </summary>
        /// <value>The current.</value>
        public static Container Current
        {
            get { return _current ?? (_current = new Container()); }
        }

        /// <summary>
        ///   Registers the type.
        /// </summary>
        /// <typeparam name = "TFrom">The type of from.</typeparam>
        /// <typeparam name = "TTo">The type of to.</typeparam>
        public void RegisterType<TFrom, TTo>()
        {
            if (_toResolve.ContainsKey(typeof (TFrom)))
                throw new AccessViolationException("Type already register");
            _toResolve.Add(typeof (TFrom), typeof (TTo));
        }


        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="typeFrom">The type from.</param>
        /// <param name="typeTo">The type to.</param>
        public void RegisterType(Type typeFrom, Type typeTo)
        {
            if (_toResolve.ContainsKey(typeFrom))
                throw new AccessViolationException("Type already register");
            _toResolve.Add(typeFrom, typeTo);
        }

        public TToResolve Resolve<TToResolve>() where TToResolve : class
        {
            Type typeToBuild = typeof (TToResolve);

            if (typeToBuild.IsInterface)
                typeToBuild = _toResolve[typeToBuild];

            if (_items.ContainsKey(typeToBuild.FullName))
                return _items[typeToBuild.FullName] as TToResolve;

            object instance = ObjectBuilder.CreateObject(typeToBuild);
            Add(instance, typeToBuild.FullName);

            return (TToResolve) instance;
        }

        /// <summary>
        ///   Create a new instance at each time
        /// </summary>
        /// <returns></returns>
        public object GetInstance(Type typeToBuild)
        {
            object instance = ObjectBuilder.CreateObject(typeToBuild);
            Add(instance, typeToBuild.FullName + instance.GetHashCode());

            return instance;
        }

        /// <summary>
        ///   Create a new instance at each time
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <returns></returns>
        public T GetInstance<T>()
        {
            Type typeToBuild = typeof (T);

            return (T) GetInstance(typeToBuild);
        }

        public object Resolve(Type typeToBuild)
        {
            if (_items.ContainsKey(typeToBuild.FullName))
                return _items[typeToBuild.FullName];

            if (typeToBuild.IsInterface)
                typeToBuild = _toResolve[typeToBuild];

            object instance = ObjectBuilder.CreateObject(typeToBuild);
            Add(instance, typeToBuild.FullName);

            return instance;
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<TInterface>(TInterface instance)
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
        public void RegisterInstance(Type interfaceToRegister, object instance)
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
            if (searchType.IsValueType)
                throw new ArgumentException("searchType must be a reference type");

            if (searchType.IsInterface)
            {
                return (_items.Where(i => i.Value.GetType().GetInterfaces().Contains(searchType)).Select(i => i.Value));
            }
            return (_items.Where(i => i.Value.GetType().Equals(searchType)).Select(i => i.Value));
        }

        /// <summary>
        /// Gets the type of the resolved.
        /// </summary>
        /// <param name="toResolve">To resolve.</param>
        /// <returns></returns>
        internal object GetResolvedType(Type toResolve)
        {
            if (!_toResolve.ContainsKey(toResolve))
                throw new AccessViolationException("Type is not register");

            if (!_items.ContainsKey(toResolve.FullName))
                return Resolve(toResolve);

            return _toResolve[toResolve];
        }
    }
}