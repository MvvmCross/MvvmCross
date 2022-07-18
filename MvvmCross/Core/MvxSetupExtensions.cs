// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.IoC;

namespace MvvmCross.Core
{
#nullable enable
    public static class MvxSetupExtensions
    {
        public static void RegisterSetupType<TMvxSetup>(this object platformApplication, params Assembly[] assemblies)
            where TMvxSetup : MvxSetup, new()
        {
            if (platformApplication == null)
                throw new ArgumentNullException(nameof(platformApplication));

            MvxSetup.RegisterSetupType<TMvxSetup>(
                new[] { platformApplication.GetType().Assembly }.Union(assemblies ?? Array.Empty<Assembly>()).ToArray());
        }

        public static TSetup CreateSetup<TSetup>(Assembly assembly, params object[] parameters) where TSetup : MvxSetup
        {
            var setupType = FindSetupType<TSetup>(assembly);
            if (setupType == null)
            {
                throw new MvxException("Could not find a Setup class for application");
            }

            try
            {
                return (TSetup)Activator.CreateInstance(setupType, parameters);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Failed to create instance of {0}", setupType.FullName);
            }
        }

        public static TSetup CreateSetup<TSetup>() where TSetup : MvxSetup
        {
            var setupType = FindSetupType<TSetup>();
            if (setupType == null)
            {
                throw new MvxException("Could not find a Setup class for application");
            }

            try
            {
                return (TSetup)Activator.CreateInstance(setupType);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Failed to create instance of {0}", setupType.FullName);
            }
        }

        public static Type? FindSetupType<TSetup>(Assembly assembly)
        {
            var query = from type in assembly.ExceptionSafeGetTypes()
                        where type.Name == "Setup"
                        where typeof(TSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }

        public static Type? FindSetupType<TSetup>()
        {
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.ExceptionSafeGetTypes()
                        where type.Name == "Setup"
                        where typeof(TSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }
    }
#nullable restore
}
