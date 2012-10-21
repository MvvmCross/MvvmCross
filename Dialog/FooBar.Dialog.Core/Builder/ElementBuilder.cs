using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;

namespace Foobar.Dialog.Core.Builder
{
    public abstract class ElementBuilder
    {
        public const string StandardConventionalEnding = "Element";
        public const string StandardDefaultElementKey = "String";
        public const string StandardCustomPropertyIndicator = "@";
        public const string StandardCustomPropertyTermination = ":";
        public const string StandardEscapedCustomPropertyIndicator = "@@";

        public string ConventionalEnding { get; set; }
        public string DefaultElementKey { get; set; }
        public string CustomPropertyIndicator { get; set; }
        public string CustomPropertyTermination { get; set; }
        public string EscapedCustomPropertyIndicator { get; set; }

        public IDictionary<string, Type> KnownElements { get; private set; }
        public IDictionary<string, IPropertySetter> CustomPropertySetters { get; private set; }

        protected ElementBuilder()
        {
            ConventionalEnding = StandardConventionalEnding;
            DefaultElementKey = StandardDefaultElementKey;
            CustomPropertyIndicator = StandardCustomPropertyIndicator;
            EscapedCustomPropertyIndicator = StandardEscapedCustomPropertyIndicator;
            CustomPropertyTermination = StandardCustomPropertyTermination;

            CustomPropertySetters = new Dictionary<string, IPropertySetter>();

            KnownElements = new Dictionary<string, Type>();
        }

        public void RegisterConventionalElements(Assembly assembly, string elementNamesEndWith = null)
        {
            elementNamesEndWith = elementNamesEndWith ?? ConventionalEnding;
            var elementTypes = assembly.GetTypes()
                            .Where(t => t.Name.EndsWith(elementNamesEndWith))
                            .Where(t => !t.IsAbstract)
                            .Where(t => typeof(IElement).IsAssignableFrom(t));

            foreach (var elementType in elementTypes)
            {
                var name = elementType.Name;
                if (name.EndsWith(elementNamesEndWith))
                    name = name.Substring(0, name.Length - elementNamesEndWith.Length);

                KnownElements[name] = elementType;
            }
        }

        public IElement Build(ElementDescription description)
        {
            if (description == null)
                return null;

            var key = string.IsNullOrEmpty(description.Key) ? DefaultElementKey : description.Key;
            Type type;
            if (!KnownElements.TryGetValue(key, out type))
            {
                throw new KeyNotFoundException("Could not find Element for " + description.Key);
            }

            var constructor = type.GetConstructors()
                                  .FirstOrDefault(c => c.GetParameters().All(p => p.IsOptional));
            if (constructor == null)
            {
                throw new ArgumentException("No parameterless Constructor found for " + key);
            }
            //var parameters = constructor.GetParameters().Select(p => (object)Type.Missing).ToArray();
            var parameters = constructor.GetParameters().Select(p => (object) p.DefaultValue).ToArray();
            var instance = constructor.Invoke(null, parameters);

            FillGroup(instance, description.Group);
            FillProperties(instance, description.Properties);
            FillSections(instance, description.Sections);

            return instance as IElement;
        }

        protected abstract ISection CreateNewSection(SectionDescription sectionDescription);
        protected abstract IGroup CreateNewGroup(GroupDescription groupDescription);

        private ISection Build(SectionDescription sectionDescription)
        {
            var section = CreateNewSection(sectionDescription);

            section.HeaderView = Build(sectionDescription.HeaderElement);
            section.FooterView = Build(sectionDescription.FooterElement);

            FillProperties(section, sectionDescription.Properties);
            FillElements(section, sectionDescription.Elements);

            return section;
        }

        private void FillGroup(object element, GroupDescription groupDescription)
        {
            if (groupDescription == null)
                return;

            var rootElement = element as IRootElement;
            if (rootElement == null)
            {
                throw new ArgumentException("You cannot set a group on an Element of type " + element.GetType().Name);
            }

            var group = CreateNewGroup(groupDescription);
            FillProperties(group, groupDescription.Properties);
            rootElement.Group = group;
        }

        private void FillProperties(object target, Dictionary<string, object> propertyDescriptions)
        {
            if (propertyDescriptions != null)
            {
                foreach (var kvp in propertyDescriptions)
                {
                    FillProperty(target, kvp.Key, kvp.Value);
                }
            }
        }

        private void FillProperty(object target, string targetPropertyName, object value)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                if (stringValue.StartsWith(EscapedCustomPropertyIndicator))
                {
                    value = stringValue.Substring(EscapedCustomPropertyIndicator.Length);
                }
                else if (stringValue.StartsWith(CustomPropertyIndicator))
                {
                    FillCustomProperty(target, targetPropertyName, stringValue);  
                    return;
                }
            }

            var property = target.GetType().GetProperty(targetPropertyName);

            if (property.PropertyType == typeof(Int32))
            {
                var t = value.GetType();
                if (t == typeof(Int64))
                {
                    value = (Int32)(Int64)value;
                }
            }
            property.GetSetMethod().Invoke(target, new object[] { value });
        }

        private void FillCustomProperty(object target, string targetPropertyName, string keyAndConfiguration)
        {
            string key;
            string configuration;
            SplitCustomPropertyConfiguration(keyAndConfiguration, out key, out configuration);

            IPropertySetter setter;
            if (!CustomPropertySetters.TryGetValue(key, out setter))
            {
                throw new KeyNotFoundException("Could not find property setter for " + key);
            }

            setter.Set(target, targetPropertyName, configuration);
        }

        private void SplitCustomPropertyConfiguration(string raw, out string key, out string configuration)
        {
            var indexOfTermination = raw.IndexOf(CustomPropertyTermination);
            if (indexOfTermination < 0)
            {
                key = raw.Substring(CustomPropertyIndicator.Length);
                configuration = string.Empty;
            }
            else
            {
                key = raw.Substring(CustomPropertyIndicator.Length, indexOfTermination - CustomPropertyIndicator.Length);
                configuration = raw.Substring(indexOfTermination + 1);
            }
        }

        private void FillElements(ISection section, IEnumerable<ElementDescription> elementDescriptions)
        {
            if (elementDescriptions != null)
            {
                foreach (var elementDescription in elementDescriptions)
                {
                    var element = Build(elementDescription);
                    if (element != null)
                        section.Add(element);
                    }
            }
        }

        private void FillSections(object instance, IEnumerable<SectionDescription> sectionDescriptions)
        {
            if (sectionDescriptions != null)
            {
                if (instance is IRootElement)
                {
                    var root = instance as IRootElement;
                    foreach (var sectionDescription in sectionDescriptions)
                    {
                        var section = Build(sectionDescription);
                        root.Add(section);
                    }
                }
                else if (sectionDescriptions.Any())
                {
                    throw new ArgumentException("Sections cannot be added to Element " + instance.GetType().Name);
                }
            }
        }
    }
}