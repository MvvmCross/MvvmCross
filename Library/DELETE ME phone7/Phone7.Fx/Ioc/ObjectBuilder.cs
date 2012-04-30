#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace Phone7.Fx.Ioc
{
    internal class ObjectBuilder
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

            if (type.IsInterface)
                throw new Exception(string.Format("Cannot create an instance of an interface ({0}).", type.Name));


            IEnumerable<ConstructorInfo> ctors =
                (type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(c => c.IsPublic && c.GetCustomAttributes(typeof(InjectionAttribute), true).Count() > 0));

            if (!ctors.Any())
            {
                // no injection ctor, get the default, parameterless ctor
                IEnumerable<ConstructorInfo> parameterlessCtors =
                    (type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic |
                                          BindingFlags.Instance).Where(c => c.GetParameters().Length == 0));
                if (!parameterlessCtors.Any())
                {
                    throw new ArgumentException(
                        string.Format(
                            "Type '{0}' has no public parameterless constructor or injection constructor.\r\nAre you missing the InjectionConstructor attribute?",
                            type));
                }

                // create the object
                ConstructorInfo constructorInfo = parameterlessCtors.First();
                try
                {
                    instance = constructorInfo.Invoke(null);
                    ConstructorCache.Add(type, new InjectionConstructor { ConstructorInfo = constructorInfo });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
            else if (ctors.Count() == 1)
            {
                // call the injection ctor
                ConstructorInfo constructorInfo = ctors.First();
                ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
                IEnumerable<object> parameters = GetParameterObjectsForParameterList(parameterInfos, type.Name);
                try
                {
                    instance = constructorInfo.Invoke(parameters.ToArray());
                    ConstructorCache.Add(type,
                                          new InjectionConstructor { ConstructorInfo = constructorInfo, ParameterInfos = parameterInfos });
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

            //DoInjections(instance);

            return instance;
        }

        /// <summary>
        /// Does the injections.
        /// </summary>
        /// <param name="instance">The instance.</param>
        internal static void DoInjections(object instance)
        {
            Type t = instance.GetType();


            // look for service dependecy setters
            var serviceDependecyProperties = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(
                    p => p.GetCustomAttributes(typeof(DependencyAttribute), true).Count() > 0);

            foreach (PropertyInfo pi in serviceDependecyProperties)
            {
                object resolved = Container.Current.GetResolvedType(pi.PropertyType);
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

            InjectionConstructor injectionConstructor = ConstructorCache[type];

            try
            {
                if ((injectionConstructor.ParameterInfos == null) || (injectionConstructor.ParameterInfos.Length == 0))
                {
                    return injectionConstructor.ConstructorInfo.Invoke(null);
                }
                IEnumerable<object> parameters = GetParameterObjectsForParameterList(
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

            List<object> paramObjects = new List<object>();

            foreach (var pi in parameterInfos)
            {
                if (pi.ParameterType.IsValueType)
                {
                    throw new ArgumentException(
                        string.Format("Injection on type '{0}' cannot have value type parameters",
                                      typeName));
                }

                if (Equals(pi.ParameterType.FullName, typeof(Container).FullName))
                {
                    paramObjects.Add(Container.Current);
                    continue;
                }


                // see if there is an item that matches the type
                List<object> itemList = Container.Current.FindByType(pi.ParameterType).ToList();
                if (!itemList.Any())
                    itemList.Add(Container.Current.GetResolvedType(pi.ParameterType));

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