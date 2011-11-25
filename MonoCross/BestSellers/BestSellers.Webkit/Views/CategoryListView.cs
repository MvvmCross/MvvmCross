using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using MonoCross.Navigation;
using MonoCross.Webkit;

namespace BestSellers.Webkit.Views
{
    public class CategoryListView : MXWebkitView<CategoryList>
    {
        public override void Render()
        {
            //HtmlGenericControl button = new HtmlGenericControl("a");
            //button.Attributes.Add("href", "Customers/NEW");
            //button.Attributes.Add("rel", "action");
            //button.Attributes.Add("rev", "async");
            //button.Attributes.Add("class", "iButton iBClassic");

            //HtmlGenericControl image = new HtmlGenericControl("img");
            //image.Attributes.Add("src", "../../WebApp/Img/more.png");

            HtmlGenericControl list = new HtmlGenericControl("div");
            list.Attributes.Add("class", "iMenu");
            list.Controls.Add(new HtmlGenericControl("h3") { InnerText = "The New York Times Best Sellers" });

            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "iArrow");
            ul.Attributes.Add("style", "background-color: #FFFFFF; color: #000000");

            foreach (Category category in Model)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("href", string.Format("{0}", category.ListNameEncoded));
                a.Attributes.Add("rev", "async");
                HtmlGenericControl img = new HtmlGenericControl("img");
                img.Attributes.Add("src", "NytIcon.png");
                img.Attributes.Add("style", "max-height:44px;max-width:32px");
                HtmlGenericControl em = new HtmlGenericControl("em");
                em.InnerText = category.ListName;
                //HtmlGenericControl small = new HtmlGenericControl("small");
                //small.Attributes.Add("style", "color:#666666");
                //small.InnerText = category.Website;
                a.Controls.Add(img);
                a.Controls.Add(em);
                li.Controls.Add(a);
                ul.Controls.Add(li);
            }
            list.Controls.Add(ul);
            WriteToResponse("CategoryList", "Categories", list);
        }
    }
}