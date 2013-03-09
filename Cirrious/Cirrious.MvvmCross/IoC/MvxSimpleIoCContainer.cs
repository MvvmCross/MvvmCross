// MvxSimpleIoCContainer.cs
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
            private readonly MvxSimpleIoCContainer _parent;

            public ConstructingResolver(Type type, MvxSimpleIoCContainer parent)
            {
                _type = type;
                _parent = parent;
            }

            #region Implementation of IResolver

            public object Resolve()
            {
                return _parent.IoCConstruct(_type);
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
                        _theObject = _theConstructor();
                }

                return _theObject;
            }

            #endregion
        }

        public bool CanResolve<T>()
            where T : class
        {
            return CanResolve(typeof (T));
        }

        public bool CanResolve(Type t)
        {
            lock (this)
            {
                return _resolvers.ContainsKey(t);
            }
        }

        public bool TryResolve<T>(out T resolved)
            where T : class
        {
            object item;
            var toReturn = TryResolve(typeof (T), out item);
            resolved = (T) item;
            return toReturn;
        }

        public bool TryResolve(Type type, out object resolved)
        {
            lock (this)
            {
                IResolver resolver;
                if (!_resolvers.TryGetValue(type, out resolver))
                {
                    resolved = type.IsValueType ? Activator.CreateInstance(type) : null;
                    return false;
                }

                var raw = resolver.Resolve();
                if (!(type.IsInstanceOfType(raw)))
                {
                    throw new MvxException("Resolver returned object type {0} which does not support interface {1}",
                                           raw.GetType().FullName, type.FullName);
                }

                resolved = raw;
                return true;
            }
        }

        public T Resolve<T>()
            where T : class
        {
            return (T) Resolve(typeof (T));
        }

        public object Resolve(Type t)
        {
            lock (this)
            {
                object resolved;
                if (!TryResolve(t, out resolved))
                {
                    throw new MvxException("Failed to resolve type {0}", t.FullName);
                }
                return resolved;
            }
        }

        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            RegisterType(typeof (TInterface), typeof (TToConstruct));
        }

        public void RegisterType(Type tInterface, Type tConstruct)
        {
            lock (this)
            {
                _resolvers[tInterface] = new ConstructingResolver(tConstruct, this);
            }
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            RegisterSingleton(typeof (TInterface), theObject);
        }

        public void RegisterSingleton(Type tInterface, object theObject)
        {
            lock (this)
            {
                _resolvers[tInterface] = new SingletonResolver(theObject);
            }
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            RegisterSingleton(typeof (TInterface), theConstructor);
        }

        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            lock (this)
            {
                _resolvers[tInterface] = new ConstructingSingletonResolver(theConstructor);
            }
        }

        public T IoCConstruct<T>()
            where T : class
        {
            return (T) IoCConstruct(typeof (T));
        }

        public object IoCConstruct(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var firstConstructor = constructors.FirstOrDefault();

            if (firstConstructor == null)
                throw new MvxException("Failed to find constructor for type {0}", type.FullName);

            var parameters = GetIoCParameterValues(type, firstConstructor);
            var toReturn = firstConstructor.Invoke(parameters.ToArray());

            // decide not to do property injection for now
            //InjectProperties(type, toReturn);

            return toReturn;
        }

#warning Remove InjectProperties then
        /*
        private void InjectProperties(Type type, object toReturn)
        {
            var injectableProperties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.PropertyType.IsInterface)
                .Where(p => p.CanWrite);

            foreach (var injectableProperty in injectableProperties)
            {
                object propertyValue;
                if (TryResolve(injectableProperty.PropertyType, out propertyValue))
                {
                    injectableProperty.SetValue(toReturn, propertyValue, null);
                }
            }
        }
        */

        private List<object> GetIoCParameterValues(Type type, ConstructorInfo firstConstructor)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in firstConstructor.GetParameters())
            {
                object parameterValue;
                if (!TryResolve(parameterInfo.ParameterType, out parameterValue))
                {
                    throw new MvxException(
                        "Failed to resolve parameter for parameter {0} of type {1} when creating {2}",
                        parameterInfo.Name,
                        parameterInfo.ParameterType.Name,
                        type.FullName);
                }

                parameters.Add(parameterValue);
            }
            return parameters;
        }
    }
}