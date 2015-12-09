// BaseAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto
{
    using System.Collections.Generic;

    using CrossUI.Core.Descriptions;

    public class BaseAuto
    {
        public string OnlyFor { get; set; }
        public string NotFor { get; set; }
        public Dictionary<string, object> Properties { get; set; }

        public BaseAuto(string onlyFor = null, string notFor = null)
        {
            this.OnlyFor = onlyFor;
            this.NotFor = notFor;
            this.Properties = new Dictionary<string, object>();
        }

        protected void Fill(BaseDescription baseDescription)
        {
            foreach (var kvp in this.Properties)
            {
                baseDescription.Properties[kvp.Key] = kvp.Value;
            }
            baseDescription.OnlyFor = this.OnlyFor;
            baseDescription.NotFor = this.NotFor;
        }
    }
}