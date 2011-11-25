using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.Console.Views
{
    public class CustomerListView : MXConsoleView<List<Customer>>
    {
        public override void Render()
        {
            // Output Customer List to Console
            System.Console.Clear();
            System.Console.WriteLine("Customers");
            System.Console.WriteLine();

            int index = 1;
            foreach (Customer customer in Model)
            {
                System.Console.WriteLine(index.ToString() + ". " + customer.Name);
                index++;
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Enter Customer Index, (N)ew Customer or Enter to go Back");

            // Input Actions from Console
            do
            {
                string input = System.Console.ReadLine().Trim();
                
                if (input.Length == 0)
                {
                    this.Back();
                    return;
                }

                if (int.TryParse(input, out index) && index > 0 && index <= Model.Count)
                {
                    this.Navigate(string.Format("Customers/{0}", Model[index - 1].ID));
                    return;
                }
                else if (string.Equals(input, "N"))
                {
                    this.Navigate("Customers/NEW");
                    return;
                }
                else
                {
                    System.Console.WriteLine("Invalid input, retry input or Enter to go back");
                }

            } while (true);
        }
    }
}
                                                                                    