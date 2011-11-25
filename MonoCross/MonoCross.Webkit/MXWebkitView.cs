using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO;

using MonoCross.Navigation;

namespace MonoCross.Webkit
{
    public class MXWebkitView<T>: IMXView
    {
        public virtual void Render() { }

        public T Model 
        {
            get { return (T)HttpContext.Current.Session[this.GetType().ToString()]; }
            set { HttpContext.Current.Session[this.GetType().ToString()] = value; }
        }
        public Type ModelType { get { return typeof(T); } }
        public void SetModel(object model)
        {
            Model = (T)model;
        }

        public static void WriteAjaxToResponse(string viewId, string viewTitle, Control control)
        {
            string xml = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Ajax.xml"));
            xml = xml.Replace("{ViewId}", viewId);
            xml = xml.Replace("{Title}", viewTitle);
            using (System.IO.StringWriter strwriter = new System.IO.StringWriter())
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(strwriter))
                {
                    control.RenderControl(writer);
                    xml = xml.Replace("{Markup}", strwriter.ToString());
                }
            }
            HttpContext.Current.Response.ContentType = "application/xml";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Output.Write(xml);
        }
        public static void WriteToResponse(string viewId, string viewTitle, Control control)
        {
            string html = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Root.html"));
            html = html.Replace("{AppTitle}", MXContainer.Instance.App.Title);
            html = html.Replace("{ViewId}", viewId);
            html = html.Replace("{ViewTitle}", viewTitle);
            using (System.IO.StringWriter strwriter = new System.IO.StringWriter())
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(strwriter))
                {
                    control.RenderControl(writer);
                    html = html.Replace("{Markup}", strwriter.ToString());
                }
            }
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Output.Write(html);
        }
        public static void WriteToResponse(string viewId, string viewTitle, Control[] controls)
        {
            string html = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Root.html"));
            html = html.Replace("{AppTitle}", MXContainer.Instance.App.Title);
            html = html.Replace("{ViewId}", viewId);
            html = html.Replace("{ViewTitle}", viewTitle);
            using (System.IO.StringWriter strwriter = new System.IO.StringWriter())
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(strwriter))
                {
                    foreach (Control control in controls)
                    {
                        control.RenderControl(writer);
                    }
                    html = html.Replace("{Markup}", strwriter.ToString());
                }
            }
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Output.Write(html);
        }
    }
}
