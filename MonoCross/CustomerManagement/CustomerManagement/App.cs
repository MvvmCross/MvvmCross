﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;

using CustomerManagement.Controllers;

namespace CustomerManagement
{
    public class App : MXApplication
    {
        public override void OnAppLoad()
        {
			// Set the application title
            Title = "Customer Management";

            // Add navigation mappings
            NavigationMap.Add("Customers", new CustomerListController());

			CustomerController customerController = new CustomerController();
            NavigationMap.Add("Customers/{customerId}", customerController);
            NavigationMap.Add("Customers/{customerId}/{Action}", customerController);

            // Set default navigation URI
            NavigateOnLoad = "Customers";
        }
    }
}