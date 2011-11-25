using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Webkit.Dialog
{
    public class TextElement : Element
    {
        public string ID { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }

        public TextElement(string caption, string value, string property) : base(caption)
        {
            ID = property; // add session or hash code to id
            Label = caption;
            Value = value;
        }

        //public TextElement(string caption, string value, string fieldId)
        //    : base(property)
        //{
        //    ID = string.Format("{0}_{1}.{2}", HttpContext.Current.Session.SessionID, fieldId, property); // add session or hash code to id
        //    Placeholder = property;
        //    Value = value;
        //}
        //public static TextElement<T> Bind(T model, string property)
        //{ 
        //    return new TextElement<T>(property, typeof(T)
        //        .GetProperty(property)
        //        .GetValue(model, null) != null ? typeof(T)
        //        .GetProperty(property)
        //        .GetValue(model, null).ToString() : string.Empty, 
        //        string.Format("{0}_{1}", model.GetHashCode().ToString(), property));
        //}
        public override Control Control
        {
            get 
            {
                HtmlGenericControl item = new HtmlGenericControl("li");
                HtmlGenericControl label = new HtmlGenericControl("label");
                HtmlGenericControl input = new HtmlGenericControl("input");
                input.Attributes.Add("type", "text");
                input.Attributes.Add("name", ID);
                label.InnerText = Label;
                if (!string.IsNullOrWhiteSpace(Value)) input.Attributes.Add("value", Value);
                item.Controls.Add(label);
                item.Controls.Add(input);
                return item;
            }
        }
    }
}
