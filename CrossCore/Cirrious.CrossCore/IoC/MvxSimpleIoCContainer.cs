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

namespace Cirrious.CrossCore.IoC
{
    public class MvxSimpleIoCContainer
        : MvxSingleton<IMvxIoCProvider>
          , IMvxIoCProvider
    {
        public static IMvxIoCProvider Initialize()
        {
            if (Instance != null)
            {
                return Instance;
            }

            // create a new ioc container - it will register itself as the singleton
            new MvxSimpleIoCContainer();
			return Instance;
        }

        private readonly Dictionary<Type, IResolver> _resolvers = new Dictionary<Type, IResolver>();
        private readonly Dictionary<Type, List<Action>> _waiters = new Dictionary<Type, List<Action>>();
        private readonly object _lockObject = new object();
        protected object LockObject { get { return _lockObject; } }

        private interface IResolver
        {
            object Resolve();
            bool Supports(ResolveOptions options);
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

            public bool Supports(ResolveOptions options)
            {
                return options != ResolveOptions.SingletonOnly;
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

            public bool Supports(ResolveOptions options)
            {
                return options != ResolveOptions.CreateOnly;
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

            public bool Supports(ResolveOptions options)
            {
                return options != ResolveOptions.CreateOnly;
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
            lock (_lockObject)
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
            lock (_lockObject)
            {
                return InternalTryResolve(type, out resolved);
            }
        }

        public T Resolve<T>()
            where T : class
        {
            return (T) Resolve(typeof (T));
        }

        public object Resolve(Type t)
        {
            lock (_lockObject)
            {
                object resolved;
                if (!InternalTryResolve(t, out resolved))
                {
                    throw new MvxException("Failed to resolve type {0}", t.FullName);
                }
                return resolved;
            }
        }

        public T GetSingleton<T>()
            where T : class
        {
            return (T) GetSingleton(typeof (T));
        }

        public object GetSingleton(Type t)
        {
            lock (_lockObject)
            {
                object resolved;
                if (!InternalTryResolve(t, ResolveOptions.SingletonOnly, out resolved))
                {
                    throw new MvxException("Failed to resolve type {0}", t.FullName);
                }
                return resolved;
            }
        }

        public T Create<T>()
            where T : class
        {
            return (T) Create(typeof (T));
        }

        public object Create(Type t)
        {
            lock (_lockObject)
            {
                object resolved;
                if (!InternalTryResolve(t, ResolveOptions.CreateOnly, out resolved))
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
            var resolver = new ConstructingResolver(tConstruct, this);
            InternalSetResolver(tInterface, resolver);
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            RegisterSingleton(typeof (TInterface), theObject);
        }

        public void RegisterSingleton(Type tInterface, object theObject)
        {
            InternalSetResolver(tInterface, new SingletonResolver(theObject));
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
#warning when the MonoTouch/Droid code fully supports CoVariance (Contra?) then we can change this)
            RegisterSingleton(typeof (TInterface), () => (object) theConstructor());
        }

        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            InternalSetResolver(tInterface, new ConstructingSingletonResolver(theConstructor));
        }

        public T IoCConstruct<T>()
            where T : class
        {
            return (T) IoCConstruct(typeof (T));
        }

        public virtual object IoCConstruct(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var firstConstructor = constructors.FirstOrDefault();

            if (firstConstructor == null)
                throw new MvxException("Failed to find constructor for type {0}", type.FullName);

            var parameters = GetIoCParameterValues(type, firstConstructor);
            var toReturn = firstConstructor.Invoke(parameters.ToArray());

            return toReturn;
        }

        public void CallbackWhenRegistered<T>(Action action)
        {
            CallbackWhenRegistered(typeof (T), action);
        }

        public void CallbackWhenRegistered(Type type, Action action)
        {
            lock (_lockObject)
            {
                if (!CanResolve(type))
                {
                    List<Action> actions;
                    if (_waiters.TryGetValue(type, out actions))
                    {
                        actions.Add(action);
                    }
                    else
                    {
                        actions = new List<Action> {action};
                        _waiters[type] = actions;
                    }
                    return;
                }
            }

            // if we get here then the type is already registered - so call the aciton immediately
            action();
        }

        private enum ResolveOptions
        {
            All,
            CreateOnly,
            SingletonOnly
        }

        private bool InternalTryResolve(Type type, out object resolved)
        {
            return InternalTryResolve(type, ResolveOptions.All, out resolved);
        }

        private bool InternalTryResolve(Type type, ResolveOptions resolveOptions, out object resolved)
        {
            IResolver resolver;
            if (!_resolvers.TryGetValue(type, out resolver))
            {
                resolved = type.CreateDefault();
                return false;
            }

            if (!resolver.Supports(resolveOptions))
            {
                resolved = type.CreateDefault();
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

        private void InternalSetResolver(Type tInterface, IResolver resolver)
        {
            List<Action> actions;
            lock (this)
            {
                _resolvers[tInterface] = resolver;
                if (_waiters.TryGetValue(tInterface, out actions))
                    _waiters.Remove(tInterface);
            }

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action();
                }
            }
        }

        protected virtual List<object> GetIoCParameterValues(Type type, ConstructorInfo firstConstructor)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in firstConstructor.GetParameters())
            {
                object parameterValue;
                if (!TryResolve(parameterInfo.ParameterType, out parameterValue))
                {
                    if (parameterInfo.IsOptional)
                    {
                        parameterValue = Type.Missing;
                    }
                    else
                    {
                        throw new MvxException(
                            "Failed to resolve parameter for parameter {0} of type {1} when creating {2}",
                            parameterInfo.Name,
                            parameterInfo.ParameterType.Name,
                            type.FullName);
                    }
                }

                parameters.Add(parameterValue);
            }
            return parameters;
        }
    }
}