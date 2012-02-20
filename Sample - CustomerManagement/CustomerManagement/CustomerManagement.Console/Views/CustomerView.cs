using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Console.Views;
using Cirrious.MvvmCross.ShortNames;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Console.Views
{
    class CustomerView : MvxConsoleView<DetailsCustomerViewModel>
    {
        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            System.Console.Clear();
            System.Console.WriteLine("Customer Details");
            System.Console.WriteLine();

            System.Console.WriteLine(ViewModel.Customer.Name);
            System.Console.WriteLine(string.Format("{0} {1}", ViewModel.Customer.PrimaryAddress.Street1, ViewModel.Customer.PrimaryAddress.Street2));
            System.Console.WriteLine(string.Format("{0}, {1}  {2}", ViewModel.Customer.PrimaryAddress.City, ViewModel.Customer.PrimaryAddress.State, ViewModel.Customer.PrimaryAddress.Zip));
            //System.Console.WriteLine("Previous Orders: " + ViewModel.Customer.Orders.Count.ToString());
            //System.Console.WriteLine("Addresses:" + ViewModel.Customer.Addresses.Count.ToString()); 
            //System.Console.WriteLine("Contacts: " + ViewModel.Customer.Contacts.Count.ToString());
            System.Console.WriteLine();
            System.Console.WriteLine("Web: " + ViewModel.Customer.Website);
            System.Console.WriteLine("Phone: " + ViewModel.Customer.PrimaryPhone);
            System.Console.WriteLine();
            System.Console.WriteLine("To call the customer, (P)HONE");
            System.Console.WriteLine("To visit the website, (W)WW");
            System.Console.WriteLine("To see the location, (M)AP");
            System.Console.WriteLine("(D)ELETE, (E)DIT, or (B)ACK");
        }

        public override bool HandleInput(string input)
        {
            input = input.Trim().ToUpper();
            switch (input)
            {
                case "DELETE":
                case "D":
                    ViewModel.DeleteCommand.Execute();
                    return true;
                case "EDIT":
                case "E":
                    ViewModel.EditCommand.Execute();
                    return true;
                case "MAP":
                case "M":
                    ViewModel.ShowOnMapCommand.Execute();
                    return true;
                case "WWW":
                case "W":
                    ViewModel.ShowWebsiteCommand.Execute();
                    return true;
                case "PHONE":
                case "P":
                    ViewModel.CallCustomerCommand.Execute();
                    return true;
            }
            return base.HandleInput(input);
        }
    }
}
