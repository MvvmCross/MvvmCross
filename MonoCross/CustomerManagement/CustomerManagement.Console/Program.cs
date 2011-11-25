using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;

using CustomerManagement;
using CustomerManagement.Controllers;

using CustomerManagement.Shared;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize container
            MXConsoleContainer.Initialize(new CustomerManagement.App());

            // customer list view
            MXConsoleContainer.AddView<List<Customer>>(new Views.CustomerListView(), ViewPerspective.Default);

            // customer view and customer edit/new
            MXConsoleContainer.AddView<Customer>(new Views.CustomerView(), ViewPerspective.Default);
            MXConsoleContainer.AddView<Customer>(new Views.CustomerEdit(), ViewPerspective.Update);

            // navigate to first view
            MXConsoleContainer.Navigate(MXContainer.Instance.App.NavigateOnLoad);
        }
    }
}
