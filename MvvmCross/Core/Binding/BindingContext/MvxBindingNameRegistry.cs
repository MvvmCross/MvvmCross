// MvxBindingNameRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using MvvmCross.Platform;

namespace MvvmCross.Binding.BindingContext
{
    public class MvxBindingNameRegistry
        : IMvxBindingNameLookup
            , IMvxBindingNameRegistry
    {
        private readonly Dictionary<Type, string> _lookup = new Dictionary<Type, string>();

        public string DefaultFor(Type type)
        {
            string toReturn;
            TryDefaultFor(type, out toReturn, true);
            return toReturn;
        }

        public void AddOrOverwrite(Type type, string name)
        {
            _lookup[type] = name;
        }

        public void AddOrOverwrite<T>(Expression<Func<T, object>> nameExpression)
        {
            var path = MvxBindingSingletonCache.Instance.PropertyExpressionParser.Parse(nameExpression);
            _lookup[typeof(T)] = path.Print();
        }

        private bool TryDefaultFor(Type type, out string toReturn, bool includeInterfaces = true)
        {
            if (type == typeof(object))
            {
                toReturn = null;
                return false;
            }

            if (_lookup.TryGetValue(type, out toReturn))
                return true;

            if (type.IsConstructedGenericType)
            {
                var openType = type.GetGenericTypeDefinition();
                if (openType != null && _lookup.TryGetValue(openType, out toReturn))
                    return true;
            }

            if (type.GetTypeInfo().IsInterface)
                return false;

            if (includeInterfaces)
            {
                var interfaces = type.GetInterfaces();
                foreach (var iface in interfaces)
                    if (TryDefaultFor(iface, out toReturn, false))
                        return true;
            }

            return TryDefaultFor(type.GetTypeInfo().BaseType, out toReturn, false);
        }
    }
}