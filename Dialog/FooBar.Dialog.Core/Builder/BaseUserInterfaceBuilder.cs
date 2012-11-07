using System;
using System.Collections.Generic;
using System.Linq;
using Foobar.Dialog.Core.Descriptions;

namespace Foobar.Dialog.Core.Builder
{
    public interface IPropertyBuilder
    {
        string CustomPropertyIndicator { get; set; }
        string CustomPropertyTermination { get; set; }
        string EscapedCustomPropertyIndicator { get; set; }
        IDictionary<string, IPropertySetter> CustomPropertySetters { get; }
        void FillProperties(object target, Dictionary<string, object> propertyDescriptions);
        void FillProperty(object target, string targetPropertyName, object value);
        void FillCustomProperty(object target, string targetPropertyName, string keyAndConfiguration);
    }

    public class PropertyBuilder : IPropertyBuilder
    {
        public const string StandardCustomPropertyIndicator = "@";
        public const string StandardCustomPropertyTermination = ":";
        public const string StandardEscapedCustomPropertyIndicator = "@@";

        public string CustomPropertyIndicator { get; set; }
        public string CustomPropertyTermination { get; set; }
        public string EscapedCustomPropertyIndicator { get; set; }

        public IDictionary<string, IPropertySetter> CustomPropertySetters { get; private set; }

        public PropertyBuilder()
        {
            CustomPropertyIndicator = StandardCustomPropertyIndicator;
            EscapedCustomPropertyIndicator = StandardEscapedCustomPropertyIndicator;
            CustomPropertyTermination = StandardCustomPropertyTermination;

            CustomPropertySetters = new Dictionary<string, IPropertySetter>();
        }

        public virtual void FillProperties(object target, Dictionary<string, object> propertyDescriptions)
        {
            if (propertyDescriptions != null)
            {
                foreach (var kvp in propertyDescriptions)
                {
                    FillProperty(target, kvp.Key, kvp.Value);
                }
            }
        }

        public virtual void FillProperty(object target, string targetPropertyName, object value)
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

        public virtual void FillCustomProperty(object target, string targetPropertyName, string keyAndConfiguration)
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
    }

    public abstract class BaseUserInterfaceBuilder
    {
        private readonly Dictionary<string, bool> _platformTags;

        protected abstract IPropertyBuilder PropertyBuilder { get;  }

        protected BaseUserInterfaceBuilder(string platformName)
        {
            _platformTags = new Dictionary<string, bool>();
            AddPlatformName(platformName);
        }

        public void AddPlatformName(string tag)
        {
            _platformTags[tag] = true;
        }

        protected virtual void FillProperties(object target, Dictionary<string, object> propertyDescriptions)
        {
            PropertyBuilder.FillProperties(target, propertyDescriptions);
        }

        protected virtual bool CheckDescription(BaseDescription description)
        {
            if (description == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(description.NotFor))
            {
                var notFor = description.NotFor.Split(';');
                if (notFor.Any(_platformTags.ContainsKey))
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(description.OnlyFor))
            {
                var onlyFor = description.OnlyFor.Split(';');
                if (onlyFor.Any(_platformTags.ContainsKey))
                {
                    return true;
                }

                return false;
            }

            return true;
        }
    }
}