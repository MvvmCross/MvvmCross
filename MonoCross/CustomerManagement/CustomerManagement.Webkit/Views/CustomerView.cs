using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using MonoCross.Navigation;
using MonoCross.Webkit;

using Webkit.Dialog;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Webkit.Views
{
    public class CustomerView : MXWebkitView<Customer>
    {
        public override void Render()
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "iMenu");

            HtmlGenericControl contactHeader = new HtmlGenericControl("h3");
            contactHeader.InnerText = "Contact Info";
            HtmlGenericControl contact = new HtmlGenericControl("ul");
            contact.Controls.Add(LabelItem("ID", Model.ID));
            contact.Controls.Add(LabelItem("Name", Model.Name));
            contact.Controls.Add(LinkItem(Model.Website, "Website", Model.Website));
            contact.Controls.Add(LinkItem(string.Format("tel:{0}", Model.PrimaryPhone), "Primary Phone", Model.PrimaryPhone));

            HtmlGenericControl addressHeader = new HtmlGenericControl("h3");
            addressHeader.InnerText = "Primary Address";
            HtmlGenericControl address = new HtmlGenericControl("ul");
            address.Controls.Add(BlockItem("Address", string.Format("{0}<br>{1} {2}<br>{3}, {4}  {5}", 
                                                                    Model.PrimaryAddress.Description, 
                                                                    Model.PrimaryAddress.Street1,
                                                                    Model.PrimaryAddress.Street2,
                                                                    Model.PrimaryAddress.City,
                                                                    Model.PrimaryAddress.State,
                                                                    Model.PrimaryAddress.Zip
                                                                    )));
            address.Controls.Add(LabelItem("Previous Orders", Model.Orders.Count.ToString()));
            address.Controls.Add(LabelItem("Addresses", Model.Addresses.Count.ToString()));

            div.Controls.Add(contact);
            div.Controls.Add(address);

            div.Controls.Add(DeleteButton(string.Format("/Customers/{0}/{1}", Model.ID, "DELETE"), "Delete Customer", false));
            div.Controls.Add(EditButton(string.Format("/Customers/{0}/{1}", Model.ID, "EDIT"), "Change Customer", true));
            
            WriteAjaxToResponse("ViewCustomer", "Customer Details", div);
        }
        static HtmlGenericControl LabelItem(string caption, string value)
        {
            HtmlGenericControl item = new HtmlGenericControl("li");
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.InnerText = value;
            item.InnerText = caption;
            item.Controls.Add(span);
            return item;
        }
        static HtmlGenericControl BlockItem(string caption, string html)
        {
            HtmlGenericControl item = new HtmlGenericControl("li");
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "iBlock");
            div.Attributes.Add("style", "font-weight:normal");
            div.InnerHtml = html;
            item.InnerText = caption;
            item.Controls.Add(div);
            return item;
        }
        static HtmlGenericControl LinkItem(string link, string caption, string value)
        {
            HtmlGenericControl item = new HtmlGenericControl("li");
            HtmlGenericControl a = new HtmlGenericControl("a");
            HtmlGenericControl span = new HtmlGenericControl("span");
            a.Attributes.Add("href", link);
            a.Attributes.Add("rev", "async");
            span.InnerText = value;
            a.InnerText = caption;
            a.Controls.Add(span);
            item.Controls.Add(a);
            return item;
        }
        //<a href="javascript:void(0)" class="iPush iBCancel" style="width:100%">Black Button</a>
        static HtmlGenericControl DeleteButton(string link, string caption, bool async)
        {
            HtmlGenericControl a = new HtmlGenericControl("a");
            a.Attributes.Add("href", link);
            if (async) a.Attributes.Add("rev", "async");
            a.Attributes.Add("class", "iPush iBWarn");
            a.Attributes.Add("style", "width:100%");
            a.InnerText = caption;
            return a;
        }
        //<a href="javascript:void(0)" class="iPush iBCancel" style="width:100%">Black Button</a>
        static HtmlGenericControl EditButton(string link, string caption, bool async)
        {
            HtmlGenericControl a = new HtmlGenericControl("a");
            a.Attributes.Add("href", link);
            if (async) a.Attributes.Add("rev", "async");
            a.Attributes.Add("class", "iPush iBClassic");
            a.Attributes.Add("style", "width:100%");
            a.InnerText = caption;
            return a;
        }
    }
}