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
        public static IMvxIoCProvider Initialize(bool shouldDetectCircularReferences = true)
        {
            if (Instance != null)
            {
                return Instance;
            }

            // create a new ioc container - it will register itself as the singleton
// ReSharper disable ObjectCreationAsStatement
            new MvxSimpleIoCContainer(shouldDetectCircularReferences);
// ReSharper restore ObjectCreationAsStatement
			return Instance;
        }

        private readonly Dictionary<Type, IResolver> _resolvers = new Dictionary<Type, IResolver>();
        private readonly Dictionary<Type, List<Action>> _waiters = new Dictionary<Type, List<Action>>();
        private readonly object _lockObject = new object();
        protected object LockObject { get { return _lockObject; } }
        private readonly Dictionary<Type, bool> _singletonsCurrentlyBeingConstructed = new Dictionary<Type, bool>();

        protected MvxSimpleIoCContainer(bool shouldDetectCircularReferences = true)
        {
            ShouldDetectCircularReferences = shouldDetectCircularReferences;
        }

        public bool ShouldDetectCircularReferences { get; private set; }

        public interface IResolver
        {
            object Resolve();
            ResolverType ResolveType { get; }
        }

        public class ConstructingResolver : IResolver
        {
            private readonly Type _type;
            private readonly MvxSimpleIoCContainer _parent;

            public ConstructingResolver(Type type, MvxSimpleIoCContainer parent)
            {
                _type = type;
                _parent = parent;
            }

            public object Resolve()
            {
                return _parent.IoCConstruct(_type);
            }

            public ResolverType ResolveType { get { return ResolverType.Singleton; } }
        }

        public class SingletonResolver : IResolver
        {
            private readonly object _theObject;

            public SingletonResolver(object theObject)
            {
                _theObject = theObject;
            }

            public object Resolve()
            {
                return _theObject;
            }

            public ResolverType ResolveType { get { return ResolverType.Singleton;} }
        }

        public class ConstructingSingletonResolver : IResolver
        {
            private readonly Func<object> _theConstructor;
            private object _theObject;

            public ConstructingSingletonResolver(Func<object> theConstructor)
            {
                _theConstructor = theConstructor;
            }

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

            public ResolverType ResolveType { get { return ResolverType.Singleton; } }
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
            try
            {
                object item;
                var toReturn = TryResolve(typeof(T), out item);
                resolved = (T)item;
                return toReturn;
            }
            catch (MvxIoCResolveException)
            {
                resolved = (T)typeof(T).CreateDefault();
                return false;
            }
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
                if (!InternalTryResolve(t, ResolverType.Singleton, out resolved))
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
                if (!InternalTryResolve(t, ResolverType.CreatePerResolve, out resolved))
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

        public enum ResolverType
        {
            CreatePerResolve,
            Singleton,
            Unknown
        }

        private static readonly ResolverType? ResolverTypeNoneSpecified = null;

        private bool Supports(IResolver resolver, ResolverType? requiredResolverType)
        {
            if (!requiredResolverType.HasValue)
                return true;
            return resolver.ResolveType == requiredResolverType.Value;
        }

        private bool InternalTryResolve(Type type, out object resolved)
        {
            return InternalTryResolve(type, ResolverTypeNoneSpecified, out resolved);
        }

        private bool InternalTryResolve(Type type, ResolverType? requiredResolverType, out object resolved)
        {
            IResolver resolver;
            if (!_resolvers.TryGetValue(type, out resolver))
            {
                resolved = type.CreateDefault();
                return false;
            }

            if (!Supports(resolver, requiredResolverType))
            {
                resolved = type.CreateDefault();
                return false;
            }

            return InternalTryResolve(type, resolver, out resolved);
        }

        private bool InternalTryResolve(Type type, IResolver resolver, out object resolved)
        {
            var detectingCircular = ShouldDetectCircularReferences
                                            && resolver.ResolveType == ResolverType.Singleton;
            if (detectingCircular)
            {
                try
                {
                    _singletonsCurrentlyBeingConstructed.Add(type, true);
                }
                catch (ArgumentException)
                {
                    // the item already exists in the lookup table
                    // - this is "game over" for the IoC lookup
                    // - see https://github.com/MvvmCross/MvvmCross/issues/553
                    Mvx.Error("IoC circular reference detected - cannot currently resolve {0}", type.Name);
                    resolved = type.CreateDefault();
                    return false;
                }
            }

            try
            {
                var raw = resolver.Resolve();
                if (!(type.IsInstanceOfType(raw)))
                {
                    throw new MvxException("Resolver returned object type {0} which does not support interface {1}",
                                           raw.GetType().FullName, type.FullName);
                }

                resolved = raw;
                return true;
            }
            finally 
            {
                if (detectingCircular)
                {
                    _singletonsCurrentlyBeingConstructed.Remove(type);
                }
            }
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
                        throw new MvxIoCResolveException(
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