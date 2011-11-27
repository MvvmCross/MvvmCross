﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
﻿using Cirrious.MonoCross.Extensions.Application;
﻿using Cirrious.MonoCross.Extensions.Interfaces.Conventions;
﻿using MonoCross.Navigation;
using Cirrious.MonoCross.Extensions.Interfaces;

using CustomerManagement.Controllers;

namespace CustomerManagement
{
    public class App : MXConventionBasedApplication
    {
        public App()
        {
			// Set the application title
            Title = "Customer Management";

            // Add navigation mappings
            var controllers = new List<IMXConventionBasedController>();

            controllers.Add(new CustomerListController());
            controllers.Add(new CustomerController());

            base.AddEmptyRoute(controllers.First());
            base.AddRoutesbyConvention(controllers);

            // Set default navigation URI
            NavigateOnLoad = "";
        }
    }
}