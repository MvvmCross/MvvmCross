// MvxPropertyInjectingIoCContainer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
    public class MvxPropertyInjectingIoCContainer
        : MvxSimpleIoCContainer
    {
        public static new IMvxIoCProvider Initialize()
        {
            if (Instance != null)
            {
                return Instance;
            }

            // create a new ioc container - it will register itself as the singleton
            new MvxPropertyInjectingIoCContainer();
            return Instance;
        }

        public override object IoCConstruct(Type type)
        {
            var toReturn = base.IoCConstruct(type);
            InjectProperties(type, toReturn);
            return toReturn;
        }

        private void InjectProperties(Type type, object toReturn)
        {
            var injectableProperties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.PropertyType.IsInterface)
                .Where(p => p.IsConventional())
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
    }
}