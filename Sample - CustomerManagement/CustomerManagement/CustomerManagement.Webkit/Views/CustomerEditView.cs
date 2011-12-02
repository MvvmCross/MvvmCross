using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MonoCross.Navigation;
using MonoCross.Webkit;

using Webkit.Dialog;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Webkit.Views
{
    public class CustomerEditView : MXWebkitDialogView<Customer>
    {
        public override void Render()
        {
            this.Root = new RootElement("Customer Info", "customerForm", "Customers", string.Format("/Customers/{0}/{1}", Model.ID, Model.ID == "0" ? "CREATE" : "UPDATE"), false)
            {
                new Section("Contact Info")
                {
                    new StringElement("ID", Model.ID),
                    new TextElement("Name", Model.Name, "Name"),
                    new TextElement("Website", Model.Website, "Website"),
                    new TextElement("Primary Phone", Model.PrimaryPhone, "PrimaryPhone")
                },
                new Section("Primary Address")
                {
                    new TextElement("Address 1", Model.PrimaryAddress.Street1, "PrimaryAddress.Street1"),
                    new TextElement("Address 2", Model.PrimaryAddress.Street2, "PrimaryAddress.Street2"),
                    new TextElement("City", Model.PrimaryAddress.City, "PrimaryAddress.City"),
                    new TextElement("State", Model.PrimaryAddress.State, "PrimaryAddress.State"),
                    new TextElement("Zip", Model.PrimaryAddress.Zip, "PrimaryAddress.Zip")
                }
            };            
            WriteRootToResponse("EditCustomer", "Edit Customer", Root);
        }
    }
}