// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.IoC
{
    public class MvxPropertyInjector : IMvxPropertyInjector
    {
        public virtual void Inject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TTarget>(
            TTarget target, IMvxPropertyInjectorOptions options = null)
        {
            options ??= MvxPropertyInjectorOptions.All;

            if (options.InjectIntoProperties == MvxPropertyInjection.None)
                return;

            ArgumentNullException.ThrowIfNull(target);

            var injectableProperties = FindInjectableProperties(target.GetType(), options);

            foreach (var injectableProperty in injectableProperties)
            {
                InjectProperty(target, injectableProperty, options);
            }
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "The Inject method already has proper DynamicallyAccessedMembers annotations")]
        protected virtual void InjectProperty(object toReturn, PropertyInfo injectableProperty, IMvxPropertyInjectorOptions options)
        {
            object propertyValue;

            if (Mvx.IoCProvider?.TryResolve(injectableProperty.PropertyType, out propertyValue) == true)
            {
                try
                {
                    injectableProperty.SetValue(toReturn, propertyValue, null);
                }
                catch (TargetInvocationException invocation)
                {
                    throw new MvxIoCResolveException(invocation, "Failed to inject into {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
            }
            else
            {
                if (options.ThrowIfPropertyInjectionFails)
                {
                    throw new MvxIoCResolveException("IoC property injection failed for {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
                else
                {
                    MvxLogHost.Default?.Log(LogLevel.Warning,
                        "IoC property injection skipped for {PropertyName} on {TypeName}",
                        injectableProperty.Name, toReturn.GetType().Name);
                }
            }
        }

        protected virtual IEnumerable<PropertyInfo> FindInjectableProperties(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type type, IMvxPropertyInjectorOptions options)
        {
            var injectableProperties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.PropertyType.GetTypeInfo().IsInterface)
                .Where(p => p.IsConventional())
                .Where(p => p.CanWrite);

            switch (options.InjectIntoProperties)
            {
                case MvxPropertyInjection.MvxInjectInterfaceProperties:
                    injectableProperties = injectableProperties
                        .Where(p => p.GetCustomAttributes(typeof(MvxInjectAttribute), false).Any());
                    break;

                case MvxPropertyInjection.AllInterfaceProperties:
                    break;

                case MvxPropertyInjection.None:
                    MvxLogHost.Default?.Log(LogLevel.Error, "Internal error - should not call FindInjectableProperties with MvxPropertyInjection.None");
                    injectableProperties = [];
                    break;

                default:
                    throw new MvxException("unknown option for InjectIntoProperties {0}", options.InjectIntoProperties);
            }
            return injectableProperties;
        }
    }
}
