// PropertyBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Generic;

namespace CrossUI.Core.Builder
{
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

            if (property == null)
            {
                DialogTrace.WriteLine("property {0} is not available on the receiver, skipping it", targetPropertyName);
                return;
            }

            if (property.PropertyType == typeof(Int32))
            {
                var t = value.GetType();
                if (t == typeof(Int64))
                {
                    value = (Int32)(Int64)value;
                }
            }
            else if (property.PropertyType == typeof(Dictionary<string, string>))
            {
                value = FlattenToStringDictionary(value);
            }
            property.GetSetMethod().Invoke(target, new[] { value });
        }

        private Dictionary<string, string> FlattenToStringDictionary(object input)
        {
            var toReturn = new Dictionary<string, string>();

            var tenum = input as IEnumerable;
            if (tenum != null)
            {
                foreach (var item in tenum)
                {
                    if (item == null)
                    {
                        throw new Exception("How the heck is item null?");
                    }
                    var keyProperty = item.GetType().GetProperty("Key");
                    var valueProperty = item.GetType().GetProperty("Value");
                    if (keyProperty == null)
                    {
                        keyProperty = item.GetType().GetProperty("Name");
                    }
                    if (keyProperty == null)
                    {
                        throw new Exception("No key or name property in " + item.GetType().Name);
                    }
                    if (valueProperty == null)
                    {
                        throw new Exception("No value property in " + item.GetType().Name);
                    }
                    var key = keyProperty.GetValue(item, null);
                    var value = valueProperty.GetValue(item, null);
                    if (key != null)
                    {
                        toReturn[key.ToString()] = value == null ? null : value.ToString();
                    }
                }
            }

            return toReturn;
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
}