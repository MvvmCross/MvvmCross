using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Webkit.Dialog
{
    public class StringElement : Element
    {
        public string Value { get; set; }

        public StringElement(string caption, string value)
            : base(caption)
        {
            Value = value;
        }
        public override Control Control
        {
            get
            {
                HtmlGenericControl item = new HtmlGenericControl("li");
                HtmlGenericControl span = new HtmlGenericControl("span");
                span.InnerText = string.IsNullOrWhiteSpace(Value) ? "N/A" : Value;
                item.InnerText = Caption;
                item.Controls.Add(span);
                return item;
            }
        }
    }
}
