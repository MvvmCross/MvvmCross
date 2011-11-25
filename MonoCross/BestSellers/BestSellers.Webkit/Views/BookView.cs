using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using MonoCross.Navigation;
using MonoCross.Webkit;

using BestSellers;

namespace BestSellers.Webkit.Views
{
    public class BookView : MXWebkitView<Book>
    {
        public override void Render()
        {
            string markup = "{0}<br><br><b>Publisher: </b>{1}<br>{2}<br><br><b>List Price: </b>${3}<br><br>";
            HtmlGenericControl div = new HtmlGenericControl("div");
            HtmlGenericControl block = new HtmlGenericControl("div");
            block.Attributes.Add("class", "iBlock");

            HtmlGenericControl content = new HtmlGenericControl("div");
            content.Attributes.Add("style", "padding:7px");

            HtmlGenericControl image = new HtmlGenericControl("img");
            image.Attributes.Add("src", Model.AmazonImageUrl);
            image.Attributes.Add("align", "left");
            image.Attributes.Add("style", "max-height:125px;max-width:75px:padding:5px");

            content.InnerHtml = string.Format(markup, Model.Contributor, Model.Publisher, Model.Description, Model.Price);

            content.Controls.AddAt(0,image);
            block.Controls.Add(content);
            div.Controls.Add(block);

            if (!string.IsNullOrEmpty(Model.BookReviewLink))
            {
                HtmlGenericControl menu = new HtmlGenericControl("div");
                menu.Attributes.Add("class", "iMenu");
                HtmlGenericControl ul = new HtmlGenericControl("ul");
                ul.Attributes.Add("class", "iArrow");
                ul.Attributes.Add("style", "background-color:#FFFFFF;color:#000000");
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("href", Model.BookReviewLink);
                HtmlGenericControl em = new HtmlGenericControl("em");
                li.Controls.Add(a);
                em.InnerText = "View Book Review...";
                a.Controls.Add(em);
                ul.Controls.Add(li);
                menu.Controls.Add(ul);
                div.Controls.Add(menu);
            }

            if (!string.IsNullOrEmpty(Model.SundayReviewLink))
            {
                HtmlGenericControl menu = new HtmlGenericControl("div");
                menu.Attributes.Add("class", "iMenu");
                HtmlGenericControl ul = new HtmlGenericControl("ul");
                ul.Attributes.Add("class", "iArrow");
                ul.Attributes.Add("style", "background-color:#FFFFFF;color:#000000");
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("href", Model.SundayReviewLink);
                HtmlGenericControl em = new HtmlGenericControl("em");
                li.Controls.Add(a);
                a.Controls.Add(em);
                em.InnerText = "View Sunday Review...";
                ul.Controls.Add(li);
                menu.Controls.Add(ul);
                div.Controls.Add(menu);
            }

            if (!string.IsNullOrEmpty(Model.ArticleChapterLink))
            {
                HtmlGenericControl menu = new HtmlGenericControl("div");
                menu.Attributes.Add("class", "iMenu");
                HtmlGenericControl ul = new HtmlGenericControl("ul");
                ul.Attributes.Add("class", "iArrow");
                ul.Attributes.Add("style", "background-color:#FFFFFF;color:#000000");
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("href", Model.ArticleChapterLink);
                HtmlGenericControl em = new HtmlGenericControl("em");
                li.Controls.Add(a);
                a.Controls.Add(em);
                em.InnerText = "View Article Chapter...";
                ul.Controls.Add(li);
                menu.Controls.Add(ul);
                div.Controls.Add(menu);
            }

            WriteAjaxToResponse("BookDetail", Model.Title, div);
        }
    }
}