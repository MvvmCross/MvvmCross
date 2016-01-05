// MvxSimpleIoCContainer.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;

    public class MvxSimpleIoCContainer
        : MvxSingleton<IMvxIoCProvider>
          , IMvxIoCProvider
    {
        public static IMvxIoCProvider Initialize(IMvxIocOptions options = null)
        {
            if (Instance != null)
            {
                return Instance;
            }

            // create a new ioc container - it will register itself as the singleton
            // ReSharper disable ObjectCreationAsStatement
            new MvxSimpleIoCContainer(options);
            // ReSharper restore ObjectCreationAsStatement
            return Instance;
        }

        private readonly Dictionary<Type, IResolver> _resolvers = new Dictionary<Type, IResolver>();
        private readonly Dictionary<Type, List<Action>> _waiters = new Dictionary<Type, List<Action>>();
        private readonly Dictionary<Type, bool> _circularTypeDetection = new Dictionary<Type, bool>();
        private readonly object _lockObject = new object();
        private readonly IMvxIocOptions _options;
        private readonly IMvxPropertyInjector _propertyInjector;

        protected object LockObject => this._lockObject;
        protected IMvxIocOptions Options => this._options;

        protected MvxSimpleIoCContainer(IMvxIocOptions options)
        {
            this._options = options ?? new MvxIocOptions();
            if (this._options.PropertyInjectorType != null)
            {
                this._propertyInjector = Activator.CreateInstance(this._options.PropertyInjectorType) as IMvxPropertyInjector;
            }
            if (this._propertyInjector != null)
            {
                this.RegisterSingleton(typeof(IMvxPropertyInjector), this._propertyInjector);
            }
        }

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
                this._type = type;
                this._parent = parent;
            }

            public object Resolve()
            {
                return this._parent.IoCConstruct(this._type);
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public class FuncConstructingResolver : IResolver
        {
            private readonly Func<object> _constructor;

            public FuncConstructingResolver(Func<object> constructor)
            {
                this._constructor = constructor;
            }

            public object Resolve()
            {
                return this._constructor();
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public class SingletonResolver : IResolver
        {
            private readonly object _theObject;

            public SingletonResolver(object theObject)
            {
                this._theObject = theObject;
            }

            public object Resolve()
            {
                return this._theObject;
            }

            public ResolverType ResolveType => ResolverType.Singleton;
        }

        public class ConstructingSingletonResolver : IResolver
        {
            private readonly object _syncObject = new object();
            private readonly Func<object> _constructor;
            private object _theObject;

            public ConstructingSingletonResolver(Func<object> theConstructor)
            {
                this._constructor = theConstructor;
            }

            public object Resolve()
            {
                if (this._theObject != null)
                    return this._theObject;

                lock (this._syncObject)
                {
                    if (this._theObject == null)
                        this._theObject = this._constructor();
                }

                return this._theObject;
            }

            public ResolverType ResolveType => ResolverType.Singleton;
        }

        public bool CanResolve<T>()
            where T : class
        {
            return this.CanResolve(typeof(T));
        }

        public bool CanResolve(Type t)
        {
            lock (this._lockObject)
            {
                return this._resolvers.ContainsKey(t);
            }
        }

        public bool TryResolve<T>(out T resolved)
            where T : class
        {
            try
            {
                object item;
                var toReturn = this.TryResolve(typeof(T), out item);
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
            lock (this._lockObject)
            {
                return this.InternalTryResolve(type, out resolved);
            }
        }

        public T Resolve<T>()
            where T : class
        {
            return (T)this.Resolve(typeof(T));
        }

        public object Resolve(Type t)
        {
            lock (this._lockObject)
            {
                object resolved;
                if (!this.InternalTryResolve(t, out resolved))
                {
                    throw new MvxIoCResolveException("Failed to resolve type {0}", t.FullName);
                }
                return resolved;
            }
        }

        public T GetSingleton<T>()
            where T : class
        {
            return (T)this.GetSingleton(typeof(T));
        }

        public object GetSingleton(Type t)
        {
            lock (this._lockObject)
            {
                object resolved;
                if (!this.InternalTryResolve(t, ResolverType.Singleton, out resolved))
                {
                    throw new MvxIoCResolveException("Failed to resolve type {0}", t.FullName);
                }
                return resolved;
            }
        }

        public T Create<T>()
            where T : class
        {
            return (T)this.Create(typeof(T));
        }

        public object Create(Type t)
        {
            lock (this._lockObject)
            {
                object resolved;
                if (!this.InternalTryResolve(t, ResolverType.DynamicPerResolve, out resolved))
                {
                    throw new MvxIoCResolveException("Failed to resolve type {0}", t.FullName);
                }
                return resolved;
            }
        }

        public void RegisterType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            this.RegisterType(typeof(TInterface), typeof(TToConstruct));
        }

        public void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            var resolver = new FuncConstructingResolver(constructor);
            this.InternalSetResolver(typeof(TInterface), resolver);
        }

        public void RegisterType(Type t, Func<object> constructor)
        {
            var resolver = new FuncConstructingResolver(() =>
            {
                var ret = constructor();
                if ((ret != null) && !t.IsInstanceOfType(ret))
                    throw new MvxIoCResolveException("Constructor failed to return a compatibly object for type {0}", t.FullName);

                return ret;
            });

            this.InternalSetResolver(t, resolver);
        }

        public void RegisterType(Type tInterface, Type tConstruct)
        {
            var resolver = new ConstructingResolver(tConstruct, this);
            this.InternalSetResolver(tInterface, resolver);
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            this.RegisterSingleton(typeof(TInterface), theObject);
        }

        public void RegisterSingleton(Type tInterface, object theObject)
        {
            this.InternalSetResolver(tInterface, new SingletonResolver(theObject));
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
#warning when the MonoTouch/Droid code fully supports CoVariance (Contra?) then we can change this)
            this.RegisterSingleton(typeof(TInterface), () => (object)theConstructor());
        }

        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            this.InternalSetResolver(tInterface, new ConstructingSingletonResolver(theConstructor));
        }

        public T IoCConstruct<T>()
            where T : class
        {
            return (T)this.IoCConstruct(typeof(T));
        }

        public virtual object IoCConstruct(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var firstConstructor = constructors.FirstOrDefault();

            if (firstConstructor == null)
                throw new MvxIoCResolveException("Failed to find constructor for type {0}", type.FullName);

            var parameters = this.GetIoCParameterValues(type, firstConstructor);
            object toReturn;
            try
            {
                toReturn = firstConstructor.Invoke(parameters.ToArray());
            }
            catch (TargetInvocationException invocation)
            {
                throw new MvxIoCResolveException(invocation, "Failed to construct {0}", type.Name);
            }

            try
            {
                this.InjectProperties(toReturn);
            }
            catch (Exception)
            {
                if (!this.Options.CheckDisposeIfPropertyInjectionFails)
                    throw;

                toReturn.DisposeIfDisposable();
                throw;
            }
            return toReturn;
        }

        public void CallbackWhenRegistered<T>(Action action)
        {
            this.CallbackWhenRegistered(typeof(T), action);
        }

        public void CallbackWhenRegistered(Type type, Action action)
        {
            lock (this._lockObject)
            {
                if (!this.CanResolve(type))
                {
                    List<Action> actions;
                    if (this._waiters.TryGetValue(type, out actions))
                    {
                        actions.Add(action);
                    }
                    else
                    {
                        actions = new List<Action> { action };
                        this._waiters[type] = actions;
                    }
                    return;
                }
            }

            // if we get here then the type is already registered - so call the aciton immediately
            action();
        }

        public enum ResolverType
        {
            DynamicPerResolve,
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
            return this.InternalTryResolve(type, ResolverTypeNoneSpecified, out resolved);
        }

        private bool InternalTryResolve(Type type, ResolverType? requiredResolverType, out object resolved)
        {
            IResolver resolver;
            if (!this._resolvers.TryGetValue(type, out resolver))
            {
                resolved = type.CreateDefault();
                return false;
            }

            if (!this.Supports(resolver, requiredResolverType))
            {
                resolved = type.CreateDefault();
                return false;
            }

            return this.InternalTryResolve(type, resolver, out resolved);
        }

        private bool ShouldDetectCircularReferencesFor(IResolver resolver)
        {
            switch (resolver.ResolveType)
            {
                case ResolverType.DynamicPerResolve:
                    return this.Options.TryToDetectDynamicCircularReferences;

                case ResolverType.Singleton:
                    return this.Options.TryToDetectSingletonCircularReferences;

                case ResolverType.Unknown:
                    throw new MvxException("A resolver must have a known type - error in {0}", resolver.GetType().Name);
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolver), "unknown resolveType of " + resolver.ResolveType);
            }
        }

        private bool InternalTryResolve(Type type, IResolver resolver, out object resolved)
        {
            var detectingCircular = this.ShouldDetectCircularReferencesFor(resolver);
            if (detectingCircular)
            {
                try
                {
                    this._circularTypeDetection.Add(type, true);
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
                if (!type.IsInstanceOfType(raw))
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
                    this._circularTypeDetection.Remove(type);
                }
            }
        }

        private void InternalSetResolver(Type tInterface, IResolver resolver)
        {
            List<Action> actions;
            lock (this._lockObject)
            {
                this._resolvers[tInterface] = resolver;
                if (this._waiters.TryGetValue(tInterface, out actions))
                    this._waiters.Remove(tInterface);
            }

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action();
                }
            }
        }

        protected virtual void InjectProperties(object toReturn)
        {
            this._propertyInjector?.Inject(toReturn, this._options.PropertyInjectorOptions);
        }

        protected virtual List<object> GetIoCParameterValues(Type type, ConstructorInfo firstConstructor)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in firstConstructor.GetParameters())
            {
                object parameterValue;
                if (!this.TryResolve(parameterInfo.ParameterType, out parameterValue))
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