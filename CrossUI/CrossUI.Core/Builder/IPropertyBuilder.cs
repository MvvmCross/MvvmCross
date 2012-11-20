using System.Collections.Generic;

namespace CrossUI.Core.Builder
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
}