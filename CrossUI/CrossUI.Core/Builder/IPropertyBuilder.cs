// IPropertyBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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