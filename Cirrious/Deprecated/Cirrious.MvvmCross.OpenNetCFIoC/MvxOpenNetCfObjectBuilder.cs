// MvxOpenNetCfObjectBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region Credit - OpenNetCf

// This file is based on the OpenNetCf IoC container - used under free license -see http://ioc.codeplex.com

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Exceptions;

#endregion

namespace Cirrious.MvvmCross.OpenNetCfIoC
{
    internal class MvxOpenNetCfObjectBuilder
    {
        private static readonly Dictionary<Type, InjectionConstructor> ConstructorCache =
            new Dictionary<Type, InjectionConstructor>();

        /// <summary>
        ///   Creates the object.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        internal static object CreateObject(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            object instance = null;

            // first check the cache
            if (ConstructorCache.ContainsKey(type))
                return CreateObjectFromCache(type);

#if NETFX_CORE
            if (type.GetTypeInfo().IsInterface)
#else
            if (type.IsInterface)
#endif
                throw new MvxException(string.Format("Cannot create an instance of an interface ({0}).", type.Name));


#if NETFX_CORE
            var ctors =
                (type.GetTypeInfo().DeclaredConstructors
                    .Where(
                        c =>
                        c.IsPublic && c.GetCustomAttributes(typeof (MvxOpenNetCfInjectionAttribute), true).Count() > 0));
#else
            var ctors =
                (type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                     .Where(
                         c =>
                         c.IsPublic && c.GetCustomAttributes(typeof (MvxOpenNetCfInjectionAttribute), true).Count() > 0));
#endif

            if (!ctors.Any())
            {
                // no injection ctor, get the default, parameterless ctor
#if NETFX_CORE
                var parameterlessCtors =
                    (type.GetTypeInfo().DeclaredConstructors.Where(c => c.IsPublic && c.GetParameters().Length == 0));
#else
                var parameterlessCtors =
                    (type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic |
                                          BindingFlags.Instance).Where(c => c.GetParameters().Length == 0));
#endif
                if (!parameterlessCtors.Any())
                {
                    throw new ArgumentException(
                        string.Format(
                            "Type '{0}' has no public parameterless constructor or injection constructor.\r\nAre you missing the InjectionConstructor attribute?",
                            type));
                }

                // create the object
                var constructorInfo = parameterlessCtors.First();
                try
                {
                    instance = constructorInfo.Invoke(null);
                    ConstructorCache.Add(type, new InjectionConstructor {ConstructorInfo = constructorInfo});
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
            else if (ctors.Count() == 1)
            {
                // call the injection ctor
                var constructorInfo = ctors.First();
                var parameterInfos = constructorInfo.GetParameters();
                var parameters = GetParameterObjectsForParameterList(parameterInfos, type.Name);
                try
                {
                    instance = constructorInfo.Invoke(parameters.ToArray());
                    ConstructorCache.Add(type,
                                         new InjectionConstructor
                                             {
                                                 ConstructorInfo = constructorInfo,
                                                 ParameterInfos = parameterInfos
                                             });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
            else
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' has {1} defined injection constructors.  Only one is allowed", type.Name,
                                  ctors.Count()));
            }

            //// Adrian Sudbury 31st Jan 2013 - this line is commented out in the original version!
            DoInjections(instance);

            return instance;
        }

        /// <summary>
        /// Does the injections.
        /// </summary>
        /// <param name="instance">The instance.</param>
        internal static void DoInjections(object instance)
        {
            var t = instance.GetType();


            // look for service dependecy setters
#if NETFX_CORE
            var serviceDependecyProperties = t.GetTypeInfo().DeclaredProperties
#else
            var serviceDependecyProperties = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance)
#endif
                                              .Where(p =>
                                                     p.GetCustomAttributes(
                                                         typeof (MvxOpenNetCfDependencyAttribute), true).
                                                       Count() > 0);

            foreach (var pi in serviceDependecyProperties)
            {
                var resolved = MvxOpenNetCfContainer.Current.GetResolvedType(pi.PropertyType);
                if (resolved != null)
                    pi.SetValue(instance, resolved, null);
                else
                    pi.SetValue(instance, CreateObject(pi.PropertyType), null);
            }
        }

        /// <summary>
        /// Creates the object from cache.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static object CreateObjectFromCache(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var injectionConstructor = ConstructorCache[type];

            try
            {
                if ((injectionConstructor.ParameterInfos == null) || (injectionConstructor.ParameterInfos.Length == 0))
                {
                    return injectionConstructor.ConstructorInfo.Invoke(null);
                }
                var parameters = GetParameterObjectsForParameterList(
                    injectionConstructor.ParameterInfos, type.Name);
                return injectionConstructor.ConstructorInfo.Invoke(parameters.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Gets the parameter objects for parameter list.
        /// </summary>
        /// <param name="parameterInfos">The parameter infos.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        private static IEnumerable<object> GetParameterObjectsForParameterList(
            IEnumerable<ParameterInfo> parameterInfos, string typeName)
        {
            if (parameterInfos == null)
                throw new ArgumentNullException("parameterInfos");
            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentNullException("typeName");

            var paramObjects = new List<object>();

            foreach (var pi in parameterInfos)
            {
#if NETFX_CORE
                if (pi.ParameterType.GetTypeInfo().IsValueType)
#else
                if (pi.ParameterType.IsValueType)
#endif
                {
                    throw new ArgumentException(
                        string.Format("Injection on type '{0}' cannot have value type parameters",
                                      typeName));
                }

                if (Equals(pi.ParameterType.FullName, typeof (MvxOpenNetCfContainer).FullName))
                {
                    paramObjects.Add(MvxOpenNetCfContainer.Current);
                    continue;
                }


                // see if there is an item that matches the type
                var itemList = MvxOpenNetCfContainer.Current.FindByType(pi.ParameterType).ToList();
                if (!itemList.Any())
                    itemList.Add(MvxOpenNetCfContainer.Current.GetResolvedType(pi.ParameterType));

                paramObjects.Add(itemList.First());
            }

            return paramObjects.ToArray();
        }

        #region Nested InjectionConstructor

        private struct InjectionConstructor
        {
            public ConstructorInfo ConstructorInfo { get; set; }
            public ParameterInfo[] ParameterInfos { get; set; }
        }

        #endregion
    }
}