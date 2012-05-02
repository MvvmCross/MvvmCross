using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.IoC
{
    public class MvxSimpleIoCContainer
    {
        public static readonly MvxSimpleIoCContainer Current = new MvxSimpleIoCContainer();

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
                if (!_resolvers.TryGetValue(typeof(T), out resolver))
                {
                    resolved = default(T);
                    return false;
                }

                resolved = (T) resolver.Resolve();
                return true;
            }
        }

        public T Resolve<T>()
            where T : class
        {
            lock (this)
            {
                return (T) _resolvers[typeof (T)].Resolve();
            }
        }

        public void RegisterServiceType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class
        {
            lock (this)
            {
                _resolvers[typeof(TInterface)] = new ConstructingResolver(typeof(TToConstruct));                
            }
        }

        public void RegisterServiceInstance<TInterface>(TInterface theObject)
            where TInterface : class
        {
            lock (this)
            {
                _resolvers[typeof(TInterface)] = new SingletonResolver(theObject);
            }
        }
    }
}