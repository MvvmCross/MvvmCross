#region Copyright

// <copyright file="BaseAuto.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Collections.Generic;
using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Auto
{
    public class BaseAuto
    {
        public string OnlyFor { get; set; }
        public string NotFor { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public BaseAuto(string onlyFor = null, string notFor = null)
        {
            OnlyFor = onlyFor;
            NotFor = notFor;
            Properties = new Dictionary<string, object>();
        }

        protected void Fill(BaseDescription baseDescription)
        {
            foreach (var kvp in Properties)
            {
                baseDescription.Properties[kvp.Key] = kvp.Value;
            }
            baseDescription.OnlyFor = OnlyFor;
            baseDescription.NotFor = NotFor;
        }
    }
}