// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.IoC
{
    public class MvxIoCContainer
        : IMvxIoCProvider
    {
        private readonly Dictionary<Type, IResolver> _resolvers = new Dictionary<Type, IResolver>();
        private readonly Dictionary<Type, List<Action>> _waiters = new Dictionary<Type, List<Action>>();
        private readonly Dictionary<Type, bool> _circularTypeDetection = new Dictionary<Type, bool>();
        private readonly object _lockObject = new object();
        private readonly IMvxIocOptions _options;
        private readonly IMvxPropertyInjector _propertyInjector;
        private readonly IMvxIoCProvider _parentProvider;

        protected object LockObject => _lockObject;

        protected IMvxIocOptions Options => _options;

        public MvxIoCContainer(IMvxIocOptions options, IMvxIoCProvider parentProvider = null)
        {
            _options = options ?? new MvxIocOptions();
            if (_options.PropertyInjectorType != null)
            {
                _propertyInjector = Activator.CreateInstance(_options.PropertyInjectorType) as IMvxPropertyInjector;
            }
            if (_propertyInjector != null)
            {
                RegisterSingleton(typeof(IMvxPropertyInjector), _propertyInjector);
            }
            if (parentProvider != null)
            {
                _parentProvider = parentProvider;
            }
        }

        public MvxIoCContainer(IMvxIoCProvider parentProvider)
            : this(null, parentProvider)
        {
            if (parentProvider == null)
            {
                throw new ArgumentNullException(nameof(parentProvider), "Provide a parent ioc provider to this constructor");
            }
        }

        public interface IResolver
        {
            [RequiresUnreferencedCode("Resolve is incompatible with trimming")]
            object Resolve();

            ResolverType ResolveType { get; }

            void SetGenericTypeParameters(Type[] genericTypeParameters);
        }

        public class ConstructingResolver : IResolver
        {
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
            private readonly Type _type;
            private readonly IMvxIoCProvider _parent;

            public ConstructingResolver([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, IMvxIoCProvider parent)
            {
                _type = type;
                _parent = parent;
            }

            [RequiresUnreferencedCode("Resolve is incompatible with trimming")]
            public object Resolve()
            {
                return _parent.IoCConstruct(_type);
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public class FuncConstructingResolver : IResolver
        {
            private readonly Func<object> _constructor;

            public FuncConstructingResolver(Func<object> constructor)
            {
                _constructor = constructor;
            }

            [RequiresUnreferencedCode("Resolve is incompatible with trimming")]
            public object Resolve()
            {
                return _constructor();
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public class SingletonResolver : IResolver
        {
            private readonly object _theObject;

            public SingletonResolver(object theObject)
            {
                _theObject = theObject;
            }

            [RequiresUnreferencedCode("Resolve is incompatible with trimming")]
            public object Resolve()
            {
                return _theObject;
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
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
                _constructor = theConstructor;
            }

            [RequiresUnreferencedCode("Resolve is incompatible with trimming")]
            public object Resolve()
            {
                if (_theObject != null)
                    return _theObject;

                lock (_syncObject)
                {
                    if (_theObject == null)
                        _theObject = _constructor();
                }

                return _theObject;
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                throw new InvalidOperationException("This Resolver does not set generic type parameters");
            }

            public ResolverType ResolveType => ResolverType.Singleton;
        }

        public class ConstructingOpenGenericResolver : IResolver
        {
            private readonly Type _genericTypeDefinition;
            private readonly IMvxIoCProvider _parent;

            private Type[] _genericTypeParameters;

            public ConstructingOpenGenericResolver(Type genericTypeDefinition, IMvxIoCProvider parent)
            {
                _genericTypeDefinition = genericTypeDefinition;
                _parent = parent;
            }

            public void SetGenericTypeParameters(Type[] genericTypeParameters)
            {
                _genericTypeParameters = genericTypeParameters;
            }

            [RequiresUnreferencedCode("Resolve is incompatible with trimming")]
            public object Resolve()
            {
                return _parent.IoCConstruct(_genericTypeDefinition.MakeGenericType(_genericTypeParameters));
            }

            public ResolverType ResolveType => ResolverType.DynamicPerResolve;
        }

        public bool CanResolve<T>()
            where T : class
        {
            return CanResolve(typeof(T));
        }

        public bool CanResolve(Type type)
        {
            lock (_lockObject)
            {
                if (_resolvers.ContainsKey(type))
                {
                    return true;
                }
                if (_parentProvider != null && _parentProvider.CanResolve(type))
                {
                    return true;
                }
                return false;
            }
        }

        [RequiresUnreferencedCode("TryResolve is not compatible with trimming")]
        public bool TryResolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(out T resolved)
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

        [RequiresUnreferencedCode("TryResolve is not compatible with trimming")]
        public bool TryResolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, out object resolved)
        {
            lock (_lockObject)
            {
                return InternalTryResolve(type, out resolved);
            }
        }

        [RequiresUnreferencedCode("Resolve is not compatible with trimming")]
        public T Resolve<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class
        {
            return (T)Resolve(typeof(T));
        }

        [RequiresUnreferencedCode("Resolve is not compatible with trimming")]
        public object Resolve([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
        {
            lock (_lockObject)
            {
                object resolved;
                if (!InternalTryResolve(type, out resolved))
                {
                    throw new MvxIoCResolveException("Failed to resolve type {0}", type.FullName);
                }
                return resolved;
            }
        }

        [RequiresUnreferencedCode("GetSingleton is not compatible with trimming")]
        public T GetSingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class
        {
            return (T)GetSingleton(typeof(T));
        }

        [RequiresUnreferencedCode("GetSingleton is not compatible with trimming")]
        public object GetSingleton([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
        {
            lock (_lockObject)
            {
                object resolved;
                if (!InternalTryResolve(type, ResolverType.Singleton, out resolved))
                {
                    throw new MvxIoCResolveException("Failed to resolve type {0}", type.FullName);
                }
                return resolved;
            }
        }

        [RequiresUnreferencedCode("Create is not compatible with trimming")]
        public T Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class
        {
            return (T)Create(typeof(T));
        }

        [RequiresUnreferencedCode("Create is not compatible with trimming")]
        public object Create([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
        {
            lock (_lockObject)
            {
                object resolved;
                if (!InternalTryResolve(type, ResolverType.DynamicPerResolve, out resolved))
                {
                    throw new MvxIoCResolveException("Failed to resolve type {0}", type.FullName);
                }
                return resolved;
            }
        }

        public void RegisterType<TInterface, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface
        {
            RegisterType(typeof(TInterface), typeof(TToConstruct));
        }

        public void RegisterType<TInterface>(Func<TInterface> constructor)
            where TInterface : class
        {
            var resolver = new FuncConstructingResolver(constructor);
            InternalSetResolver(typeof(TInterface), resolver);
        }

        public void RegisterType(Type t, Func<object> constructor)
        {
            var resolver = new FuncConstructingResolver(() =>
            {
                var ret = constructor();
                if (ret != null && !t.IsInstanceOfType(ret))
                    throw new MvxIoCResolveException("Constructor failed to return a compatibly object for type {0}", t.FullName);

                return ret;
            });

            InternalSetResolver(t, resolver);
        }

        public void RegisterType(Type tFrom, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type tTo)
        {
            IResolver resolver = null;
            if (tFrom.GetTypeInfo().IsGenericTypeDefinition)
            {
                resolver = new ConstructingOpenGenericResolver(tTo, this);
            }
            else
            {
                resolver = new ConstructingResolver(tTo, this);
            }

            InternalSetResolver(tFrom, resolver);
        }

        public void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class
        {
            RegisterSingleton(typeof(TInterface), theObject);
        }

        public void RegisterSingleton(Type tInterface, object theObject)
        {
            InternalSetResolver(tInterface, new SingletonResolver(theObject));
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class
        {
            RegisterSingleton(typeof(TInterface), theConstructor);
        }

        public void RegisterSingleton(Type tInterface, Func<object> theConstructor)
        {
            InternalSetResolver(tInterface, new ConstructingSingletonResolver(theConstructor));
        }

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        public object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
        {
            return IoCConstruct(type, default(IDictionary<string, object>));
        }

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        public T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class
        {
            return (T)IoCConstruct(typeof(T));
        }

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        public virtual object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, object arguments)
        {
            return IoCConstruct(type, arguments.ToPropertyDictionary());
        }

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        public virtual T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(IDictionary<string, object> arguments)
            where T : class
        {
            return (T)IoCConstruct(typeof(T), arguments);
        }

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        public virtual T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(object arguments)
            where T : class
        {
            return (T)IoCConstruct(typeof(T), arguments.ToPropertyDictionary());
        }

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        public virtual T IoCConstruct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(params object[] arguments) where T : class
        {
            return (T)IoCConstruct(typeof(T), arguments);
        }

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        public virtual object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, params object[] arguments)
        {
            var selectedConstructor = type.FindApplicableConstructor(arguments);

            if (selectedConstructor == null)
            {
                throw new MvxIoCResolveException($"Failed to find constructor for type {type.FullName} with arguments: {arguments.Select(x => x.GetType().Name + ", ")}");
            }

            var parameters = GetIoCParameterValues(type, selectedConstructor, arguments);
            return IoCConstruct(type, selectedConstructor, parameters.ToArray());
        }

        [RequiresUnreferencedCode("IoCConstruct is incompatible with trimming")]
        public virtual object IoCConstruct(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, IDictionary<string, object> arguments)
        {
            var selectedConstructor = type.FindApplicableConstructor(arguments);

            if (selectedConstructor == null)
            {
                throw new MvxIoCResolveException("Failed to find constructor for type {0}", type.FullName);
            }

            var parameters = GetIoCParameterValues(type, selectedConstructor, arguments);
            return IoCConstruct(type, selectedConstructor, parameters.ToArray());
        }

        protected virtual object IoCConstruct([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, ConstructorInfo constructor, object[] arguments)
        {
            object toReturn;
            try
            {
                toReturn = constructor.Invoke(arguments);
            }
            catch (TargetInvocationException invocation)
            {
                throw new MvxIoCResolveException(invocation, "Failed to construct {0}", type.Name);
            }

            try
            {
                InjectProperties(toReturn);
            }
            catch (Exception)
            {
                if (!Options.CheckDisposeIfPropertyInjectionFails)
                    throw;

                toReturn.DisposeIfDisposable();
                throw;
            }
            return toReturn;
        }

        public void CallbackWhenRegistered<T>(Action action)
        {
            CallbackWhenRegistered(typeof(T), action);
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
                        actions = new List<Action> { action };
                        _waiters[type] = actions;
                    }
                    return;
                }
            }

            // if we get here then the type is already registered - so call the aciton immediately
            action();
        }

        public void CleanAllResolvers()
        {
            _resolvers.Clear();
            _waiters.Clear();
            _circularTypeDetection.Clear();
        }

        public enum ResolverType
        {
            DynamicPerResolve,
            Singleton,
            Unknown
        }

        public virtual IMvxIoCProvider CreateChildContainer() => new MvxIoCContainer(this);

        private static readonly ResolverType? ResolverTypeNoneSpecified = null;

        private bool Supports(IResolver resolver, ResolverType? requiredResolverType)
        {
            if (!requiredResolverType.HasValue)
                return true;
            return resolver.ResolveType == requiredResolverType.Value;
        }

        [RequiresUnreferencedCode("InternalTryResolve is not compatible with trimming")]
        private bool InternalTryResolve(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, out object resolved)
        {
            return InternalTryResolve(type, ResolverTypeNoneSpecified, out resolved);
        }

        [RequiresUnreferencedCode("InternalTryResolve is not compatible with trimming")]
        private bool InternalTryResolve(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, ResolverType? requiredResolverType, out object resolved)
        {
            IResolver resolver;
            if (!TryGetResolver(type, out resolver))
            {
                if (_parentProvider != null && _parentProvider.TryResolve(type, out resolved))
                {
                    return true;
                }

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

        [RequiresUnreferencedCode("InternalTryResolve is not compatible with trimming")]
        private bool InternalTryResolve(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type, IResolver resolver, out object resolved)
        {
            var detectingCircular = ShouldDetectCircularReferencesFor(resolver);
            if (detectingCircular)
            {
                try
                {
                    _circularTypeDetection.Add(type, true);
                }
                catch (ArgumentException)
                {
                    // the item already exists in the lookup table
                    // - this is "game over" for the IoC lookup
                    // - see https://github.com/MvvmCross/MvvmCross/issues/553
                    MvxLogHost.Default?.Log(LogLevel.Error, "IoC circular reference detected - cannot currently resolve {typeName}", type.Name);
                    resolved = type.CreateDefault();
                    return false;
                }
            }

            try
            {
                if (resolver is ConstructingOpenGenericResolver)
                {
                    resolver.SetGenericTypeParameters(type.GetTypeInfo().GenericTypeArguments);
                }

                var raw = resolver.Resolve();
                if (raw == null)
                {
                    throw new MvxException("Resolver returned null");
                }
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
                    _circularTypeDetection.Remove(type);
                }
            }
        }

        private bool TryGetResolver(Type type, out IResolver resolver)
        {
            if (_resolvers.TryGetValue(type, out resolver))
            {
                return true;
            }

            if (!type.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            return _resolvers.TryGetValue(type.GetTypeInfo().GetGenericTypeDefinition(), out resolver);
        }

        private bool ShouldDetectCircularReferencesFor(IResolver resolver)
        {
            switch (resolver.ResolveType)
            {
                case ResolverType.DynamicPerResolve:
                    return Options.TryToDetectDynamicCircularReferences;

                case ResolverType.Singleton:
                    return Options.TryToDetectSingletonCircularReferences;

                case ResolverType.Unknown:
                    throw new MvxException("A resolver must have a known type - error in {0}", resolver.GetType().Name);
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolver), "unknown resolveType of " + resolver.ResolveType);
            }
        }

        private void InternalSetResolver(Type interfaceType, IResolver resolver)
        {
            List<Action> actions;
            lock (_lockObject)
            {
                _resolvers[interfaceType] = resolver;
                if (_waiters.TryGetValue(interfaceType, out actions))
                {
                    _waiters.Remove(interfaceType);
                }
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
            _propertyInjector?.Inject(toReturn, _options.PropertyInjectorOptions);
        }

        [RequiresUnreferencedCode("TryResolveParameter is incompatible with trimming")]
        protected virtual List<object> GetIoCParameterValues(Type type, ConstructorInfo selectedConstructor, IDictionary<string, object> arguments)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in selectedConstructor.GetParameters())
            {
                if (arguments != null && arguments.ContainsKey(parameterInfo.Name))
                {
                    parameters.Add(arguments[parameterInfo.Name]);
                }
                else if (TryResolveParameter(type, parameterInfo, out var parameterValue))
                {
                    parameters.Add(parameterValue);
                }
            }
            return parameters;
        }

        [RequiresUnreferencedCode("TryResolveParameter is incompatible with trimming")]
        protected virtual List<object> GetIoCParameterValues(Type type, ConstructorInfo selectedConstructor, object[] arguments)
        {
            var parameters = new List<object>();
            var unusedArguments = arguments.ToList();

            foreach (var parameterInfo in selectedConstructor.GetParameters())
            {
                var argumentMatch = unusedArguments.FirstOrDefault(arg => parameterInfo.ParameterType.IsInstanceOfType(arg));

                if (argumentMatch != null)
                {
                    parameters.Add(argumentMatch);
                    unusedArguments.Remove(argumentMatch);
                }
                else if (TryResolveParameter(type, parameterInfo, out var parameterValue))
                {
                    parameters.Add(parameterValue);
                }
            }
            return parameters;
        }

        [RequiresUnreferencedCode("TryResolveParameter is incompatible with trimming")]
        private bool TryResolveParameter(Type type, ParameterInfo parameterInfo, out object parameterValue)
        {
            if (!TryResolve(parameterInfo.ParameterType, out parameterValue))
            {
                if (parameterInfo.IsOptional)
                {
                    parameterValue = Type.Missing;
                }
                else
                {
                    throw new MvxIoCResolveException(
                        "Failed to resolve parameter for parameter {0} of type {1} when creating {2}. You may pass it as an argument",
                        parameterInfo.Name,
                        parameterInfo.ParameterType.Name,
                        type.FullName);
                }
            }

            return true;
        }
    }
}
