using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Webkit.Dialog
{
    public class Section : Element, IEnumerable
    {
        List<Element> elements = new List<Element>();

        public Section(string caption) : base(caption) { }

        public override Control Control
        {
            get
            {
                HtmlGenericControl section = new HtmlGenericControl("fieldset");
                HtmlGenericControl legend = new HtmlGenericControl("legend");
                HtmlGenericControl list = new HtmlGenericControl("ul");
                legend.InnerText = Caption;
                section.Controls.Add(legend);
                section.Controls.Add(list);
                elements.ForEach(i => list.Controls.Add(i.Control));
                return section;
            }
        }
        public void Add(Element element)
        {
            if (element != null)
                elements.Add(element);
        }
        public int Add(IEnumerable<Element> elements)
        {
            int count = 0;
            foreach (Section e in elements)
            {
                Add(e);
                count++;
            }
            return count;
        }
        public IEnumerator GetEnumerator()
        {
            foreach (var e in elements)
                yield return e;
        }
    }
}
