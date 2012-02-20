using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Console.Views;
using CustomerManagement.Core.Models;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Console.Views
{
    public class CustomerListView : MvxConsoleView<CustomerListViewModel>
    {
        protected override void OnViewModelChanged()
        {
            // Output Customer List to Console
            System.Console.Clear();
            System.Console.WriteLine("Customers");
            System.Console.WriteLine();

            int index = 1;
            foreach (Customer customer in ViewModel.Customers)
            {
                System.Console.WriteLine(index.ToString() + ". " + customer.Name);
                index++;
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Enter Customer Index Number, (N)EW Customer or (B)ACK to go Back");
        }

        public override bool  HandleInput(string input)
        {
            input = input.Trim().ToUpper();
            switch (input)
            {
                case "NEW":
                case "N":
                    ViewModel.AddCommand.Execute();
                    return true;
                default:
                    int index;
                    if (int.TryParse(input, out index) 
                        && index > 0 
                        && index <= ViewModel.Customers.Count)
                    {
                        ViewModel.CustomerSelectedCommand.Execute(ViewModel.Customers[index - 1]);
                        return true;
                    }
                    break;
            }
            return base.HandleInput(input);
        }
    }
}
                                                                                    