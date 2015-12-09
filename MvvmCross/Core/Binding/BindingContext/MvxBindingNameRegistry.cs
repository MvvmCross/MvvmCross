// MvxBindingNameRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using MvvmCross.Platform;

    public class MvxBindingNameRegistry
        : IMvxBindingNameLookup
          , IMvxBindingNameRegistry
    {
        private readonly Dictionary<Type, string> _lookup = new Dictionary<Type, string>();

        public string DefaultFor(Type type)
        {
            string toReturn;
            this.TryDefaultFor(type, out toReturn, true);
            return toReturn;
        }

        private bool TryDefaultFor(Type type, out string toReturn, bool includeInterfaces = true)
        {
            if (type == typeof(Object))
            {
                toReturn = null;
                return false;
            }

            if (this._lookup.TryGetValue(type, out toReturn))
                return true;

            if (type.IsConstructedGenericType)
            {
                var openType = type.GetGenericTypeDefinition();
                if (openType != null && this._lookup.TryGetValue(openType, out toReturn))
                    return true;
            }

            if (type.GetTypeInfo().IsInterface)
                return false;

            if (includeInterfaces)
            {
                var interfaces = type.GetInterfaces();
                foreach (var iface in interfaces)
                {
                    if (this.TryDefaultFor(iface, out toReturn, false))
                        return true;
                }
            }

            return this.TryDefaultFor(type.GetTypeInfo().BaseType, out toReturn, false);
        }

        public void AddOrOverwrite(Type type, string name)
        {
            this._lookup[type] = name;
        }

        public void AddOrOverwrite<T>(Expression<Func<T, object>> nameExpression)
        {
            var path = MvxBindingSingletonCache.Instance.PropertyExpressionParser.Parse(nameExpression);
            this._lookup[typeof(T)] = path.Print();
        }
    }
}