using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.Console.Views
{
    class CustomerView : MXConsoleView<Customer>
    {
        public override void Render()
        {
            System.Console.Clear();
            System.Console.WriteLine("Customer Details");
            System.Console.WriteLine();

            System.Console.WriteLine(Model.Name);
            System.Console.WriteLine(string.Format("{0} {1}", Model.PrimaryAddress.Street1, Model.PrimaryAddress.Street2));
            System.Console.WriteLine(string.Format("{0}, {1}  {2}", Model.PrimaryAddress.City, Model.PrimaryAddress.State, Model.PrimaryAddress.Zip));
            System.Console.WriteLine("Previous Orders: " + Model.Orders.Count.ToString());
            System.Console.WriteLine("Addresses:" + Model.Addresses.Count.ToString()); 
            System.Console.WriteLine("Contacts: " + Model.Contacts.Count.ToString());
            System.Console.WriteLine();
            System.Console.WriteLine("Web: " + Model.Website);
            System.Console.WriteLine("Phone: " + Model.PrimaryPhone);
            System.Console.WriteLine();
            System.Console.WriteLine("Enter to Continue, (D)elete or (E)dit");

            while (true)
            {
                string input = System.Console.ReadLine().Trim();
                if (input.Length == 0)
                {
                    this.Back();
                    return;
                }
                else if (input.StartsWith("E"))
                {
                    this.Navigate("Customers/" + Model.ID + "/EDIT");
                }
                else if (input.StartsWith("D"))
                {
                    this.Navigate("Customers/" + Model.ID + "/DELETE");
                }
            }
        }
    }
}
