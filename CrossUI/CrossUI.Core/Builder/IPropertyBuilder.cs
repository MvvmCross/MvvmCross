#region Copyright

// <copyright file="IPropertyBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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