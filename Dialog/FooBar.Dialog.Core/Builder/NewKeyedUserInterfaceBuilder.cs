using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Lists;

namespace Foobar.Dialog.Core.Builder
{
    public class TypedDiaogElementBuilder
    {
        public Type Type { get; private set; }
        public IDictionary<string, Type> KnownKeys { get; private set; }
        public string ConventionalEnding { get; set; }
        public string DefaultKey { get; set; }
        
    }

    public class TypedUserInterfaceBuilder
    {
        public Type Type { get; private set; }
        public IDictionary<string, Type> KnownKeys { get; private set; }
        public string ConventionalEnding { get; set; }
        public string DefaultKey { get; set; }

        public TypedUserInterfaceBuilder(Type type, string conventionalEnding, string defaultKey)
        {
            DefaultKey = defaultKey;
            ConventionalEnding = conventionalEnding;
            KnownKeys = new Dictionary<string, Type>();
        }

        public void RegisterConventionalKeys(Assembly assembly, string keyNamesEndWith = null)
        {
            keyNamesEndWith = keyNamesEndWith ?? ConventionalEnding;
            var elementTypes = assembly.GetTypes()
                .Where(t => t.Name.EndsWith(keyNamesEndWith))
                .Where(t => !t.IsAbstract)
                .Where(t => Type.IsAssignableFrom(t));

            foreach (var elementType in elementTypes)
            {
                var name = elementType.Name;
                if (name.EndsWith(keyNamesEndWith))
                    name = name.Substring(0, name.Length - keyNamesEndWith.Length);

                KnownKeys[name] = elementType;
            }
        }

        public object Build(KeyedDescription description)
        {
            var key = string.IsNullOrEmpty(description.Key) ? DefaultKey : description.Key;
            Type type;
            if (!KnownKeys.TryGetValue(key, out type))
            {
                throw new KeyNotFoundException("Could not find class for " + description.Key);
            }

            var constructor = type.GetConstructors()
                                  .FirstOrDefault(c => c.GetParameters().All(p => p.IsOptional));
            if (constructor == null)
            {
                throw new ArgumentException("No parameterless Constructor found for " + key);
            }
            //var parameters = constructor.GetParameters().Select(p => (object)Type.Missing).ToArray();
            var parameters = constructor.GetParameters().Select(p => (object)p.DefaultValue).ToArray();
            var instance = constructor.Invoke(null, parameters);

            return instance;
        }
    }

    public class NewKeyedUserInterfaceBuilder : BaseUserInterfaceBuilder
    {
        public Dictionary<Type, TypedUserInterfaceBuilder> Builders { get; private set; }

        protected NewKeyedUserInterfaceBuilder(string platformName)
            : base(platformName)
        {
        }

        public object Build(Type interfaceType, KeyedDescription description)
        {
#warning rename CheckDescription
            if (!CheckDescription(description))
            {
                return null;
            }

            TypedUserInterfaceBuilder typeBuilder;
            if (!Builders.TryGetValue(interfaceType, out typeBuilder))
            {
                return null;
            }

            var userInterfaceInstance = typeBuilder.Build(description);
            if (userInterfaceInstance == null)
            {
                return null;
            }

            FillProperties(userInterfaceInstance, description.Properties);

            FillBuildableProperties(description, userInterfaceInstance);

            return userInterfaceInstance;
        }

        private void FillBuildableProperties(KeyedDescription description, object userInterfaceInstance)
        {
            var reservedPropertyNames = new [] { "Key", "Properties", "NotFor", "OnlyFor" };

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
                userInterfaceInstance.GetType().GetProperty(buildablePropertyInfo.PropertyType.Name);
            if (userInterfacePropertyInfo == null)
            {
                throw new Exception("No User Interface member for description property " + buildablePropertyInfo.Name);
            }

            if (buildablePropertyInfo.PropertyType.IsGenericType)
            {
                var genericPropertyType = buildablePropertyInfo.PropertyType.GetGenericTypeDefinition();

                if (genericPropertyType == typeof (Dictionary<int, int>).GetGenericTypeDefinition())
                {
                    FillDictionary(buildablePropertyInfo, buildablePropertyValue, userInterfacePropertyInfo,
                                   userInterfaceInstance);
                }
                else if (genericPropertyType == typeof (List<int>).GetGenericTypeDefinition())
                {
                    FillList(buildablePropertyInfo, buildablePropertyValue, userInterfacePropertyInfo, userInterfaceInstance);
                }
                else
                {
                    throw new Exception("Unexpected Generic Property Type " + buildablePropertyInfo.PropertyType);
                }
            }
            else
            {
                FillUserInterfaceElement(buildablePropertyInfo, buildablePropertyValue, userInterfacePropertyInfo,
                                         userInterfaceInstance);
            }
        }

        private void FillUserInterfaceElement(PropertyInfo descriptionPropertyInfo, object descriptionPropertyValue, PropertyInfo userInterfacePropertyInfo, object userInterfaceInstance)
        {
            var descriptionValueType = descriptionPropertyInfo.PropertyType;
            if (!typeof(KeyedDescription).IsAssignableFrom(descriptionValueType))
                throw new Exception("Don't know what to do with description property " + descriptionValueType);

            var userInterfaceValueType = userInterfacePropertyInfo.PropertyType;
            if (!typeof(IBuildableUserInterfaceElement).IsAssignableFrom(userInterfaceValueType))
                throw new Exception("Don't know what to do with user interface property " + userInterfaceValueType);

            var builtUserInterfaceElement = Build(userInterfaceValueType, (KeyedDescription)descriptionPropertyValue);
            if (builtUserInterfaceElement == null)
            {
                throw new Exception("Failed to build user interface instance");
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
                                            typeof (string), 
                                            typeof (KeyedDescription));

            var userInterfaceValueType = CheckDictionaryAndGetValueType(
                                            descriptionPropertyInfo,
                                            typeof(string),
                                            typeof(IBuildableUserInterfaceElement));

            var descriptionDictionary = (IDictionary) descriptionPropertyValue;
            if (descriptionDictionary == null)
            {
                // nothing to do - the description is empty
                return;
            }

            var instanceDictionary = (IDictionary) userInterfacePropertyInfo.GetValue(userInterfaceInstance, null);
            if (instanceDictionary == null)
            {
                throw new Exception("The UserInterfaceElement must be constructed with a valid Dictionary for " + userInterfacePropertyInfo.Name);
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
                                            descriptionPropertyInfo,
                                            typeof(IBuildableUserInterfaceElement));

            var descriptionList = (IList)descriptionPropertyValue;
            if (descriptionList == null)
            {
                // nothing to do - the description is empty
                return;
            }

            var instanceList = (IList)userInterfacePropertyInfo.GetValue(userInterfaceInstance, null);
            if (instanceList == null)
            {
                throw new Exception("The UserInterfaceElement must be constructed with a valid Dictionary for " + userInterfacePropertyInfo.Name);
            }

            foreach (KeyedDescription description in descriptionList)
            {
                // now we can finally do the actual build of items...
                var builtUserInterfaceElement = Build(userInterfaceValueType, description);

                // so now need to insert the value into the target...
                if (builtUserInterfaceElement == null)
                {
                    throw new Exception("Failed to build description");
                }

                instanceList.Add(builtUserInterfaceElement);
            }
        }

        private static Type CheckDictionaryAndGetValueType(PropertyInfo propertyInfo, Type expectedKeyType, Type expectedValueBaseType)
        {
            var genericPropertyType = propertyInfo.PropertyType.GetGenericTypeDefinition();
            if (genericPropertyType != typeof(Dictionary<int, int>).GetGenericTypeDefinition())
            {
                throw new Exception("The property is not a generic Dictionary");
            }

            var genericTypes = genericPropertyType.GetGenericArguments();
            if (genericTypes.Length != 2)
            {
                throw new Exception("We have a generic Dictionary with <>2 type arguments");
            }

            var keyType = genericTypes[0];
            if (keyType != expectedKeyType)
            {
                throw new Exception("Key Type mismatch in generic Dictionary - type must be " + expectedKeyType.Name);
            }

            var valueType = genericTypes[1];
            if (!expectedValueBaseType.IsAssignableFrom(valueType))
            {
                throw new Exception("Value Type mismatch in generic Dictionary - type must be assignable to " + expectedValueBaseType.Name);
            }

            return valueType;
        }

        private static Type CheckListAndGetValueType(PropertyInfo propertyInfo, Type expectedValueBaseType)
        {
            var genericPropertyType = propertyInfo.PropertyType.GetGenericTypeDefinition();
            if (genericPropertyType != typeof(List<int>).GetGenericTypeDefinition())
            {
                throw new Exception("The property is not a generic List");
            }

            var genericTypes = genericPropertyType.GetGenericArguments();
            if (genericTypes.Length != 1)
            {
                throw new Exception("We have a generic List with <>1 type arguments");
            }

            var valueType = genericTypes[0];
            if (!expectedValueBaseType.IsAssignableFrom(valueType))
            {
                throw new Exception("Value Type mismatch in generic List - type must be assignable to " + expectedValueBaseType.Name);
            }

            return valueType;
        }
    }
}