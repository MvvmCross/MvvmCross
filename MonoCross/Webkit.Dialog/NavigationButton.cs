using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Webkit.Dialog
{
    public class NavigationButton : Button
    {
        public NavigationButton(string caption, string url, string onclick) : base(caption, url)
        {
            this.OnClick = onclick;
        }

        public string Value { get; set; }
        public string OnClick { get; set; }

        public override Control Control
        {
            get
            {
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("href", Url);
                a.Attributes.Add("rel", "action");
                a.Attributes.Add("class", Style);
                a.Attributes.Add("onclick", OnClick);
                a.InnerText = Caption;
                return a;
            }
        }
    }
}
