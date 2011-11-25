using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using MonoCross.Navigation;
using MonoCross.Webkit;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Webkit.Views
{
    public class CustomerListView : MXWebkitView<List<Customer>>
    {
        public override void Render()
        {
            HtmlGenericControl button = new HtmlGenericControl("a");
            button.Attributes.Add("href", "Customers/NEW");
            button.Attributes.Add("rel", "action");
            button.Attributes.Add("rev", "async");
            button.Attributes.Add("class", "iButton iBClassic");

            HtmlGenericControl image = new HtmlGenericControl("img");
            image.Attributes.Add("src", "../../WebApp/Img/more.png");

            HtmlGenericControl list = new HtmlGenericControl("div");
            list.Attributes.Add("class", "iList");

            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "iArrow");
            ul.Attributes.Add("style", "background-color: #FFFFFF; color: #000000");

            foreach (CustomerManagement.Shared.Model.Customer customer in Model)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("href", string.Format("/Customers/{0}", customer.ID));
                a.Attributes.Add("rev", "async");
                HtmlGenericControl em = new HtmlGenericControl("em");
                em.InnerText = customer.Name;
                HtmlGenericControl small = new HtmlGenericControl("small");
                small.Attributes.Add("style", "color:#666666");
                small.InnerText = customer.Website;
                a.Controls.Add(em);
                a.Controls.Add(small);
                li.Controls.Add(a);
                ul.Controls.Add(li);
            }
            button.Controls.Add(image);
            list.Controls.Add(ul);
            WriteToResponse("CustomerList", "Customers", new Control[] { button, list }); 
        }
    }
}