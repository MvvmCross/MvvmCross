// KeyedUserInterfaceBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Descriptions;
using CrossUI.Core.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrossUI.Core.Builder
{
    public abstract class KeyedUserInterfaceBuilder : BaseUserInterfaceBuilder
    {
        private readonly IBuilderRegistry _builderRegistry;

        protected KeyedUserInterfaceBuilder(string platformName, IBuilderRegistry builderRegistry)
            : base(platformName)
        {
            _builderRegistry = builderRegistry;
        }

        public object Build(Type interfaceType, KeyedDescription description)
        {
            DialogTrace.WriteLine("Building {0} for {1}", interfaceType.Name, description.Key ?? "-empty-");
            if (!ShouldBuildDescription(description))
            {
                DialogTrace.WriteLine("Skipping - not for this platform");
                return null;
            }

            TypedUserInterfaceBuilder typeBuilder;
            if (!_builderRegistry.TryGetValue(interfaceType, out typeBuilder))
            {
                DialogTrace.WriteLine("No builder found for that {0}", interfaceType.Name);
                return null;
            }

            var userInterfaceInstance = typeBuilder.Build(description);
            if (userInterfaceInstance == null)
            {
                DialogTrace.WriteLine("Builder returned NULL for {0}", interfaceType.Name);
                return null;
            }

            FillProperties(userInterfaceInstance, description.Properties);

            FillBuildableProperties(description, userInterfaceInstance);

            return userInterfaceInstance;
        }

        private void FillBuildableProperties(KeyedDescription description, object userInterfaceInstance)
        {
            var reservedPropertyNames = new[] { "Key", "Properties", "NotFor", "OnlyFor" };

            var buildableProperties = from p in description.GetType().GetProperties()
                                      where !reservedPropertyNames.Contains(p.Name)
                                      select p;

            foreach (var buildablePropertyInfo in buildableProperties)
            {
                FillBuildableProperty(description, userInterfaceInstance, buildablePropertyInfo);
            }
        }

        private void FillBuildableProperty(KeyedDescription description, object userInterfaceInstance,
                                           PropertyInfo buildablePropertyInfo)
        {
            var buildablePropertyValue = buildablePropertyInfo.GetValue(description, null);

            var userInterfacePropertyInfo =
                userInterfaceInstance.GetType().GetProperty(buildablePropertyInfo.Name);
            if (userInterfacePropertyInfo == null)
            {
                var props = userInterfaceInstance.GetType().GetProperties().Select(p => p.Name);
                var available = string.Join("'", props);
                DialogTrace.WriteLine("No User Interface member for description property {0} on {1}",
                    buildablePropertyInfo.Name, available);
                return;
            }

            if (buildablePropertyInfo.PropertyType.GetTypeInfo().IsGenericType)
            {
                var genericPropertyType = buildablePropertyInfo.PropertyType.GetGenericTypeDefinition();

                if (genericPropertyType == typeof(Dictionary<int, int>).GetGenericTypeDefinition())
                {
                    DialogTrace.WriteLine("Filling Dictionary {0}", buildablePropertyInfo.Name);
                    FillDictionary(buildablePropertyInfo, buildablePropertyValue, userInterfacePropertyInfo,
                                   userInterfaceInstance);
                }
                else if (genericPropertyType == typeof(List<int>).GetGenericTypeDefinition())
                {
                    DialogTrace.WriteLine("Filling List {0}", buildablePropertyInfo.Name);
                    FillList(buildablePropertyInfo, buildablePropertyValue, userInterfacePropertyInfo,
                             userInterfaceInstance);
                }
                else
                {
                    throw new Exception("Unexpected Generic Property Type " + buildablePropertyInfo.PropertyType);
                }
            }
            else
            {
                DialogTrace.WriteLine("Filling Element {0}", buildablePropertyInfo.Name);
                FillUserInterfaceElement(buildablePropertyInfo, buildablePropertyValue, userInterfacePropertyInfo,
                                         userInterfaceInstance);
            }
        }

        private void FillUserInterfaceElement(PropertyInfo descriptionPropertyInfo, object descriptionPropertyValue,
                                              PropertyInfo userInterfacePropertyInfo, object userInterfaceInstance)
        {
            if (descriptionPropertyValue == null)
            {
                userInterfacePropertyInfo.SetValue(userInterfaceInstance, null, null);
                return;
            }

            var descriptionValueType = descriptionPropertyInfo.PropertyType;
            if (!typeof(KeyedDescription).IsAssignableFrom(descriptionValueType))
                throw new Exception("Don't know what to do with description property " + descriptionValueType);

            var userInterfaceValueType = userInterfacePropertyInfo.PropertyType;
            if (!typeof(IBuildable).IsAssignableFrom(userInterfaceValueType))
                throw new Exception("Don't know what to do with user interface property " + userInterfaceValueType);

            var builtUserInterfaceElement = Build(userInterfaceValueType, (KeyedDescription)descriptionPropertyValue);
            if (builtUserInterfaceElement == null)
            {
                throw new Exception("Failed to build user interface instance of type " + userInterfaceValueType.Name +
                                    " for property " + descriptionPropertyInfo.Name);
            }

            userInterfacePropertyInfo.SetValue(userInterfaceInstance, builtUserInterfaceElement, null);
        }

        private void FillDictionary(
            PropertyInfo descriptionPropertyInfo,
            object descriptionPropertyValue,
            PropertyInfo userInterfacePropertyInfo,
            object userInterfaceInstance)
        {
            var descriptionValueType = CheckDictionaryAndGetValueType(
                descriptionPropertyInfo,
                typeof(string),
                typeof(KeyedDescription));

            var userInterfaceValueType = CheckDictionaryAndGetValueType(
                userInterfacePropertyInfo,
                typeof(string),
                typeof(IBuildable));

            var descriptionDictionary = descriptionPropertyValue as IDictionary;
            if (descriptionDictionary == null)
            {
                // nothing to do - the description is empty
                return;
            }

            var instanceDictionary = (IDictionary)userInterfacePropertyInfo.GetValue(userInterfaceInstance, null);
            if (instanceDictionary == null)
            {
                throw new Exception("The UserInterfaceElement must be constructed with a valid Dictionary for " +
                                    userInterfacePropertyInfo.Name);
            }

            foreach (string key in descriptionDictionary.Keys)
            {
                var value = (KeyedDescription)descriptionDictionary[key];
                if (value == null)
                {
                    throw new Exception("Missing description for " + key);
                }

                // now we can finally do the actual build of items...
                var builtUserInterfaceElement = Build(userInterfaceValueType, value);

                // so now need to insert the value into the target...
                if (builtUserInterfaceElement == null)
                {
                    throw new Exception("Failed to build " + key);
                }

                FixParent(builtUserInterfaceElement, userInterfaceInstance);

                instanceDictionary[key] = builtUserInterfaceElement;
            }
        }

        private void FillList(
            PropertyInfo descriptionPropertyInfo,
            object descriptionPropertyValue,
            PropertyInfo userInterfacePropertyInfo,
            object userInterfaceInstance)
        {
            var descriptionValueType = CheckListAndGetValueType(
                descriptionPropertyInfo,
                typeof(KeyedDescription));

            var userInterfaceValueType = CheckListAndGetValueType(
                userInterfacePropertyInfo,
                typeof(IBuildable));

            if (descriptionPropertyValue == null)
            {
                // nothing to do - the description is empty
                return;
            }

            var descriptionList = descriptionPropertyValue as IList;
            if (descriptionList == null)
            {
                throw new Exception("Incoming description was not an IList");
            }

            var rawUserInstanceList = userInterfacePropertyInfo.GetValue(userInterfaceInstance, null);
            if (rawUserInstanceList == null)
            {
                throw new Exception("The UserInterfaceElement must be constructed with a valid List for " +
                                    userInterfacePropertyInfo.Name);
            }

            var instanceList = rawUserInstanceList as IList;
            if (instanceList == null)
            {
                throw new Exception("The UserInterfaceElement must be constructed with a valid IList-supporting value for " +
                                    userInterfacePropertyInfo.Name);
            }

            foreach (KeyedDescription description in descriptionList)
            {
                // now we can finally do the actual build of items...
                var builtUserInterfaceElement = Build(userInterfaceValueType, description);

                // so now need to insert the value into the target...
                if (builtUserInterfaceElement == null)
                {
                    throw new Exception("Failed to build description " + description.Key + " as " +
                                        userInterfaceValueType.Name);
                }

                FixParent(builtUserInterfaceElement, userInterfaceInstance);

                try
                {
                    instanceList.Add(builtUserInterfaceElement);
                }
                catch (Exception e)
                {
                    DialogTrace.WriteLine("Problem adding to list {0} {1}", e.GetType().Name, e.Message);
                }
            }
        }

        private static void FixParent(object child, object parent)
        {
            var parentProperty = child.GetType().GetProperty("Parent");
            if (parentProperty == null)
            {
                // nothing to set - so skip this
                return;
            }

            parentProperty.SetValue(child, parent, null);
        }

        private static Type CheckDictionaryAndGetValueType(PropertyInfo propertyInfo, Type expectedKeyType,
                                                           Type expectedValueBaseType)
        {
            var genericPropertyType = propertyInfo.PropertyType.GetGenericTypeDefinition();
            if (genericPropertyType != typeof(Dictionary<int, int>).GetGenericTypeDefinition())
            {
                throw new Exception("The property is not a generic Dictionary");
            }

            var genericTypes = propertyInfo.PropertyType.GetGenericArguments();
            if (genericTypes.Length != 2)
            {
                throw new Exception("We have a generic Dictionary with <>2 type arguments");
            }

            var keyType = genericTypes[0];
            if (keyType != expectedKeyType)
            {
                throw new Exception("ValueType mismatch in Dict. " + keyType + " is not " + expectedKeyType.Name);
            }

            var valueType = genericTypes[1];
            if (!expectedValueBaseType.IsAssignableFrom(valueType))
            {
                throw new Exception("ValueType mismatch in Dict. " + valueType.Name + " not assignable to " +
                                    expectedValueBaseType.Name);
            }

            return valueType;
        }

        private static Type CheckListAndGetValueType(PropertyInfo propertyInfo, Type expectedValueBaseType)
        {
            var genericPropertyType = propertyInfo.PropertyType.GetGenericTypeDefinition();

            if (genericPropertyType == null)
            {
                throw new Exception("The property is not a generic <T> class - this is needed for us to work out the generic type");
            }

            // note: this check removed after ObservableCollection and IList<T> changes
            //      for full info see https://github.com/slodge/MvvmCross/pull/294
            //if (!typeof(IList<>).IsAssignableFrom(genericPropertyType))
            //{
            //    throw new Exception("The property is a generic ICollection<T> but does not implement IList - property type : " + propertyInfo.PropertyType.Name);
            //}

            var genericTypes = propertyInfo.PropertyType.GetGenericArguments();
            if (genericTypes.Length != 1)
            {
                throw new Exception("We have a generic List with <>1 type arguments");
            }

            var valueType = genericTypes[0];
            if (!expectedValueBaseType.IsAssignableFrom(valueType))
            {
                throw new Exception("ValueType mismatch in List. " + valueType.Name + " not assignable to " +
                                    expectedValueBaseType.Name);
            }

            return valueType;
        }
    }
}