using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Webkit.Dialog
{
    public abstract class Element
    {
        public abstract Control Control { get; }
        public string Caption { get; private set; }

        public Element(string caption)
        {
            Caption = caption;
        }
    }

    public class RootElement : Element, IEnumerable, IEnumerable<Section>
    {
        List<Section> sections = new List<Section>();

        public string ID { get; private set; }
        public string Action { get; set; }
        public bool Async { get; set; }
        public NavigationButton Submit { get; set; }
        public NavigationButton Cancel { get; private set; }
        

        public RootElement(string caption, string formId, string url, string action, bool async) : base(caption) 
        {
            ID = formId;
            Action = action;
            Async = async;
            Cancel = new NavigationButton("Cancel", "#", "WA.Back(); return false;");
            Submit = new NavigationButton("Save", url, Async ? string.Format("WA.Submit(\"{0}\"); return false;", ID) : string.Format("document.{0}.submit(); return false;", ID));
        }

        public override Control Control
        {
            get 
            {
                HtmlGenericControl root = new HtmlGenericControl("form");
                HtmlGenericControl panel = new HtmlGenericControl("div");

                sections.ForEach(i => panel.Controls.Add(i.Control));
                panel.Attributes.Add("class", "iPanel");

                root.Attributes.Add("id", ID);
                root.Attributes.Add("name", ID);
                root.Attributes.Add("onsubmit", "return false;");
                root.Attributes.Add("action", Action);
                root.Attributes.Add("method", "post");

                root.Controls.Add(panel);
                return root; 
            } 
        }
        public void Add(Section section)
        {
            if (section != null)
                sections.Add(section);
        }
        public int Add(IEnumerable<Element> sections)
        {
            int count = 0;
            foreach (Section s in sections)
            {
                Add(s);
                count++;
            }
            return count;
        }

        public string GetMarkup()
        {
            using (System.IO.StringWriter strwriter = new System.IO.StringWriter())
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(strwriter))
                {
                    Control.RenderControl(writer);
                    return strwriter.ToString();
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var s in sections)
                yield return s;
        }

        IEnumerator<Section> IEnumerable<Section>.GetEnumerator()
        {
            foreach (var s in sections)
                yield return s;
        }
    }
}
