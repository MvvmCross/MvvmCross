using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.Console.Views
{
    class CustomerEdit: MXConsoleView<Customer>
    {
        public override void Render()
        {
            System.Console.Clear();
            System.Console.WriteLine("Customer Details");
            System.Console.WriteLine();

            System.Console.Write("Name: (" + Model.Name + ") New Name: ");
            string input = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                Model.Name = input;

            System.Console.Write("Website: (" + Model.Website + ") New Website: ");
            input = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                Model.Website = input;

            System.Console.Write("Primary Phone: (" + Model.PrimaryPhone + ") New Primary Phone: ");
            input = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                Model.PrimaryPhone = input;

            System.Console.WriteLine();
            System.Console.WriteLine("New Customer Info");
            System.Console.WriteLine("Name: " + Model.Name);
            System.Console.WriteLine("Web: " + Model.Website);
            System.Console.WriteLine("Phone: " + Model.PrimaryPhone);
            System.Console.WriteLine();
            System.Console.WriteLine("Enter to Cancel - (S)ave");
            input = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && string.Equals(input, "S")) {
                string action = string.Equals(Model.ID, "0") ? "CREATE": "UPDATE";
                this.Navigate("Customers/" + Model.ID + "/" + action);
            } else {
                this.Back();
            }
        }
    }
}
