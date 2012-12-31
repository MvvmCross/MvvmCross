#region Copyright

// <copyright file="BaseDescription.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Collections.Generic;

namespace CrossUI.Core.Descriptions
{
    public class BaseDescription
    {
        public string OnlyFor { get; set; }
        public string NotFor { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public BaseDescription()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}