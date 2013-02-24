// MvxSimpleIoCContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;

namespace Cirrious.MvvmCross.IoC
{
    public class MvxSimpleIoCContainer
        : MvxSingleton<IMvxIoCProvider>
        , IMvxIoCProvider
    {
        public static IMvxIoCProvider Initialise()
        {
            if (Instance != null)
            {
                return Instance;
            }

            // create a new ioc container - it will register itself as the singleton
            var ioc = new MvxSimpleIoCContainer();
            return Instance;
        }

        private readonly Dictionary<Type, IResolver> _resolvers = new Dictionary<Type, IResolver>();

        private interface IResolver
        {
            object Resolve();
        }

        private class ConstructingResolver : IResolver
        {
            private readonly Type _type;

            public ConstructingResolver(Type type)
            {
                _type = type;
            }

            #region Implementation of IResolver

            public object Resolve()
            {
                return Activator.CreateInstance(_type);
            }

            #endregion
        }

        private class SingletonResolver : IResolver
        {
            private readonly object _theObject;

            public SingletonResolver(object theObject)
            {
                _theObject = theObject;
            }

            #region Implementation of IResolver

            public object Resolve()
            {
                return _theObject;
            }

            #endregion
        }

        private class ConstructingSingletonResolver : IResolver
        {
            private readonly Func<object> _theConstructor;
            private object _theObject;

            public ConstructingSingletonResolver(Func<object> theConstructor)
            {
                _theConstructor = theConstructor;
            }

            #region Implementation of IResolver

            public object Resolve()
            {
                if (_theObject != null)
                    return _theObject;

                lock (_theConstructor)
                {
                    if (_theObject == null)
                    {
                        _theObject = _theConstructor();
                    }
                }

                return _theObject;
            }

            #endregion
        }

        public bool CanResolve<T>()
            where T : class
        {
            lock (this)
            {
                return _resolvers.ContainsKey(typeof (T));
            }
        }

        public bool TryResolve<T>(out T resolved)
            where T : class
        {
            lock (this)
            {
                IResolver resolver;
                if (!_resolvers.TryGetValue(typeof (T), out resolver))
                {
                    resolved = default(T);
                    return false;
                }

                var raw = resolver.Resolve();
                if (!(raw is T))
                {
                    throw new MvxException("Resolver returned object type {0} which does not support interface {1}",
                                           raw.GetType().FullName, typeof (T).FullName);
                }

                resolved = (T) raw;
                return true;
            }
        }

        public T Resolve<T>()
            where T : class
        {
            lock (this)
            {
                T resolved;
                if (!this.TryResolve(out resolved))
                {
                    throw new MvxException("Failed to resolve type {0}", typeof (T).FullName);
                }
                return resolved;
            }
        }

        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            lock (this)
            {
                _resolvers[typeof (TInterface)] = new ConstructingResolver(typeof (TToConstruct));
            }
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            lock (this)
            {
                _resolvers[typeof (TInterface)] = new SingletonResolver(theObject);
            }
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            lock (this)
            {
                _resolvers[typeof (TInterface)] = new ConstructingSingletonResolver(() => (object) theConstructor());
            }
        }
    }
}