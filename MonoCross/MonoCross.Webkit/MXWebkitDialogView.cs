using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;

using MonoCross.Navigation;

using Webkit.Dialog;

namespace MonoCross.Webkit
{
    public class MXWebkitDialogView<T> : MXWebkitView<T>, IMXView
    {
        public RootElement Root { get; set; }

        public static void WriteRootToResponse(string viewId, string viewTitle, RootElement root)
        {
            string xml = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Ajax.xml"));
            xml = xml.Replace("{ViewId}", viewId);
            xml = xml.Replace("{Title}", viewTitle);
            using (System.IO.StringWriter strwriter = new System.IO.StringWriter())
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(strwriter))
                {
                    root.Submit.Control.RenderControl(writer);
                    root.Cancel.Control.RenderControl(writer);
                    root.Control.RenderControl(writer);
                    xml = xml.Replace("{Markup}", strwriter.ToString());
                }
            }
            HttpContext.Current.Response.ContentType = "application/xml";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Output.Write(xml);
        }
    }
}
