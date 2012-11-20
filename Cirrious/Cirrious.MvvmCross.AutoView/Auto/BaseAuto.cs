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